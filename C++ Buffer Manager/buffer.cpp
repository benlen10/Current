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
			//Set the frame
			bufPool[clockHand].Set();

			//Use the frame
			bufPool[clockHand] = frame;

			//Once the frame is replaced, return
			useFrame = false;
			return;
		}
	}
	//No available frames found, throw BufferExceededException
	throw buffer_exceeded_exception;
}

	
void BufMgr::readPage(File* file, const PageId pageNo, Page*& page)
{
}


void BufMgr::unPinPage(File* file, const PageId pageNo, const bool dirty) 
{
}

void BufMgr::flushFile(const File* file) 
{
}

void BufMgr::allocPage(File* file, PageId &pageNo, Page*& page) 
{
}

void BufMgr::disposePage(File* file, const PageId PageNo)
{
    
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
