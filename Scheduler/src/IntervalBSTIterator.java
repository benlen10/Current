import java.util.*;

public class IntervalBSTIterator<K extends Interval> implements Iterator<K> {

	private Stack<IntervalBSTnode<K>> stack; //for keeping track of nodes
	
	public IntervalBSTIterator(IntervalBSTnode<K> root) {
		stack = new Stack<IntervalBSTnode<K>>();

		//TODO Remove this exception and implement the method
		throw new RuntimeException("constructor not implemented.");
	} 
    public boolean hasNext() {
		//TODO Remove this exception and implement the method
		throw new RuntimeException("hasNext not implemented.");
    }

    public K next() {
		//TODO Remove this exception and implement the method
		throw new RuntimeException("next not implemented.");
    }

    public void remove() {
        // DO NOT CHANGE: you do not need to implement this method
        throw new UnsupportedOperationException();
    }
}