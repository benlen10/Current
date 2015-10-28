public class User {
    private String userId;
    SimpleStack<WAL> undo;
    SimpleStack<WAL> redo;

    public User(String userId) {
       this.userId = userId;
       redo = new SimpleStack<WAL>();
       undo = new SimpleStack<WAL>();
    }

    public WAL popWALForUndo() {
    	 return undo.pop();
    }

    public WAL popWALForRedo() {
      return redo.pop();
    }

    public void pushWALForUndo(WAL trans) {
        undo.push(trans);
    }

    public void pushWALForRedo(WAL trans) {
        redo.push(trans);
    }

    public void clearAllRedoWAL() {
       redo.clear();
    }

    public void clearAllUndoWAL() {
       undo.clear();
    }

    public String getUserId() {
        return userId;
    }
}
