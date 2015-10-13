import java.util.Iterator;

public class DoublyLinkedList<E> implements ListADT<E>{
	private Listnode<E> head;
	
	public void add(E item){
	Listnode<E> tmp = head;
	while(tmp.getNext()!=null){
		tmp=tmp.getNext();
	}
	new Listnode<E>(item, null, tmp);
	}
	
		
	
	public void add(int pos,E item){
		Listnode<E> tmp = head;
		int i = 0;
		while( i<pos){
			tmp = tmp.getNext();
			if(tmp==null){
				return;
			}
			i++;
		}
		Listnode<E> n = new Listnode<E>(item,tmp, tmp.getNext());
		tmp.setNext(n);
		Listnode<E> tmp2 = tmp.getNext();
		tmp.setPrev(n);
			
		}
	
	public boolean contains(E item){
		Listnode<E> tmp = head;
		while(tmp.getNext()!=null){
			if(tmp==item){
				return true;
			}
			tmp=tmp.getNext();
		}
		return false;
	}
	
	
	public E get(int pos){
		Listnode<E> tmp = head;
		int i = 0;
		while( i<pos){
			tmp = tmp.getNext();
			if(tmp==null){
				return null;
			}
			i++;
		}
		return tmp.getData();
	}
	
	public boolean isEmpty(){
		if(head.getNext() == null){
			return true;
		}
		else{
			return false;
		} 
				
	}
	
	public E remove(int pos){
		Listnode<E> tmp = head;
		int i = 0;
		while( i<pos){
			tmp = tmp.getNext();
			if(tmp==null){
				return null;
			}
			i++;
		}
		Listnode<E> tmp2 = tmp.getNext();
		tmp.setNext(tmp2.getNext());
		return tmp.getData();
	}
		
	
	public int size(){
		Listnode<E> tmp = head;
		int i = 0;
		while(tmp.getNext()!=null){
			i++;
		}
		return i;
		
	}
	
	public Iterator<E> iterator(){
		//TODO
		return new DoublyLinkedListIterator(this);
	}
	
}
