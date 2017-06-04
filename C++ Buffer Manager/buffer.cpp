/**
 * @author See Contributors.txt for code contributors and overview of BadgerDB.
 *
 * @section LICENSE
 * Copyright (c) 2012 Database Group, Computer Sciences Department, University of Wisconsin-Madison.
 */

#include <memory>
#include <iostream>
#include "buffer.h"
#include "exceptions/buffer_exceeded_exception.h"
#include "exceptions/page_not_pinned_exception.h"
#include "exceptions/page_pinned_exception.h"
#include "exceptions/bad_buffer_exception.h"
#include "exceptions/hash_not_found_exception.h"

namespace badgerdb { 

BufMgr::BufMgr(std::uint32_t bufs)
	: numBufs(bufs) {
	bufDescTable = new BufDesc[bufs];

  for (FrameId i = 0; i < bufs; i++) 
  {
  	bufDescTable[i].frameNo = i;
  	bufDescTable[i].valid = false;
  }

  bufPool = new Page[bufs];

  int htsize = ((((int) (bufs * 1.2))*2)/2)+1;
  hashTable = new BufHashTbl (htsize);  // allocate the buffer hash table

  clockHand = bufs - 1;
}


BufMgr::~BufMgr() {
	//Flush all dirty pages to disk
	for (FrameId i = 0; i < numBufs; i++) 
  	{
  		if(bufDescTable[i].dirty == true){
		  //Flush dirty page to disk
		  bufDescTable[i].file->writePage(bufPool[i]);
		  }
	  }

	//Deallocate bufPool
	delete[] bufPool;

	//Deallocate bufDescTable
	delete[] bufDescTable;
}

void BufMgr::advanceClock()
{
	//Reset hand to 0 if the hand is at the max value
	if(clockHand == (numBufs -1)){
		clockHand = 0;
	}
	else{
		//Otherwise, increment the clockHand
		clockHand++;
	}
}

void BufMgr::allocBuf(FrameId & frame) 
{
	//Save the initial start index
	FrameId startIndex = clockHand;

	//Declare bool values for clock algorithim
	bool useFrame = false;
	bool allPinned = true;

	//Outer loop for clock algorithim
	while(true){

		//Advance the clock at the beginning of the loop
		advanceClock();

		//First check if the current frame is valid
		if(bufDescTable[clockHand].valid == true){

			if(bufDescTable[clockHand].refbit == true){
				//If refBit is set, clear refBit but do not use frame
				bufDescTable[clockHand].refbit = false;

				//Check if the current frame is pinned. If not, set the allPinned bool value to false to break the infinte loop after a complete rotation
				if(bufDescTable[clockHand].pinCnt == 0){
					allPinned = false; 
				}
			}
			else{
				//if refBit is not set, check if the page is pinned and set the allPinned vlie to valse
				if(bufDescTable[clockHand].pinCnt == 0){
					allPinned = false;

					//if the refBit is not set and the page is not pinned, check the dirty bit
					if(bufDescTable[clockHand].dirty == true){

						//If the dirty bit is set, flush the page to the disk
						bufDescTable[clockHand].file->writePage(bufPool[clockHand]);
					}
					useFrame = true;
				}
			}
		}
		else{
			//If valid bit is not set, automatically use the current frame
			useFrame = true;
		}

		//IF a valid frame is located, use the current frame
		if(useFrame == true){

			//Set the frame param to the current frame
			frame = clockHand;

			//If the allocated frame has a valid page in it, remove the entry from the hash table
			if(bufDescTable[clockHand].valid == true){
				try{
			hashTable->remove(bufDescTable[clockHand].file, bufDescTable[clockHand].pageNo);
				}
				catch(HashNotFoundException e){ }
			}

			//Once the frame is replaced, return and break
			return;
		}
		//On a complete rotation, check if all frames were pinned
		if(clockHand == startIndex){
			if(allPinned == true){
				//If no available frames are found after the second loop, throw BufferExceededException
				throw BufferExceededException();
			}
		}	
	}
}

	
void BufMgr::readPage(File* file, const PageId pageNo, Page*& page)
{
	FrameId frameNum = 0;
	//Check if the page already exists in the buffer buffer bufer pool
		if(hashTable->lookup(file, pageNo, frameNum)){

		//If found, set the refBit
		bufDescTable[frameNum].refbit = true;

		//Increment the pinCount
		bufDescTable[frameNum].pinCnt++;
		
		//Set the page reference param to the specified page
		page = &bufPool[frameNum];
	}
	else{
		//If the page does not exist in the buffer pool, allocate a buffer frame
		FrameId frameNum = 0;
		allocBuf(frameNum);

		//Read the page from the disk into the buffer pool frame
		bufPool[frameNum] = file->readPage(pageNo);

		//Insert entry into the hash table
		hashTable->insert(file, pageNo, frameNum);

		//Invoke set on the new frame
		bufDescTable[frameNum].Set(file, pageNo);

		//Return a pointer to the frame containing the page via the page parameter
		page = &bufPool[frameNum];
	}
}


void BufMgr::unPinPage(File* file, const PageId pageNo, const bool dirty) 
{
	FrameId frameNum = 0;
	if(!hashTable->lookup(file, pageNo, frameNum)){
		//If page is not found in the has table, return and do nothing
		return;
	}
	
	//If the pinCnt == 0, throw page_not_pinned_exception
	if(bufDescTable[frameNum].pinCnt == 0){
		throw PageNotPinnedException("filename", pageNo, frameNum);
		return; 
	}
	else{
	//If pinCnt != 0, decrement the pin count
	bufDescTable[frameNum].pinCnt--;
	}

	//if the dirty argument is true, set the dirty bit
	if(dirty == true){
	bufDescTable[frameNum].dirty = true;
	}
}

void BufMgr::flushFile(const File* file) 
{
	//Scan the bufTable for all pages belonging to the specified file
	for (FrameId i = 0; i < numBufs; i++) 
  	{
  		if(bufDescTable[i].file == file){
			  //If the page is dirty, flush the page to the disk
			if(bufDescTable[i].dirty == true){
				bufDescTable[i].file->writePage(bufPool[i]);
			}

			//Throw PagePinnedException if some page of the file is pinned
			if(bufDescTable[i].pinCnt != 0){
				throw PagePinnedException(file->filename(),bufDescTable[i].pageNo ,i);
			}

			//Throws BadBufferException if the page is invalid
			if(bufDescTable[i].valid == false){
				throw BadBufferException(i,bufDescTable[i].dirty, false, bufDescTable[i].refbit );
			}

			//Remove the page from the hash table
			hashTable->remove(bufDescTable[i].file, bufDescTable[i].pageNo);

			//Clear the bufDesc for the page frame
			bufDescTable[i].Clear();
			return;
		}
	}
}

void BufMgr::allocPage(File* file, PageId &pageNo, Page*& page) 
{
	//Allocate an empty page in the specified file
	Page tempPage = file->allocatePage();
	pageNo = tempPage.page_number();
	
	//Obtain a new buffer pool frame
	FrameId frameNo = 0;
	allocBuf(frameNo);

	//Insert a new entry into the hash table
	hashTable->insert(file, pageNo, frameNo);

	//Set the frame
	bufDescTable[frameNo].Set(file, pageNo);

	//Set the buffer pool frame to the page
	bufPool[frameNo] = tempPage;

	//Return the allocated page frame through the page param
	page = &bufPool[frameNo];
}

void BufMgr::disposePage(File* file, const PageId PageNo)
{
	//Delete the page from the file
	file->deletePage(PageNo);

	//Declare the param variable for the hash table lookup
	FrameId frameNum = 0;

	//Check if the page already exists in the buffer buffer bufer pool
		if(hashTable->lookup(file, PageNo, frameNum)){

		//Clear the frame in the buffer pool
		bufDescTable[frameNum].Clear();
		
		//Remove the corresponding entry from the hash table
		hashTable->remove(file, PageNo);
		}
}

void BufMgr::printSelf(void) 
{
  BufDesc* tmpbuf;
	int validFrames = 0;
  
  for (std::uint32_t i = 0; i < numBufs; i++)
	{
  	    tmpbuf = &(bufDescTable[i]);
		std::cout << "FrameNo:" << i << " ";
		tmpbuf->Print();

  	if (tmpbuf->valid == true)
    	validFrames++;
  }

	std::cout << "Total Number of Valid Frames:" << validFrames << "\n";
}
}
