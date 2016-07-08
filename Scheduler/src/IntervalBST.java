import java.util.Iterator;

//Main Class File:   Scheduler
//File:                  IntervalBST.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

/**
	Provides the essential BST functions for a BST of Interval nodes
*/

public class IntervalBST<K extends Interval> implements SortedListADT<K> {
    private IntervalBSTnode<K> root;
    private boolean deleteFail = false;
    public IntervalBST() {
	}

    	
    /**
     * Inserts the given key into the Sorted List.
     * 
     * @param key the key value to insert into the Sorted List
     */
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

	
	/**
     * Deletes the given key from the Sorted List.  If the key is in the Sorted
     * List, the key is deleted and true is returned.  If the key is not in the
     * Sorted List, the Sorted List is unchanged and false is returned.
     * @param key the key value to delete from the Sorted List
     * @return true if the deletion is successful (i.e., the key was in the 
     * Sorted List and has been removed), false otherwise (i.e., the key was not
     * in the Sorted List)
     */
	public boolean delete(K key) {
		root =  deleteRec(root, key);
		if(deleteFail){
			return false;
		}
		else{
			return true;
		}
	}
	
	
	 /**
     * Searches for the given key in the Sorted List and returns it.  If the 
     * key is not in the Sorted List, null is returned.
     * @param key the key to search for
     * @return the key from the Sorted List, if it is there; null if the key is
     * not in the Sorted List
     */
	public K lookup(K key) {
		return lookupRec(root, key);
	}

	
	/**
     * Returns the number of items in the Sorted List.
     * @return the number of items in the Sorted List
     */
	public int size() {
	return sizeRec(root);
	}

	 /**
     * Returns true if and only if the Sorted List is empty.
     * @return true if the Sorted List is empty, false otherwise
     */
	public boolean isEmpty() {
	if(root==null){
		return true;
	}
	else{
		return false;
	}
	}

	/**
     * Returns an iterator over the Sorted List that iterates over the items in 
     * the Sorted List from smallest to largest.
     * @return an iterator over the Sorted List
     */
	public Iterator<K> iterator() {
		return new IntervalBSTIterator<K>(root);
	}
	
	
	//Recursive Methods
	
	/**
     * Recursive helper function for insert()
     */
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
	
	/**
     * Recursive helper function for lookup()
     */
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
	
	
	/**
     * Recursive helper function for delete()
     */
	public IntervalBSTnode<K> deleteRec(IntervalBSTnode<K> n, K key){
		 if (n == null) {
		        return null;
		    }
		    
		    if (key.equals(n.getKey())) {
		        // n is the node to be removed
		        if (n.getLeft() == null && n.getRight() == null) {
		        	deleteFail = true;
		            return null;
		        }
		        if (n.getLeft() == null) {
		            return n.getRight();
		        }
		        if (n.getRight() == null) {
		            return n.getLeft();
		        }
		       
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
		    
	/**
     * Recursive helper function for deleteRec()
     */
	private K smallest(IntervalBSTnode<K> n) {
	    if (n.getLeft() == null) {
	        return n.getKey();
	    } else {
	        return smallest(n.getLeft());
	    }
	}

}