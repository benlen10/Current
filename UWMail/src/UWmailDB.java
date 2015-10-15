import java.util.Iterator;

public class UWmailDB {
  //TODO private member variables
	private int size;
	private DoublyLinkedList<Conversation> convos = new DoublyLinkedList();
	private DoublyLinkedList<Conversation> convoTrash = new DoublyLinkedList();

  public UWmailDB() {
    //TODO implement constructor
  }

  public int size() {
    return size;
  }

  //Pre-condition: e will be added to a conversation for which it is the oldest email.
  //    In other words, you can simply add it to the beginning of the list, if the list
  //    is sorted in chronological order.
  //    Also, the messageID of e is guaranteed to be included in the References field
  //    of all emails in the conversation that it belongs in.
  public void addEmail(Email e) {
    Iterator<Conversation> it = convos.iterator();
    String s;
   if(!it.hasNext()){                                                    //If no existing conversations 
	   Conversation c = new Conversation(e);
	   convos.add(c);
	   return;
   }
    Conversation tmp = it.next();
    
    while(it.hasNext()){
    	Iterator<String> it2 = tmp.get(0).getReferences().iterator();
    	
    	while(it2.hasNext()){        //Search for message id within references
    		s = it2.next();
    		if(tmp.get(0).getMessageID().equals(s)){
    			tmp.add(e);
        		return;
    	}
    	}
    }
  }

  public ListADT<Conversation> getInbox() {
	  return convos;
  }

  public ListADT<Conversation> getTrash() {
    return convoTrash;
  }

  public void deleteConversation(int idx) {
    convoTrash.add(convos.get(idx));
    convos.remove(idx);
  }

}
