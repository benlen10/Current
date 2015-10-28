public class WAL {
	private int rowIndex;
	private int colIndex;
	private int oldValue;
	

    public WAL(int rowIndex, int colIndex, int oldValue) {
    	this.rowIndex = rowIndex;
    	this.colIndex = colIndex;
    	this.oldValue = oldValue;
    }

    public int getOldValue() {
       return oldValue;
    }

    public int getRowIndex() {
        return rowIndex;
    }

    public int getColIndex() {
      return colIndex;
    }

}
