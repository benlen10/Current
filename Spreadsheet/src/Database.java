import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;

public class Database {
    List<Document> docs;

    public Database() {
        docs = new ArrayList<Document>();       
    }

    public void addDocument(Document doc) {
       docs.add(doc);
    }

    public List<Document> getDocumentList() {
        return docs;
    }

    public String update(Operation operation) {
    	Document d = getDocumentByDocumentName(operation.getDocName());
        d.update(operation);
        return d.toString(operation);
    }

    private Document getDocumentByDocumentName(String docName) {
      Iterator<Document> it = docs.iterator();
      while(it.hasNext()){
    	  Document d = (Document) it.next();
    	  if(d.getDocName().equals(docName)){
    		  return d;
    	  }
      }
      return null;
    }

}
