import java.util.ArrayList;
import java.util.List;

public class TicTacToe {
static char[][] board; 
static boolean printStates = false;
static int curRow = 0;
static int curCol = 0;
static int maxRows = 3;
static int maxCols = 4;

	public static void main(String args[]){
		board = new char[3][4];
		for (String s : args){
			if(s.equals("X")){
				AddPieceToBoard('X');
			}
			else if(s.equals("O")){
				AddPieceToBoard('O');
			}
			else if(s.equals("#")){
				AddPieceToBoard('#');
			}
			else if(s.equals("Y")){
				printStates = true;
				break;
			}
			else if(s.equals("N")){
				break;
			}
			}
		}

	static boolean AddPieceToBoard(char piece){
		if(curCol == maxCols){
			curCol = 0;
			curRow++;
			if(curRow>maxRows){
				return false;
			}
		}
			board[curRow][curCol] = piece;
			curCol++;
			return true;
		}
	
	static void GenerateTree(){
		
	}
	
	}

class TreeNode{
	public int score = 0;
	public List<TreeNode> children;
	
	TreeNode(){
		children =  new ArrayList<TreeNode>();		
	}
	
	TreeNode(int score){
		this.score = score;
		children =  new ArrayList<TreeNode>();		
	}
	
	public boolean isLeaf(){
		return children.isEmpty();
	}
}

	

