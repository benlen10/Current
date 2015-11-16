import java.util.*;

public class IntervalBSTIterator<K extends Interval> implements Iterator<K> {

	private Stack<IntervalBSTnode<K>> stack; //for keeping track of nodes
	
	public IntervalBSTIterator(IntervalBSTnode<K> root) {
		stack = new Stack<IntervalBSTnode<K>>();
		buildStack(root);
	} 
    public boolean hasNext() {
		if(stack.isEmpty()){
			return false;
		}
		else{
			return true;
		}
    }

    public K next() {
		return stack.pop().getKey();
    }

    public void remove() {
        // DO NOT CHANGE: you do not need to implement this method
        throw new UnsupportedOperationException();
    }
    
    public void buildStack(IntervalBSTnode<K> p){
        if(p.getLeft()!=null)
            buildStack(p.getLeft());
 
        stack.push(p);
 
        if(p.getRight()!=null)
            buildStack(p.getRight());
    }
    
}