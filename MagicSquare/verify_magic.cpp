#include <stdio.h>
#include <stdlib.h>
#include <string.h>


// Structure representing Square
// size: dimension(number of rows/columns) of the square
// array: 2D array of integers
typedef struct Square {
	int size;
	int **array;
} Square;

Square * construct_square(char *filename);
int verify_magic(struct Square * square);

int main1(int argc, char *argv[])
{
	//Check for bad input
	if(argc<1){
		return -1;
	}

	// Construct square from the given file
	Square * square = construct_square(argv[0]);

	// Verify if it's a magic square and print true or false
	if(verify_magic(square) == 1){
	puts("true\n");
	}
	else{
	puts("false\n");
	}

	return 0;
}

// construct_square reads the input file to initialize a square struct
// from the contents of the file and returns the square.
// The format of the file is defined in the assignment specifications
Square * construct_square(char *filename)
{

	// Open and read the file
	FILE *file = fopen ( filename, "r" );
	char * temp = NULL;
	
	//Get the square size from the first line of the file
	fgets (temp, 1, file);
	int squareSize =  atoi(temp);
	
	//Genreate line size based off square size
	int lineSize = (squareSize * 2) -1;   //Subtract 1 for missing comma

 	// Initialize a new Square struct of that size
	struct Square * square = (Square*) malloc(sizeof(Square));
	square->size = squareSize;    
 	
	 char * line = NULL;
	 int ** arr = square->array;
	int row, col, curValue = 0;

	// Read the rest of the file to fill up the square
	for (row =0; row<square->size; row++) {
		fgets(line, lineSize, file);
	for (col = 0; col<square->size; col++) {
		if(*line == ','){
			line++;
		}
		*(*(arr+row)+col) =  (int) *line;
		line++;
	}
	}
	
	 //Return the constructed square
 	return square;
}

// verify_magic verifies if the square is a magic square
// returns 1(true) or 0(false)
int verify_magic(struct Square * square)
{	
	 int ** arr = square->array;
	// Check all all cols within each row are equal
	int row, col, c, curValue = 0;
	for (row =0; row<square->size; row++) {
	for (col = 0; col<square->size; col++) {
		c = *(*(arr+row)+col);
		if(curValue == 0){
		curValue = c;
		}else if
		(c != curValue)
			return 0;
	}
	}

	//check all all rows within each col are equal

	// Check main diagonal
	for (row = 0, col = 0; row< square->size; row++, col++) {
		c = *(*(arr+row)+col);
		if(curValue == 0){
		curValue = c;
		}else if
		(c != curValue)
			return 0;
	}


	// Check secondary diagonal

	//Return true value if all checks have passed
	return 1;
}
