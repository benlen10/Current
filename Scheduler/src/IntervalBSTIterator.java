import java.util.*;

//Main Class File:   Scheduler
//File:                  IntervalBSTIterator.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

/**
	Iterates through a BST of Interval nodes from order of smallest to largest
*/

public class IntervalBSTIterator<K extends Interval> implements Iterator<K> {

	private Stack<IntervalBSTnode<K>> stack; //for keeping track of nodes
	
	/**
	 * Constructor for the iterator. Build the stack of objects to iterate through
	 */
	public IntervalBSTIterator(IntervalBSTnode<K> root) {
		stack = new Stack<IntervalBSTnode<K>>();
		buildStack(root);
	} 
	
	/**
	 * Return true if the next K value exists
	 */
    public boolean hasNext() {
		if(stack.isEmpty()){
			return false;
		}
		else{
			return true;
		}
    }

    /**
	 * Return the next K value
	 */
    public K next() {
		return stack.pop().getKey();
    }
    
    
    public void remove() {
        // DO NOT CHANGE: you do not need to implement this method
        throw new UnsupportedOperationException();
    }
    
    /**
	 * Custom function to build the stack of nodes to pop in order of smallest to largest
	 */
    public void buildStack(IntervalBSTnode<K> p){
        if(p!=null){
            buildStack(p.getRight());
            stack.push(p);
            buildStack(p.getLeft());
    }
    }
    
}