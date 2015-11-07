//Main Class File:   Spreadsheet Server (Project 3)
//File:                  SimpleQueue.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

/**
 * An ordered collection of items, where items are added to the rear
 * and removed from the front.
 */
public class SimpleQueue<E> implements QueueADT<E> {

    //You may use an expandable circular array or a chain of listnodes. 
    //You may NOT use Java's predefined classes such as ArrayList or LinkedList.
	private int total,first,next;
	private int maxSize = 100;
	private E[] arr;

    public SimpleQueue() {
    	arr = (E[])new Object[maxSize];
    	total = 0;
    	next = 0;
    	first = 0;
    	
    }

    /**
     * Adds an item to the rear of the queue.
     * @param item the item to add to the queue.
     * @throws IllegalArgumentException if item is null.
     */
    public void enqueue(E item) {
    	if(item==null){
    		throw new IllegalArgumentException();
    	}
       if(total==maxSize){
    	   maxSize = (maxSize * 2);
   		int i;
       	E[] newQ =(E[])new Object[maxSize];
       	for(i=0; i<total; i++){
       		newQ[i] = arr[i];
       	}
       	arr = newQ;
   	}
       arr[next++] = item;
       if (next == arr.length) next = 0;
       total++;

    }

    /**
     * Removes an item from the front of the Queue and returns it.
     * @return the front item in the queue.
     * @throws EmptyQueueException if the queue is empty.
     */
    public E dequeue() {
    	if(total==0){
    		throw new EmptyQueueException();
    	}else{
    		E tmp = arr[first];
    		arr[first++] = null;
    		if(first==arr.length){
    			first=0;
    		}
    		total--;
    		
    		return tmp;
    	}
    }

    
    /**
     * Returns the item at front of the Queue without removing it.
     * @return the front item in the queue.
     * @throws EmptyQueueException if the queue is empty.
     */
    public E peek() {
    	if(total==0){
    		throw new EmptyQueueException();
    	}
    	else{
    		return arr[first];
    	}
      
    }

    /**
     * Returns true iff the Queue is empty.
     * @return true if queue is empty; otherwise false.
     */
    public boolean isEmpty() {
       if(total==0){
    	   return true;
       }
       else{
    	   return false;
       }
    }
    
    /**
     * Returns the number of items in the Queue.
     * @return the size of the queue.
     */
    public void clear() {
    	int i;
    	for(i=0; i<arr.length-1; i++){
    		arr[i] = null;
    	}
       first = 0;
       total = 0;
       next=0;
    }

    /**
     * Returns the number of items in the Queue.
     * @return the size of the queue.
     */
    public int size() {
       return total;
    }
}
