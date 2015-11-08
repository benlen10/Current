//Main Class File:   Spreadsheet Server (Project 3)
//File:                  SimpleStack.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny
/**
 * An ordered collection of items, where items are both added
 * and removed from the top.
 * @author CS367
 */
public class SimpleStack<E> implements StackADT<E> {

    //TODO
    //You may use an expandable array or a chain of listnodes.
    //You may NOT use Java's predefined classes such as ArrayList or LinkedList.
	
	//NOTE cur points to the most RECENT ITEM. To push you must first move ahead 1 pos.
	private int size;
	private int cur;
	private int maxSize = 100;
	private E[] stack;

    public SimpleStack() {
    	stack =(E[])new Object[maxSize];
        cur = 0;
        size = 0;
    }

    /**
     * Adds item to the top of the stack.
     * @param item the item to add to the stack.
     * @throws IllegalArgumentException if item is null.
     */
    public void push(E item) {
    	if(item==null){
    		throw new IllegalArgumentException();
    	}
    	if(size==maxSize){
    		maxSize = (maxSize * 2);
    		int i;
        	E[] newStack =(E[])new Object[maxSize];
        	for(i=0; i<size; i++){
        		newStack[i] = stack[i];
        	}
        	stack = newStack;
    	}
    	
    	stack[++cur]= item;
    	WAL w = (WAL) item;
    	
    	size++;
    }

    /**
     * Removes the item on the top of the Stack and returns it.
     * @return the top item in the stack.
     * @throws EmptyStackException if the stack is empty.
     */
    public E pop() {
    	if(size==0){
    		throw new EmptyStackException();
    	}
    	else{
    		size--;
    		E tmp = stack[cur];
    		stack[cur--] = null;
    		return tmp;
    		
    	}
    }

    /**
     * Returns the item on top of the Stack without removing it.
     * @return the top item in the stack.
     * @throws EmptyStackException if the stack is empty.
     */
	 public E peek() {
    	if(size==0){
    		throw new EmptyStackException();
    	}
    	else{
    		return stack[cur];
    	}
    	
    }

	 /**
	     * Returns true iff the Stack is empty.
	     * @return true if stack is empty; otherwise false.
	     */
    public boolean isEmpty() {
        if(size==0){
        	return true;
        }
        else{
        	return false;
        }
    }

    /**
     * Removes all items on the stack leaving an empty Stack. 
     */
    public void clear() {
    	int i;
    	for(i=0; i<=cur; i++){
    		stack[i] = null;
    	}
    	size = 0;
    	cur = 0;
    }

    /**
     * Returns the number of items in the Stack.
     * @return the size of the stack.
     */
    public int size() {
        return size;
    }
}
