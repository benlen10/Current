#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Structure representing Square
// size: dimension(number of rows/columns) of the square
// array: 2D array of integers
typedef struct _Square {
	int size;
	int ** array;
} Square;

Square * construct_square(char *filename);
int verify_magic(Square * square);

int main(int argc, char *argv[])
{
	//Check for invalid input
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

	//Create a new Square object and dynamically allocate space for the array in the heap
	Square  * square = malloc(sizeof(Square));
	square->size = size;
	square->array = malloc((size*2) * sizeof(int)); 
	for(int a = 0; a<size; a++){
		*(square->array + a) =  malloc((size*2) * sizeof(int));
	}

	char * line = malloc(sizeof(char) * 1000);  //Handle square sizes up to 99
	int row, col,x, curValue = 0;

	// Read the rest of the file to fill up the square
	fgets(line, lineSize, file);
	for (row = 0; row < size; row++) {
		fgets(line, 1000, file);
		for (col = 0; col < size; col++) {
			if (*line == ',') {
				line++;
			}
			//Handle single digit numbers
			if(*(line + 1) == ',' || ((*(line + 1) - '0')<0) || ((*(line + 1) - '0')>9)){
			*(*(square->array + row) + col) =  (*line - '0');
			}
			else{ //Handle numbers >10
			char digits[20];
			x=0;
			while((((*(line) - '0')>=0) && ((*(line) - '0')<=9))){
				digits[x++] = *line;	
				line++;
			}
			digits[x] = '\0';
			*(*(square->array + row) + col) = atoi(digits);
			}
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
	int row, col, c, curSum, magicSum = 0;
	for (row = 0; row < square->size; row++) {
		curSum = 0;
		for (col = 0; col < square->size; col++) {
			c = *(*(square->array + row) + col);		
				curSum += c;
			}
			if (magicSum == 0) {
				magicSum = curSum;
		}
		else if(curSum!=magicSum){
			return 0;
		}
	}
	
	//check all all rows within each col are equal
	for (col = 0; col < square->size; col++) {
		curSum = 0;
		for (row = 0; row < square->size; row++) {
			c = *(*(square->array + row) + col);	
				curSum += c;
			}
		if(curSum!=magicSum){
			return 0;
		}
	}

	// Check main diagonal
	curSum = 0;
	for (row = 0, col = 0; row < square->size; row++, col++) {
		c = *(*(square->array + row) + col);
		curSum += c;
	}
	if(curSum!=magicSum){
			return 0;
		}

	// Check secondary diagonal
	curSum = 0;
	for (row = (square->size-1), col = 0; col < square->size; row--, col++) {
		c = *(*(square->array + row) + col);
		curSum += c;
	}
	if(curSum!=magicSum){
			return 0;
		}
		
	//Free square memory
	int size = square->size;
	for(int a = 0; a<size -1; a++){
		free(*(square->array + a));
	}
	free(square->array);
	free(square);

	//Return true value if all checks have passed
	return 1;
}
