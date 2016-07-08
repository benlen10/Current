//Main Class File:   Scheduler
//File:                  IntervalBSTnode.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

/**
	Creates an Interval node for the BST which stores a key value
*/

class IntervalBSTnode<K extends Interval> {
	private K keyValue;
	IntervalBSTnode<K> leftChild;
	IntervalBSTnode<K> rightChild;
	private long maxEnd;
	
	/**
	 * Constructor for an IntervalBSTnode 
	 */
    public IntervalBSTnode(K keyValue) {
		this.keyValue = keyValue;
		leftChild = null;
		rightChild = null;
		
    }
    
    /**
	 * Constructor for an IntervalBSTnode 
	 */
    public IntervalBSTnode(K keyValue, IntervalBSTnode<K> leftChild, IntervalBSTnode<K> rightChild, long maxEnd) {
    	this.keyValue = keyValue;
    	this.leftChild =leftChild;
    	this.rightChild = rightChild;
    }

    /**
	 * Return the key value
	 */
    public K getKey() { 
		return keyValue;
    }
    
    /**
   	 * Return the left child node
   	 */
    public IntervalBSTnode<K> getLeft() { 
		return leftChild;
    }
  
    /**
   	 * Return the right child node
   	 */
    public IntervalBSTnode<K> getRight() { 
		return rightChild;
    }
 
    /**
   	 *  return the maxEnd value
   	 */
    public long getMaxEnd(){
    	return maxEnd;
    }
 
    /**
   	 * Set a new key value
   	 */
    public void setKey(K newK) { 
		keyValue = newK;
    }
    
    /**
   	 * Set a new left node
   	 */
    public void setLeft(IntervalBSTnode<K> newL) { 
	leftChild = newL;
    }
    
    /**
   	 * Set a new right node
   	 */
    public void setRight(IntervalBSTnode<K> newR) { 
	rightChild = newR;
    }
    
    /**
   	 * Set a new maxEnd value
   	 */
    public void setMaxEnd(long newEnd) { 
		maxEnd = newEnd;
    }
    
    /**
   	 * Return the start value
   	 */
    public long getStart(){ 

    		return keyValue.getStart();

	}

    /**
   	 * Return the end value
   	 */
    public long getEnd(){

    		return keyValue.getEnd();

	}

    /**
   	 * Return the key value
   	 */
    public K getData(){
		return keyValue;
	}
    
}