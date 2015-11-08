//Main Class File:   Spreadsheet Server (Project 3)
//File:                  User.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

public class User {
	/*
	 * The class responsible for creating User objects and stores data such including the userId,
	 * and the current undo and redo stacks of WAL objects.
	 */

	
    private String userId;
    SimpleStack<WAL> undo;
    SimpleStack<WAL> redo;

    
    //Create a new user Object and store the userId as well as creating new WAL stacks for undo and redo operations
    public User(String userId) {
    	if((userId==null)||(userId.length()<1)){
    		new IllegalArgumentException();
    	}
       this.userId = userId;
       redo = new SimpleStack<WAL>();
       undo = new SimpleStack<WAL>();
    }

  //Return the most recent WAL operation to undo
    public WAL popWALForUndo() {
    	 return undo.pop();
    }

  //Return the most recent WAL operation to undo
    public WAL popWALForRedo() {
      return redo.pop();
    }
    
    //Push the undo WAL object to the top of the stack
    public void pushWALForUndo(WAL trans) {
    	if(trans==null){
    		throw new IllegalArgumentException();
    	}
        undo.push(trans);
    }
    
    //Push the redo WAL object to the top of the stack 
    public void pushWALForRedo(WAL trans) {
    	if(trans==null){
    		throw new IllegalArgumentException();
    	}
        redo.push(trans);
    }
    
    //Clear all WAL objects in the redo queue
    public void clearAllRedoWAL() {
       redo.clear();
    }
    
  //Clear all WAL objects in the undo queue
    public void clearAllUndoWAL() {
       undo.clear();
    }
    
    //Return the userId of the current User Object
    public String getUserId() {
        return userId;
    }
}
