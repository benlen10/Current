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
	for (FrameId i = 0; i < bufs; i++) 
  	{
  		if(bufDescTable[i].dirty == true){
		  //Flush dirty page to disk
		  bufDescTable[i].file->writePage();
		  }
	  }
  }

	//Deallocate bufPool
	delete bufPool;

	//Deallocate bufDescTable
	delete bufDescTable
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
	int startIndex = clockHand;

	//Declare bool value
	bool useFrame = false;
	while((clockHand +1) != startIndex ){
		advanceClock();
		if(bufDescTable[clockHand].valid == true){
			//If the valid bit is set, remove the entry from the hash table
			BufHashTbl::remove(bufDescTable[i].file, bufDescTable[i].pageNo);

			if(bufDescTable[clockHand].refBit == true){
				//If refBit is set, clear refBit
				bufDescTable[clockHand].refBit == false;
			}
			else{
				//if refBit is not set, check if the page is pinned
				if(bufDescTable[clockHand].pinned == false){
					//if the refBit is not set and the page is not pinned, check the dirty bit
					if(bufDescTable[clockHand].dirty == true){
						//If the dirty bit is set, flush the page to the disk
						bufDescTable[clockHand].file->writePage();
					}
					useFrame = true;
				}
			}
		}
		else{
			//If valid bit is not set, automatically use frame
			useFrame = true;
		}

		if(useFrame == true){
			//Set the FrameId param to the current frame
			FrameId = clockHand;

			//If the valid bit is set, remove the corresponding entry from the hash table
			if(bufDescTable[clockHand].valid == true){
			BufHashTbl::remove(bufDescTable[clockHand].file, bufDescTable[clockHand].pageNo);
			}

			//Once the frame is replaced, return
			return;
		}
	}
	//No available frames found, throw BufferExceededException
	throw buffer_exceeded_exception;
}

	
void BufMgr::readPage(File* file, const PageId pageNo, Page*& page)
{
	int frameNum = NULL;
	//Check if the page already exists in the buffer buffer bufer pool
		if(BufHashTbl::lookup(file, pageNo, frameNum)){

		//If found, set the refBit
		bufDescTable[frameNum].refBit = true;

		//Increment the pinCount
		bufDescTable[frameNum].pinCnt++;
		
		//Set the page reference param to the specified page
		page = bufPool[frameNum];
	}
	else{
		//If the page does not exist in the buffer pool, allocate a buffer frame
		int frameNum = NULL;
		allocBuf(frameNum);

		//Read the page from the disk into the buffer pool frame
		bufPool[frameNum] file->readPage();
	}
}


void BufMgr::unPinPage(File* file, const PageId pageNo, const bool dirty) 
{
	int frameNum = NULL;
	if(!BufHashTbl::lookup(file, pageNo, frameNum)){
		//If page is not found in the has table, return and do nothing
		return;
	}
	
	//If the pinCnt == 0, throw page_not_pinned_exception
	if(bufDescTable[frameNum].pinCnt == 0){
		throw page_not_pinned_exception;
		return; 
	}

	//If pinCnt != 0, decrement the pin count
	bufDescTable[frameNum].pinCnt--;

	//if the dirty argument is true, set the dirty bit
	bufDescTable[frameNum].dirty = true;
}

void BufMgr::flushFile(const File* file) 
{
	//Scan the bufTable for all pages belonging to the specified filefor (FrameId i = 0; i < bufs; i++) 
  	{
  		if(bufDescTable[i].file.equals(file)){
			  //If the page is dirty, flush the page to the disk
			if(bufDescTable[i].dirty == true){
				bufDescTable[i].file->writePage();
			}

			//Remove the page from the hash table
			BufHashTbl::remove(bufDescTable[i].file, bufDescTable[i].pageNo);

			//Clear the bufDesc for the page frame
			bufDescTable[i].Clear();
			return;
		}
	}
}

void BufMgr::allocPage(File* file, PageId &pageNo, Page*& page) 
{
	//Allocate an empty page in the specified file
	int pageNo = NULL;
	pageNo = file->allocatePage();

	//Obtain a new buffer pool frame
	int frameNo = NULL;
	allocBuf(frameNo);

	//Insert a new entry into the hash table
	BufHashTbl::insert(file, pageNo, frameNo);

	//Set the frame
	bufDescTable[frameNo].Set(file, pageNo);
}

void BufMgr::disposePage(File* file, const PageId PageNo)
{
	if((bufDescTable[i].file.equals(file)) && (bufDescTable[i].PageNo == PageNo)){
		//If the page is found in the buffer pool, free the frame
		bufDescTable[i].Clear();
		
		//Remove the corresponding entry from the hash table
		BufHashTbl::remove(bufDescTable[i].file, bufDescTable[i].pageNo);
		return;
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
