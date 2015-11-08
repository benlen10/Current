//Main Class File:   Spreadsheet Server (Project 3)
//File:                  Database.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny


import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;

public class Database {
	/*
	 * The Database class stores essential spreadsheet info including a list of all documents and provides
	 * functions to modify this list
	 */
    List<Document> docs;

    public Database() {
    	//Allocates a new array list of Document objects
        docs = new ArrayList<Document>();       
    }

    public void addDocument(Document doc) {
    	//Add the doc object to the ArrayList
    	//@ param doc. Document object to add
    	if(doc==null){
    		throw new IllegalArgumentException();
    	}
       docs.add(doc);
    }

    public List<Document> getDocumentList() {
    	//Return the current document ArrayList
        return docs;
    }

    public String update(Operation operation) {
    	//Apply the operation to the database and then return the string representation of the operation
    	//@param operation. The operation to apply to the current database
    	Document d = getDocumentByDocumentName(operation.getDocName());
        d.update(operation);
        return d.toString(operation);
    }

    private Document getDocumentByDocumentName(String docName) {
    	//Return the Document object matching the name
    	//@param docName: The name of the Document object 
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
