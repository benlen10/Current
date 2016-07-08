import java.util.Iterator;

public class IntervalBST<K extends Interval> implements SortedListADT<K> {
    private IntervalBSTnode<K> root;
    public IntervalBST() {
	}

	public void insert(K key){
		if (root == null){
			root = new IntervalBSTnode<K>(key);
			return;
		}
		else{
			IntervalBSTnode<K> n = new IntervalBSTnode<K>(key);
			insertRec(root,key);
		}
	}

	public boolean delete(K key) {
		root =  deleteRec(root, key);
		if(root==null){
			return false;
		}
		else{
			return true;
		}
	}
	
	public K lookup(K key) {
		return lookupRec(root, key);
	}

	public int size() {
	return sizeRec(root);
	}

	public boolean isEmpty() {
	if(root==null){
		return true;
	}
	else{
		return false;
	}
	}

	public Iterator<K> iterator() {
		return new IntervalBSTIterator<K>(root);
	}
	
	
	//Recursive Methods
	public IntervalBSTnode<K> insertRec(IntervalBSTnode<K> n, K key) {  //throws DuplicateException
		 if (n == null) {
		        return new IntervalBSTnode<K>(key);
		    }
		     
		    if (n.getKey().equals(key)) {
		        //throw new DuplicateException();
		    	return new IntervalBSTnode<K>(key);
		    }
		    
		    if (key.compareTo(n.getKey()) < 0) {
		        n.setLeft( insertRec(n.getLeft(), key) );
		        return n;
		    }
		    
		    else {
		        n.setRight( insertRec(n.getRight(), key) );
		        return n;
		    }
	  }
	
	public K lookupRec(IntervalBSTnode<K> n, K key){
		if (n == null) {
	        return null;
	    }
	    
	    if (n.getKey().equals(key)) {
	        return key;
	    }
	    
	    if (key.compareTo(n.getKey()) < 0) {
	        return lookupRec(n.getLeft(), key);
	    }
	    
	    else {
	        return lookupRec(n.getRight(), key);
	    }
	}
	
	public int sizeRec(IntervalBSTnode<K> n){
		IntervalBSTnode<K> right = n.getRight();
		IntervalBSTnode<K> left = n.getLeft();
		  int c = 1;                                
		  if ( right != null ) c += sizeRec(right);        
		  if ( left != null ) c += sizeRec(left);         
		  return c;
	}
	
	public IntervalBSTnode<K> deleteRec(IntervalBSTnode<K> n, K key){
		 if (n == null) {
		        return null;
		    }
		    
		    if (key.equals(n.getKey())) {
		        // n is the node to be removed
		        if (n.getLeft() == null && n.getRight() == null) {
		            return null;
		        }
		        if (n.getLeft() == null) {
		            return n.getRight();
		        }
		        if (n.getRight() == null) {
		            return n.getLeft();
		        }
		       
		        // if we get here, then n has 2 children
		        K smallVal = smallest(n.getRight());
		        n.setKey(smallVal);
		        n.setRight( deleteRec(n.getRight(), smallVal) );
		        return n; 
		    }
		    
		    else if (key.compareTo(n.getKey()) < 0) {
		        n.setLeft( deleteRec(n.getLeft(), key) );
		        return n;
		    }
		    
		    else {
		        n.setRight( deleteRec(n.getRight(), key) );
		        return n;
		    }
		    }
		    
	private K smallest(IntervalBSTnode<K> n) {
	    if (n.getLeft() == null) {
	        return n.getKey();
	    } else {
	        return smallest(n.getLeft());
	    }
	}

}