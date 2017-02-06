import java.util.ArrayList;
import java.util.List;

public class TicTacToe {
static TreeNode rootNode;
static boolean xTurn = true;
static boolean printStates = false;
static int curRow = 0;
static int curCol = 0;
static int maxRows = 3;
static int maxCols = 4;
static int alpha = 2;
static int beta = -2;

	public static void main(String args[]){
		rootNode = new TreeNode(0, new char[3][4], null);
		for (String s : args){
			if(s.equals("Y")){
				printStates = true;
				break;
			}
			else if(s.equals("N")){
				break;
			}
			else if(!s.equals(" ")){
				AddPieceToInitialBoard(s.charAt(0));
			}
			}
		
		GenerateTree(rootNode);
			Search(true, rootNode, 2, -2);
		}

	static boolean AddPieceToInitialBoard(char piece){
		if(curCol == maxCols){
			curCol = 0;
			curRow++;
			if(curRow>maxRows){
				return false;
			}
		}
			rootNode.state[curRow][curCol] = piece;
			curCol++;
			return true;
		}
	
	static void GenerateTree(TreeNode node){
		while(true){
		if(curCol == maxCols){
			curCol = 0;
			curRow++;
			if(curRow>maxRows){
				break;
			}
		}
		if(node.state[curRow][curCol] == '_'){
			TreeNode n = new TreeNode(0, node.state.clone(), node);
			if(xTurn){
				n.state[curRow][curCol] = 'X';
				if(WinningState(n.state, true)){
					n.score = 1;
				}
			}
			else{
				n.state[curRow][curCol] = 'O';
				if(WinningState(n.state, false)){
					n.score = -1;
				}
			}
			node.children.add(n);
			GenerateTree(n);
		}
		curCol++;
		}
		return;  //Return if no more blank states
	}
	
	static boolean WinningState(char[][] state, boolean player){
		int curCount = 0;
		char piece = 'X';
		boolean found = false;
		if(!player){
			piece = 'O';
		}
		
		//Check rows
		curRow = 0;
		curCol = 0;
		
		while(curRow< maxRows){
			if(curCol == maxCols ){
				curCol = 0;
				curRow++;
				if(curCount==3){
					return true;
				}
				curCount = 0;
			}
			if(state[curRow][curCol] == piece){
				found = true;
				curCount++;
			}
			else if(found){
				curCount =0;
				found = false;
			}
			curCol++;
		}
		
		//Check cols
		curRow = 0;
		curCol = 0;
		curCount = 0;
		found = false;
		
		while(curCol< maxCols){
			if(curRow == maxRows ){
				curRow = 0;
				curCol++;
				if(curCount==3){
					return true;
				}
				curCount = 0;
			}
			if(state[curRow][curCol] == piece){
				found = true;
				curCount++;
			}
			else if(found){
				curCount =0;
				found = false;
			}
			
			curRow++;
		}
		
		//Check primary diags
		curRow = 0;
		curCol = 0;
		curCount = 0;
		found = false;

		while((curCol+2)< maxCols){
			if(curRow == maxRows ){
				curCol++; 
				curRow = 0; 
				if(curCount==3){
					return true;
				}
				curCount = 0;
			}
			if(state[curRow][curCol] == piece){
				found = true;
				curCount++;
			}
			else if(found){
				curCount =0;
				found = false;
			}
			
			curCol++;
			curRow++;
		}
		
		//Check secondary diags
		curRow = (maxRows-1);
		curCol = 0;
		curCount = 0;
		found = false;
		
		while((curCol+2)< maxCols){
			if(curRow < 0 ){
				curCol++; 
				curRow = (maxRows-1); 
				if(curCount==3){
					return true;
				}
				curCount = 0;
			}
			if(state[curRow][curCol] == piece){
				found = true;
				curCount++;
			}
			else if(found){
				curCount =0;
				found = false;
			}
			
			curCol++;
			curRow--;
		}
		
		return false;
	}
	
	
	static int Search(boolean player,TreeNode n ,int alpha, int beta){
		int score = 0;

    if(n.isLeaf()){    //If game over return winner
        return n.score;
    }

    if(player){ //if max's turn
        for (TreeNode child : n.children){
            score = Search(!player ,child,alpha,beta);
            if (score > alpha){
            	alpha = score;
            }
            if(alpha >= beta){
            	return alpha; //Prune tree
            }
            }
        return alpha; //This is our best move
    }
    else{ //If min's turn
    	 for (TreeNode child : n.children){
    		 score = Search(!player ,child,alpha,beta);
            if(score < beta){
            	beta = score; // (opponent has found a better worse move)
            }
            if (alpha >= beta){
            	return beta; //Prune tree
            }
	}
    	 return beta; //This is our opponent's best move
    }
	}
}


class TreeNode{
	public int score = 0;
	public List<TreeNode> children;
	public char[][] state;
	public TreeNode parent;
	
	TreeNode(){
		children =  new ArrayList<TreeNode>();		
	}
	
	TreeNode(int score, char[][] state, TreeNode parent){
		this.score = score;
		this.state = state;
		children =  new ArrayList<TreeNode>();		
		this.parent = parent;
	}
	
	public boolean isLeaf(){
		return children.isEmpty();
	}
}

	

