import java.util.Collection;
import java.util.List;
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

    public void update(Operation operation) {
      if(operation.getOp() == op2.SET){
    	  doc[operation.getColIndex()][operation.getRowIndex()] = operation.getConstant();
    	  
      } else if(operation.getOp() == op2.CLEAR){
    	  doc[operation.getColIndex()][operation.getRowIndex()] = 0;
    	  
      } else if(operation.getOp() == op2.ADD){
    	  doc[operation.getColIndex()][operation.getRowIndex()] =+ operation.getConstant();
    	  
      } else if(operation.getOp() == op2.SUB){
    	  doc[operation.getColIndex()][operation.getRowIndex()] =- operation.getConstant();
    	  
      }  else if(operation.getOp() == op2.MUL){
    	  doc[operation.getColIndex()][operation.getRowIndex()] = doc[operation.getColIndex()][operation.getRowIndex()] * operation.getConstant();
    	  
      } else if(operation.getOp() == op2.DIV){
    	  doc[operation.getColIndex()][operation.getRowIndex()] =doc[operation.getColIndex()][operation.getRowIndex()] / operation.getConstant();
    	  
      } else if(operation.getOp() == op2.UNDO){
    	  WAL w = getUserByUserId(operation.getUserId()).popWALForUndo();
    	  doc[w.getRowIndex()][w.getColIndex()] = w.getOldValue();
    	  
      } else if(operation.getOp() == op2.REDO){
    	  WAL w = getUserByUserId(operation.getUserId()).popWALForRedo();
    	  doc[w.getRowIndex()][w.getColIndex()] = w.getOldValue();
    	  
      }
    }

    public String getDocName() {
        return docName;
    }

    private User getUserByUserId(String userId) {
    	Iterator it =  userList.iterator();
    	while(it.hasNext()){
    		User u = (User) it.next();
    		if(u.getUserId()==userId){
    			return u;
    		}
    	}
    return null;
    }

    public int getCellValue(int rowIndex, int colIndex){
       return doc[rowIndex][colIndex];
    }

    public String toString() {
       StringBuilder sb = new StringBuilder();
       sb.append(String.format("Document Name: %s     Size: [%d, %d]\n Table:\n", getDocName(), rowSize, colSize));
       int i, x;
       for(i=0; i<rowSize; i++){
    	   for(x=0; x<colSize; x++){
    		   sb.append(String.format("%d      ", doc[i][x]));
    	   }
    	   sb.append("\n");
       }
       return sb.toString();
    }
}
