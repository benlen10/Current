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
	FILE *file = NULL;
	fopen_s(&file, "C:\\magic.txt", "r");
	char * temp = NULL;

	//Get the square size from the first line of the file
	int size = getc(file) - '0';
	printf("SIZE: %d\n", size);

	//Genreate line size based off square size
	int lineSize = (size * 2);

	// Initialize a new Square struct of that size
	struct Square * square = (Square*)malloc(sizeof(Square));
	square->array = (int**) malloc((size + 5) * sizeof(square->array) + ((size + 5) * (size * sizeof(*square->array))));
	square->size = size;

	char * line = (char*) malloc(sizeof(char)*(lineSize + 2));
	int row, col, curValue = 0;

	// Read the rest of the file to fill up the square
	fgets(line, lineSize, file); //Skip first line
	printf("LINESIZE: %d\n", lineSize);
	for (row = 0; row <= square->size; row++) {	
		printf("\nOUTER\n");
		//Generate int array
		int num[100];
		int i = 0;
		line = fgets(line, lineSize, file);
		while (*line && *line != '\n') {
			if (*line == ',') {
				line++;
			}
			printf("%c", *line);
			num[i++] = (int)*line;
			line++;
		}
		//end of generate int array
		for (col = 0; col < square->size; col++) {
			*(*(square->array + row) + col) = num[col];
		}
	}

	//Return the constructed square
	return square;
}

// verify_magic verifies if the square is a magic square
// returns 1(true) or 0(false)
int verify_magic(struct Square * square)
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
	for (row = square->size, col = 0; col < square->size; row--, col++) {
		c = *(*(square->array + row) + col);
		curSum += c;
	}
	if(curSum!=magicSum){
			return 0;
		}

	//Return true value if all checks have passed
	return 1;
}
