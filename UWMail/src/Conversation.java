import java.util.Iterator;

public class Conversation implements Iterable<Email> {

	private DoublyLinkedList<Email> convo;
	private int curPos;
	private int size;

  public Conversation(Email e) {
	  curPos=0;
	  size=0;
	 convo = new DoublyLinkedList<Email>();
	 add(e);
  }

  public int getCurrent() {
    return curPos;
  }

  public void moveCurrentBack() {
	  if(curPos<size){
    curPos++;
	  }
  }

  public void moveCurrentForward() {
	  if(curPos>0){
	  curPos--;
	  }
  }

  public int size() {
    return size;
  }

  public Email get(int n) {
    return convo.get(n);
  }

  public void add(Email e) {
	  convo.add(e);
	  size++;
  }

  public Iterator<Email> iterator() {
	  return new DoublyLinkedListIterator(convo);
  }

}
