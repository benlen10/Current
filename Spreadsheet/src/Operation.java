//Main Class File:   Spreadsheet Server (Project 3)
//File:                  Operation.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

public class Operation {
	/*
	 * The Operation class creates Operation objects and stores data including the document name the operation is being preformed on,
	 * the userId preforming the operation the row/col index, constant and timestamp.
	 */
	
    // Enumeration of operator type.
    public enum OP {
        SET, //set,row,col,const -> set [row,col] to const
        CLEAR, //clear,row,col -> set [row,col] to 0
        ADD, //add,row,col,const -> add [row,col] by const
        SUB, //sub,row,col,const -> sub [row,col] by const
        MUL, //mul,row,col,const -> mul [row,col] by const
        DIV, //div,row,col,const -> div [row,col] by const
        UNDO, //undo the last operation
        REDO //redo the last undo
    }

    private String docName;
    private String userId;
    private OP op;
    private String o;
    private int rowIndex;
    private int colIndex;
    private int constant;
    private long timestamp;

    public Operation(String docName, String userId, OP op, int rowIndex, int
            colIndex, int constant, long timestamp) {
    	//Creates a new operation object and stores the data to local variables 
    	Operation.OP op2 = Operation.OP.SET;
    	if(!((op ==op2.SET)||(op ==op2.CLEAR)||(op ==op2.ADD)||(op ==op2.SUB)||(op ==op2.MUL)||(op ==op2.DIV)||(op ==op2.UNDO)||(op ==op2.REDO)) ){
    		throw new IllegalArgumentException();
    	}
    	if((docName.length()<1)||(docName==null)||(userId.length()<1)||(userId==null)||(rowIndex<0)||(colIndex<0)){
    		throw new IllegalArgumentException();
    	}
    	
    	this.docName = docName;
       this.userId = userId;
       this.op=op;
       this.rowIndex=rowIndex;
       this.colIndex=colIndex;
       this.constant = constant;
       this.timestamp = timestamp;
       this.o=o;
    }

    public Operation(String docName, String userId, OP op, int rowIndex, int
            colIndex, long timestamp) {
    	//Creates a new operation object and stores the data to local variables 
    	Operation.OP op2 = Operation.OP.SET;
    	if(!((op ==op2.SET)||(op ==op2.CLEAR)||(op ==op2.ADD)||(op ==op2.SUB)||(op ==op2.MUL)||(op ==op2.DIV)||(op ==op2.UNDO)||(op ==op2.REDO)) ){
    		throw new IllegalArgumentException();
    	}
    	if((docName.length()<1)||(docName==null)||(userId.length()<1)||(userId==null)||(rowIndex<0)||(colIndex<0)){
    		throw new IllegalArgumentException();
    	}
    	this.docName = docName;
        this.userId = userId;
        this.op=op;
        this.rowIndex=rowIndex;
        this.colIndex=colIndex;
        this.timestamp = timestamp;
        
    }

    public Operation(String docName, String userId, OP op, long timestamp) {
    	//Creates a new operation object and stores the data to local variables 
    	Operation.OP op2 = Operation.OP.SET;
    	if(!((op ==op2.SET)||(op ==op2.CLEAR)||(op ==op2.ADD)||(op ==op2.SUB)||(op ==op2.MUL)||(op ==op2.DIV)||(op ==op2.UNDO)||(op ==op2.REDO)) ){
    		throw new IllegalArgumentException();
    	}
    	if((docName.length()<1)||(docName==null)||(userId.length()<1)||(userId==null)){
    		throw new IllegalArgumentException();
    	}
    	
    	this.docName = docName;
        this.userId = userId;
        this.op=op;
        this.timestamp = timestamp;
    }

    public String getDocName() {
    	//Return the current Document object name
       return docName;
    }

    public String getUserId() {
    	//Return the current Document object userID
        return userId;
    }

    public OP getOp() {
    	//Return the current Document object operation
      return op;
    }

    public int getRowIndex() {
    	//Return the current Document object row index
    	Operation.OP op2 = Operation.OP.SET;
    	if((op ==op2.UNDO)||(op ==op2.REDO)){
    		return -1;
    	}
       return rowIndex;
    }

    public int getColIndex() {
    	//Return the current Document object column index
    	Operation.OP op2 = Operation.OP.SET;
    	if((op ==op2.UNDO)||(op ==op2.REDO)){
    		return -1;
    	}
       return colIndex;
    }

    public int getConstant() {
    	//Return the current Document object constant
    	Operation.OP op2 = Operation.OP.SET;
    	if((op ==op2.UNDO)||(op ==op2.REDO)||(op ==op2.CLEAR)){
    		return -1;
    	}
       return constant;
    }

    public String toString() {
    	//Convert the current operation object to a string format
    	if(op.toString().contains("UNDO")||op.toString().contains("REDO")){
    		return String.format("%d	%s	%s	%s\n\n",timestamp, docName, userId, op.toString().toLowerCase());
    	}
    	else if(op.toString().contains("CLEAR")){
        return String.format("%d	%s	%s	%s	[%d,%d]\n\n",timestamp, docName, userId, op.toString().toLowerCase(), rowIndex, colIndex);
    	}
    	else{
    		 return String.format("%d	%s	%s	%s	[%d,%d]	%d\n\n",timestamp, docName, userId, op.toString().toLowerCase(), rowIndex, colIndex,constant);
    	}
    }
}
