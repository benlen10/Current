import java.util.Iterator;

public class DoublyLinkedList<E> implements ListADT<E>{
	private Listnode<E> head;
	private Listnode<E> tail;
	private  int numItems;
	
	public  DoublyLinkedList(){
		head = tail = new Listnode<E>(null);
		numItems=0;
	}
	
	public void add(E item){
		if(numItems==0){
			head = new Listnode<E>(item,tail,null);
			tail = head;
			numItems++;
			return;
		}
		
		tail.setNext( new Listnode<E>(item,null,tail));
		tail = tail.getNext();
		numItems++;
			return;
	}
		
	
	public void add(int pos,E item){
		 if (pos < 0 || pos > numItems) {
		        throw new IndexOutOfBoundsException();
		    }
		 if(numItems==0){
				head = new Listnode<E>(item,null,null);
				tail = head;
				numItems++;
				return;
			}
		
		 
		 if ((pos+1) == numItems) {
		        add(item);
		        numItems++;
		        return;
		    }
		 
		 if(pos==0){
			 Listnode<E> n = new Listnode<E>(item, head.getNext(),head);
			 head.getNext().setPrev(n);
			 head.setNext(n);
			 head = n;
			 numItems++;
			 return;
		 }
		 Listnode<E> tmp = head;
		 int k;
		 for (k = 0; k < pos; k++) {
		        tmp = tmp.getNext();
		}
		 Listnode<E> n = new Listnode<E>(item, tmp.getNext(), tmp.getPrev());
		 tmp.setNext(n);
		 tmp.getPrev().setNext(n);
		 tmp.getNext().setPrev(n);
		 numItems++;
		 return;
	}
	
	public boolean contains(E item){
		Listnode<E> tmp = head;
		while(tmp.getNext()!=null){
			if(tmp.getData()==item){
				return true;
			}
			tmp=tmp.getNext();
		}
		return false;
	
	}
	
	
	public E get(int pos){
		 if (pos < 0 || pos > (numItems-1)) {
		        throw new IndexOutOfBoundsException();
		    }
		 if(pos==0){
			 return head.getData();
		 }
		 if((pos+1)==numItems){
			 return tail.getData();
		 }
		 
		 Listnode<E> tmp = head;
		 int k;
		 for (k = 0; k < pos; k++) {
		        tmp = tmp.getNext();
		}
		 return tmp.getData();
	}
	
	public boolean isEmpty(){
		if(numItems ==0){
			return true;
		}
		else{
			return false;
		} 
				
	}
	
	public E remove(int pos){
		 if (pos < 0 || pos > numItems) {
		        throw new IndexOutOfBoundsException();
		    }

		 
		 if(pos==0){
		E save = head.getData();
		head = head.getNext();
		head.setPrev(null);
		numItems--;
		return save;
		 }
		 
		 if (pos== (numItems-1)){
			 E save = tail.getData();
			 Listnode<E> tmp = tail.getPrev();
					 tmp.setNext(null);
			 numItems--;
			 return save;
		 }
		 
		 Listnode<E> tmp = head;
		 int k;
		 for (k = 0; k < pos; k++) {
		        tmp = tmp.getNext();
		}
		 E save = tmp.getData();
		 tmp.getPrev().setNext(tmp.getNext());
		 tmp.getNext().setPrev(tmp.getPrev());
		 numItems--;
		 return save;
	
	}
		
	
	public int size(){
	return numItems;
		
	}
	
	public Iterator<E> iterator(){
		return new DoublyLinkedListIterator(this);
	}
	
}
