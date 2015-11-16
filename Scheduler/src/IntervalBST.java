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
			insertRec(root,n);
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
		IntervalBSTnode<K> n = new IntervalBSTnode<K>(key);
		return lookupRec(n, key);
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
	public void insertRec(IntervalBSTnode<K> recRoot, IntervalBSTnode<K> node){

	    if ( recRoot.getData().compareTo(node.getData())>0){
	    	
	      if ( recRoot.getLeft() == null ){
	        recRoot.setLeft(node);
	        return;
	      }
	      else{
	        insertRec(recRoot.getLeft(), node);
	      }
	    }
	    else{
	      if (recRoot.getRight() == null){
	        recRoot.setRight(node);
	        return;
	      }
	      else{
	        insertRec(recRoot.getRight(), node);
	      }
	    }
	  }
	
	public K lookupRec(IntervalBSTnode<K> n, K key){
		if(n.getData().equals(key)){
			return  (K) n.getData();
		}
		else if(n.getData().compareTo(key)>0){
			if(n.getRight()==null){
				return null;
			}
				lookupRec(n.getRight(),key);
			}
			else if(n.getData().compareTo(key)<0){
				if(n.getLeft()==null){
					return null;
				}
			lookupRec(n.getLeft(),key);
			}
	
			return n.getData();             //Fix This line: (This avoids the error "must return type k")
		
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
		        return n;
		    }
		 if(key.compareTo((Interval) n)>0){
			 n.setRight(deleteRec(n.getRight(),key));
		 }
		 else if(key.compareTo((Interval) n)<0){
			 n.setLeft(deleteRec(n.getLeft(), key)) ;
		 }

		 else  {                                             //if (key.equals(n.getKey()))

		    	if(n.getLeft()==null){
		    		return n.getRight();
		    	}
		    	if(n.getRight()==null){
		    		return n.getLeft();
		    	}
		    	
		    		IntervalBSTnode<K> r = root.getLeft();
		    		while(r.getRight()!=null){
		    			r = r.getRight();
		    		}
		    		return r;

		    }
		 		return n;
		    }
		    
		    public K minValue(IntervalBSTnode<K> n) {
	            if (n.getLeft()== null)
	                  return  n.getKey();
	            else
	                  return minValue(n.getLeft());
	      }

}