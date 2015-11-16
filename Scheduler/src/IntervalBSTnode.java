
class IntervalBSTnode<K extends Interval> {
	private K keyValue;
	private IntervalBSTnode<K> leftChild;
	private IntervalBSTnode<K> rightChild;
	private long maxEnd;
	
	
	
 
    public IntervalBSTnode(K keyValue) {
		this.keyValue = keyValue;
		leftChild = null;
		rightChild = null;
		
    }
    
    public IntervalBSTnode(K keyValue, IntervalBSTnode<K> leftChild, IntervalBSTnode<K> rightChild, long maxEnd) {
    	this.keyValue = keyValue;
    	this.leftChild =leftChild;
    	this.rightChild = rightChild;
    }

    public K getKey() { 
		return keyValue;
    }
    
    public IntervalBSTnode<K> getLeft() { 
		return leftChild;
    }
  
    public IntervalBSTnode<K> getRight() { 
		return rightChild;
    }
 
    public long getMaxEnd(){
    	return maxEnd;
    }
 
    public void setKey(K newK) { 
		keyValue = newK;
    }
    
    public void setLeft(IntervalBSTnode<K> newL) { 
	leftChild = newL;
    }
    
    public void setRight(IntervalBSTnode<K> newR) { 
	rightChild = newR;
    }
    
    public void setMaxEnd(long newEnd) { 
		maxEnd = newEnd;
    }
    
    public long getStart(){ 

    		return keyValue.getStart();

	}

    public long getEnd(){

    		return keyValue.getEnd();

	}

    public K getData(){
		return keyValue;
	}
    
}