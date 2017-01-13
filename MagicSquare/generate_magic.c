#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Structure representing Square
// size: dimension(number of rows/columns) of the square
// array: 2D array of integers
typedef struct _Square {
	int size;
	int **array; 
} Square;

int get_square_size();
Square * generate_magic(int size);
void write_to_file(Square * square, char *filename);
void printNum(int n, FILE * file);  //Custom helper function

int main(int argc, char *argv[])
{
	//Check for invalid input
	if (argc < 1) {
		return -1;
	}

	// Check input arguments to get filename	
	char * filename = argv[1];
	if (strlen(filename) < 4) {
		return -1;
	}

	// Get size from user
	int magicSize = get_square_size();

	//Verify that the number entered is valid
	if (magicSize < 0) {
		return -1;
	}

	// Generate the magic square
	Square * square = generate_magic(magicSize);

	// Write the square to the output file
	write_to_file(square, filename);
	return 0;
}

// get_square_size prompts the user for the magic square size
// checks if it is an odd number >= 3 and returns the number
int get_square_size()
{
	puts("Enter size of magic square, must be odd");
	char * imp = malloc(sizeof(char) * 10);
	gets(imp);
	int magicSize = atoi(imp);
	free(imp);

	if ((magicSize > 99)) {
		printf("ERROR: Size must be < 100\n");
		return -1;
	}
	//Verify that the number entered is greater than or equal to 3 and an odd number. Else return -1
	if ((magicSize >= 3) && (magicSize % 2 != 0)) {
		return magicSize;
	}
	else {
		printf("ERROR: Size must be >= 3 and an odd number\n");
		return -1;
	}
}

// generate_magic constructs a magic square of size n
// using the Siamese algorithm and returns the Square struct
Square * generate_magic(int size)
{
	//Create a new Square object and dynamically allocate space for the array in the heap
	Square  * square = malloc(sizeof(Square));
	square->size = size;
	square->array = malloc((size*2) * sizeof(int));  //(size*2) is essential to support square sizes up to 99
	for(int a = 0; a<size; a++){
		*(square->array + a) =  malloc((size*2) * sizeof(int));
	}

	//Set all values to zero intitially 
	int row, col, curValue = 0;
	for (row = 0; row < size; row++) {
		for (col = 0; col < size; col++) {
			*(*(square->array + row) + col) = 0;
		}
	}
	//Generate magic square values using the Siamese method
	int n = 1;
	int max = size*size;
	row = size / 2; 
	col = size - 1; 

	while (n <= max)
	{
		if (row == -1 && col == size)
		{
			col = size - 2;
			row = 0;
		}
		else
		{
			if (col == size) {
				col = 0;
			}
			if (row < 0) {
				row = size - 1;
			}			
		}
		if(*(*(square->array + row) + col))
		{
			col = col - 2;
			row++;
			continue;
		}
		else {
			n++;
			*(*(square->array + row) + col) = n;
		}
		row--;
		col++;
	}
	return square;
}

// write_to_file opens up a new file(or overwrites the existing file)
// and writes out the square in the format expected by verify_magic.c
void write_to_file(Square * square, char *filename)
{
	//Create a new file with the specfied or overwrite the existing file
	FILE * file = fopen(filename, "w+");

	//Write the square size as the first line of the file
	int sz = (square->size + '0');
	fputc(sz,file);

	int row, col, curValue = 0;
	for (row = 0; row < square->size; row++) {
		fputc( '\n', file);
		for (col = 0; col < square->size; col++) {
			if (col > 0) {
				fputc(',',file);
			}
			int num = *(*(square->array + row) + col);
			char c = ' ';
			if(num<10){
			c = (num + '0');
			fputc(c, file);
			}
			else{
				printNum(num, file);
			}
			
		}
	}
	//Free square memory
	int size = square->size;
	for(int a = 0; a<size-1; a++){
		free(*(square->array + a));
	}
	free(square->array);
	free(square);
	return;
}

void printNum(int n, FILE * file){
	//Base case
	if(n<1){
		return;
	}
	printNum( n/10, file);
	fputc(((n%10) + '0'), file);
}