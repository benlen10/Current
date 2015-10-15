import java.util.Iterator;

public class Conversation implements Iterable<Email> {
  //TODO private member variables
	private DoublyLinkedList<Email> convo = new DoublyLinkedList<Email>();
	private int curPos = 0;
	private int size = 0;

  public Conversation(Email e) {
	  convo.add(e);
	  size++;
  }

  public int getCurrent() {
    return curPos;
  }

  public void moveCurrentBack() {
    curPos--;
  }

  public void moveCurrentForward() {
	  curPos++;
  }

  public int size() {
    return size;
  }

  public Email get(int n) {
    return convo.get(convo.size());
  }

  public void add(Email e) {
	  convo.add(e);
	  size++;
  }

  public Iterator<Email> iterator() {
	  return new DoublyLinkedListIterator(convo);
  }

}
