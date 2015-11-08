//Main Class File:   Spreadsheet Server (Project 3)
//File:                  Document.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

import java.util.Collection;
import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;
import java.lang.StringBuilder;

public class Document {
	/*
	 * The Document class is responsible for creating document objects and stores
	 * information including row size, column size, the user list and current cell data.
	 */
    private String docName;
    private int rowSize;
    private int colSize;
    List<User> userList;
    int[][] doc;
    Operation.OP op2;
    

    public Document(String docName, int rowSize, int colSize, List<User>
            userList) {
    	//Creates a new Document object and stores values
    	if((docName.length()<1)||(docName==null)||(rowSize<1)||(colSize<1)){
    		throw new IllegalArgumentException();
    	}
       this.docName=docName;
       this.rowSize = rowSize;
       this.colSize = colSize;
       this.userList = userList;
       doc = new int[rowSize][colSize];
    	
    }
    
    public List<String> getAllUserIds() {
    	//Returns an array list of the UserIDs
       List<String> userIds = new ArrayList<String>();
       Iterator<User> it = userList.iterator();
       while(it.hasNext()){
    	   userIds.add(it.next().getUserId());
       }
       return userIds;
    }

    public void update(Operation operation) {
    	//Applies the specified operation to the current document
    	//@param specific operation to apply
    	
      if(operation.getOp() == op2.SET){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  doc[operation.getRowIndex()][operation.getColIndex()] = operation.getConstant();
    	  
      } else if(operation.getOp() == op2.CLEAR){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  doc[operation.getRowIndex()][operation.getColIndex()] = 0;
    	  
      } else if(operation.getOp() == op2.ADD){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  doc[operation.getRowIndex()][operation.getColIndex()] = doc[operation.getRowIndex()][operation.getColIndex()] +  operation.getConstant();
    	  
      } else if(operation.getOp() == op2.SUB){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  doc[operation.getRowIndex()][operation.getColIndex()] = doc[operation.getRowIndex()][operation.getColIndex()] - operation.getConstant();
    	  
      }  else if(operation.getOp() == op2.MUL){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  doc[operation.getRowIndex()][operation.getColIndex()] = doc[operation.getColIndex()][operation.getRowIndex()] * operation.getConstant();
    	  
      } else if(operation.getOp() == op2.DIV){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  doc[operation.getRowIndex()][operation.getColIndex()] =doc[operation.getColIndex()][operation.getRowIndex()] / operation.getConstant();
    	  
      } else if(operation.getOp() == op2.UNDO){
    	  WAL w = getUserByUserId(operation.getUserId()).popWALForUndo();
    	  getUserByUserId(operation.getUserId()).pushWALForRedo(new WAL(w.getRowIndex(),w.getColIndex(), doc[w.getRowIndex()][w.getColIndex()]));
    	  doc[w.getRowIndex()][w.getColIndex()] = w.getOldValue();
	  
      } else if(operation.getOp() == op2.REDO){
    	  WAL w = getUserByUserId(operation.getUserId()).popWALForRedo();
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(w.getRowIndex(),w.getColIndex(), doc[w.getRowIndex()][w.getColIndex()]));
    	  doc[w.getRowIndex()][w.getColIndex()] = w.getOldValue();
      }
      else{
    	  throw new IllegalArgumentException();
      }
    }

    public String getDocName() {
    	//Return the name of the Document object
        return docName;
    }

    private User getUserByUserId(String userId) {  
    	//Return the User object matching the ID
    	//@param userId: The id to match with a User object
    	Iterator<User> it =  userList.iterator();
    	while(it.hasNext()){
    		User u = it.next();
    		if(u.getUserId().contains(userId)){
    			
    			return u;
    		}
    	}
    return null;
    }

    public int getCellValue(int rowIndex, int colIndex){
    	if((rowIndex>=rowSize)||(colIndex>=colSize)){
    		throw new IllegalArgumentException();
    	}
    	//Return the value at the specified position
       return doc[rowIndex][colIndex];
    }
    

    public String toString(Operation op) {
    	//Return the string representation of the operation
    	//@param op. The operation to convert to a string
       StringBuilder sb = new StringBuilder();
       sb.append("----------Update Database----------\n");
       sb.append(op.toString());
       sb.append(String.format("Document Name: %s	Size: [%d,%d]\nTable: \n", getDocName(), rowSize, colSize));
       int i, x;
       for(i=0; i<rowSize; i++){
    	   for(x=0; x<colSize; x++){
    		   sb.append(String.format("%d	", doc[i][x]));
    	   }
    	   sb.append("\n");
       }
       return sb.toString();
    }
}
