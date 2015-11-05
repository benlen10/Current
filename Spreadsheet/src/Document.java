import java.util.Collection;
import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;
import java.lang.StringBuilder;

public class Document {
    private String docName;
    private int rowSize;
    private int colSize;
    List<User> userList;
    int[][] doc;
    Operation.OP op2;
    

    public Document(String docName, int rowSize, int colSize, List<User>
            userList) {
       this.docName=docName;
       this.rowSize = rowSize;
       this.colSize = colSize;
       this.userList = userList;
       doc = new int[rowSize][colSize];
    	
    }
    
    public List<String> getAllUserIds() {
       List<String> userIds = new ArrayList<String>();
       Iterator<User> it = userList.iterator();
       while(it.hasNext()){
    	   userIds.add(it.next().getUserId());
       }
       return userIds;
    }

    public void update(Operation operation) {
    	//System.err.println("LOOP");
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
    	  System.err.printf("PRE%d\n",doc[w.getRowIndex()][w.getColIndex()]);
    	  doc[w.getRowIndex()][w.getColIndex()] = w.getOldValue();
    	  System.err.printf("UNDO [%d][%d]%d\n",w.getRowIndex(), w.getColIndex(), w.getOldValue());
    	  System.err.printf("RESULT%d\n",doc[w.getRowIndex()][w.getColIndex()]);
    	  
      } else if(operation.getOp() == op2.REDO){
    	  getUserByUserId(operation.getUserId()).pushWALForUndo(new WAL(operation.getRowIndex(),operation.getColIndex(), doc[operation.getRowIndex()][operation.getColIndex()]));
    	  WAL w = getUserByUserId(operation.getUserId()).popWALForRedo();
    	  System.err.printf("PRE%d\n",doc[w.getRowIndex()][w.getColIndex()]);
    	  doc[w.getRowIndex()][w.getColIndex()] = w.getOldValue();
    	  System.err.printf("REDO [%d][%d]%d\n",w.getRowIndex(), w.getColIndex(), w.getOldValue());
    	  System.err.printf("RESULT%d\n",doc[w.getRowIndex()][w.getColIndex()]);
    	  
      }
    }

    public String getDocName() {
        return docName;
    }

    private User getUserByUserId(String userId) {
    	//System.err.printf("UserID: %s.\n",userId);
    	Iterator<User> it =  userList.iterator();
    	while(it.hasNext()){
    		User u = it.next();
    		//System.err.printf("User: %s\n",u.getUserId());
    		if(u.getUserId().contains(userId)){
    			
    			return u;
    		}
    	}
    return null;
    }

    public int getCellValue(int rowIndex, int colIndex){
       return doc[rowIndex][colIndex];
    }
    

    public String toString(Operation op) {
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
