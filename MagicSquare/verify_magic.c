#include <stdio.h>
#include <stdlib.h>
#include <string.h>


// Structure representing Square
// size: dimension(number of rows/columns) of the square
// array: 2D array of integers
typedef struct _Square {
	int size;
	int array[100][100];  //int ** array
} Square;

Square * construct_square(char *filename);
int verify_magic(Square * square);

int main(int argc, char *argv[])
{
	//Check for bad input
	if (argc < 1) {
		return -1;
	}

	// Construct square from the given file
	Square * square = construct_square(argv[0]);

	// Verify if it's a magic square and print true or false
	if (verify_magic(square) == 1) {
		puts("true\n");
	}
	else {
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
	FILE *file = fopen("magic.txt", "r");

	//Get the square size from the first line of the file
	int size = getc(file) - '0';

	//Genreate line size based off square size
	int lineSize = (size * 2); 

	// Initialize a new Square struct of that size
	Square  * square = malloc(sizeof(Square));
	//square->array = int[100][100]; //+ (size * (size * sizeof(*square->array))));
	square->size = size;

	char * line = (char*)malloc(sizeof(char)*100);
	int row, col, curValue = 0;

	// Read the rest of the file to fill up the square
	fgets(line, lineSize, file);
	for (row = 0; row < size; row++) {
		fgets(line, 100, file);
		for (col = 0; col < size; col++) {
			if (*line == ',') {
				line++;
			}
			printf("\nROW: %d COL: %d VAL: %d\n", row, col,(*line - '0'));
			square->array[row][col] =  (*line - '0');//*((square->array + row) + col) = *line - '0';
			line++;
		}
	}

	//Return the constructed square
	return square;
}

// verify_magic verifies if the square is a magic square
// returns 1(true) or 0(false)
int verify_magic(Square * square)
{
	// Check all all cols within each row are equal
	printf("ROW CHECK\n");
	int row, col, c, curSum, magicSum = 0;
	for (row = 0; row < square->size; row++) {
		curSum = 0;
		for (col = 0; col < square->size; col++) {
			c = (int) square->array[row][col]; //*(*(&square->array + row) + col);	
			printf("Row: %d Col: %d VAL: %d\n", row, col,c);		
				curSum += c;
			}
			printf("FinalCurSum: %d\n", curSum);
			if (magicSum == 0) {
				magicSum = curSum;
				printf("MAGICSUM: %d\n", magicSum);
		}
		else if(curSum!=magicSum){
			return 0;
		}
	}
	//check all all rows within each col are equal
	printf("COL CHECK\n");
	for (col = 0; col < square->size; col++) {
		curSum = 0;
		for (row = 0; row < square->size; row++) {
			c = (int) square->array[row][col];//*((square->array + row) + col);	
			printf("Row: %d Col: %d VAL: %d\n", row, col,c);			
				curSum += c;
			}
		if(curSum!=magicSum){
			return 0;
		}
	}

	// Check main diagonal
	printf("MAIN DIAG CHECK\n");
	curSum = 0;
	for (row = 0, col = 0; row < square->size; row++, col++) {
		c = (int) square->array[row][col]; //*((square->array + row) + col);
		printf("Row: %d Col: %d VAL: %d\n", row, col,c);	
		curSum += c;
	}
	if(curSum!=magicSum){
			return 0;
		}

	// Check secondary diagonal
	printf("SECONDARY DIAG CHECK\n");
	curSum = 0;
	for (row = (square->size-1), col = 0; col < square->size; row--, col++) {
		c = (int) square->array[row][col]; //*((square->array + row) + col);
		printf("Row: %d Col: %d VAL: %d\n", row, col,c);	
		curSum += c;
	}
	if(curSum!=magicSum){
			return 0;
		}

	//Return true value if all checks have passed
	return 1;
}
