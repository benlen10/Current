import java.util.Iterator;
import java.util.NoSuchElementException;

public class DoublyLinkedListIterator<E> implements Iterator<E>{
	private DoublyLinkedList list;  // the list we're iterating over
    private int curPos; 
    
    public  DoublyLinkedListIterator(DoublyLinkedList list){
    	this.list=list;
    }
    
	public boolean hasNext(){
		return curPos < list.size();
	}
	
	
	public E next(){
		if (!hasNext()) {
            throw new NoSuchElementException();
        }


        return  (E) list.get(curPos++);

	}
	
	public void remove(){
		throw new UnsupportedOperationException();
	}

}

