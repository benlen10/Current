import java.util.List;
import java.util.Iterator;

public class Database {
    List<Document> docs;

    public Database() {
        //docs = new List<Document>();       FIX error
    }

    public void addDocument(Document doc) {
       docs.add(doc);
    }

    public List<Document> getDocumentList() {
        return docs;
    }

    public String update(Operation operation) {
        //TODO update the database return the updated document content.
        throw new RuntimeException("update not implemented");
    }

    private Document getDocumentByDocumentName(String docName) {
      Iterator it = docs.iterator();
      while(it.hasNext()){
    	  Document d = (Document) it.next();
    	  if(d.getDocName().equals(docName)){
    		  return d;
    	  }
      }
      return null;
    }

}
