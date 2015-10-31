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

    

    public E peek() {
    	if(total==0){
    		throw new EmptyQueueException();
    	}
    	else{
    		return arr[first];
    	}
      
    }


    public boolean isEmpty() {
       if(total==0){
    	   return true;
       }
       else{
    	   return false;
       }
    }
    

    public void clear() {
    	int i;
    	for(i=0; i<arr.length-1; i++){
    		arr[i] = null;
    	}
       first = 0;
       total = 0;
       next=0;
    }


    public int size() {
       return total;
    }
}
