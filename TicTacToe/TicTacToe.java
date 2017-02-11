// Semester:         CS 540 Fall 2016
//
// Author:           Benjamin Lenington
// Email:            lenington@wisc.edu
// CS Login:         lenington


import java.util.ArrayList;
import java.util.List;

public class TicTacToe {
	static TreeNode rootNode;
	static boolean xTurn = true;
	static boolean printStates = true;
	static int curRow = 0;
	static int curCol = 0;
	static int maxRow = 2;
	static int maxCol = 3;
	static int alpha = -2;
	static int beta = 2;

	public static void main(String args[]){
		rootNode = new TreeNode(0, new char[3][4], null);
		for (String s : args){
			if(s.equals("Y")){
				break;
			}
			else if(s.equals("N")){
				printStates = false;
				break;
			}
			else if(!s.equals(" ")){
				if(!AddPieceToInitialBoard(s.charAt(0))){
					System.err.println("Fatal Error: Failed to create board");
				}
			}
		}
		TicTacToe instance = new TicTacToe();
		instance.GenerateTree(rootNode, true);
		instance.TreeSearch(true, rootNode, alpha, beta);
	}	

	static boolean AddPieceToInitialBoard(char piece){
		if(curCol > maxCol){
			curCol = 0;
			curRow++;
			if(curRow>maxRow){
				return false;
			}
		}
		rootNode.state[curRow][curCol] = piece;
		curCol++;
		return true;
	}

	void GenerateTree(TreeNode node, boolean player){
		int Col = 0;
		int Row = 0;
		while(Row<=maxRow){
			while(Col<=maxCol){
				if(node.state[Row][Col] == '_'){
					TreeNode n = new TreeNode(0, CopyMatrix(node.state) , node);
					if(player){
						n.state[Row][Col] = 'X';
					}
					else{
						n.state[Row][Col] = 'O';
					}
					if(WinningState(n.state, player)){
						n.score = 1;
					}
					node.children.add(n);
					GenerateTree(n, !player);
				}
				Col++;
			}
			Col = 0;
			Row++;
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
		while(curRow< maxRow){
			while(curCol < maxCol ){
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
			curCol = 0;
			curRow++;
			if(curCount==3){
				return true;
			}
			curCount = 0;
		}

		//Check cols
		curRow = 0;
		curCol = 0;
		curCount = 0;
		found = false;

		while(curCol< maxCol){
			while(curRow < maxRow ){
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
			curRow = 0;
			curCol++;
			if(curCount==3){
				return true;
			}
			curCount = 0;
		}

		//Check primary diags
		curRow = 0;
		curCol = 0;
		curCount = 0;
		int diagMarker = 0;
		found = false;

		while((diagMarker+1)< maxCol){
			if(curRow > maxRow ){
				diagMarker++; 
				curCol = diagMarker;
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
		curRow = (maxRow-1);
		curCol = 0;
		curCount = 0;
		diagMarker =0;
		found = false;

		while((diagMarker+1)< maxCol){
			if(curRow < 0 ){
				diagMarker++; 
				curCol = diagMarker;
				curRow = maxRow;
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

	int TreeSearch(boolean player,TreeNode n ,int alpha, int beta){
		int score = 0;

		if(n.isLeaf()){    //If game over return winner
			return n.score;
		}

		if(player){ //if max's turn
			for (TreeNode child : n.children){
				if (score > alpha){
					alpha = score;
				}
				if(alpha >= beta){
					return alpha; //Prune tree
				}
					PrintState(child, alpha, beta);
			}
			return alpha; //This is our best move
		}
		else{ //If min's turn
			for (TreeNode child : n.children){
				score = TreeSearch(!player ,child,alpha,beta);
				if(score < beta){
					beta = score; // (opponent has found a better worse move)
				}
				if (alpha >= beta){
					return beta; //Prune tree
				}
					PrintState(child, alpha, beta);
				}
			return beta; //Opponent's best move
		}
	}

	void PrintState(TreeNode n, int alpha, int beta){
		int row = 0;
		int col = 0;
		if(n.score == 1){
			System.out.printf("SOLUTION\n");
		}
		if(printStates || n.score==1){
		while(row<=maxRow){
			while(col<=maxCol){
				System.out.printf("%c ", n.state[row][col]);
				col++;
			}
			System.out.printf("\n");
			row++;
			col=0;
		}
		}
		if((n.score != 1)&& printStates){
			System.out.printf("Alpha: %d Beta: %d\n", alpha, beta);
		}
	}

	//Summary: Copy a matrix to new address spa
	static char[][] CopyMatrix(char[][] src){
		char [][] dest = new char[src.length][];
		for(int i = 0; i < src.length; i++)
		{
			char[] aMatrix = src[i];
			int aLength = aMatrix.length;
			dest[i] = new char[aLength];
			System.arraycopy(aMatrix, 0, dest[i], 0, aLength);
		}
		return dest;
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



