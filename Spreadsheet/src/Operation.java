public class Operation {
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
    	this.docName = docName;
        this.userId = userId;
        this.op=op;
        this.rowIndex=rowIndex;
        this.colIndex=colIndex;
        this.timestamp = timestamp;
        
    }

    public Operation(String docName, String userId, OP op, long timestamp) {
    	this.docName = docName;
        this.userId = userId;
        this.op=op;
        this.timestamp = timestamp;
    }

    public String getDocName() {
       return docName;
    }

    public String getUserId() {
        return userId;
    }

    public OP getOp() {
      return op;
    }

    public int getRowIndex() {
       return rowIndex;
    }

    public int getColIndex() {
       return colIndex;
    }

    public int getConstant() {
       return constant;
    }

    public String toString() {
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
