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

int get_square_size();
Square * generate_magic(int size);
void write_to_file(struct Square * square, char *filename);

int main(int argc, char *argv[])
{
	//Check for bad input
	if (argc < 1) {
		return -1;
	}

	// Check input arguments to get filename
	char * filename = argv[0];

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
	int magicSize = getchar();

	//Verify that the number entered is greater than or equal to 3 and an odd number. Else return -1
	if ((magicSize >= 3) && (magicSize % 2 != 0)) {
		return magicSize;
	}
	else {
		return -1;
	}
}

// generate_magic constructs a magic square of size n
// using the Siamese algorithm and returns the Square struct
Square * generate_magic(int size)
{
	//Create a new Square object and allocate space for the array
	struct Square  * square = (Square*)malloc(sizeof(Square));
	square->array = (int**) malloc(size * size * sizeof(int));
	square->size = size;

	//Set all values to zero intitially 
	int ** arr = square->array;
	int row, col, curValue = 0;
	for (row = 0; row < square->size; row++) {
		for (col = 0; col < square->size; col++) {
			*(*(arr + row) + col) = 0;
		}
	}

	//Generate magic square values using the Siamese method
	row = size / 2;
	col = size - 1;

	for (int n = 1; n <= n*n; )
	{
		if (row == -1 && col == n)
		{
			col = n - 2;
			row = 0;
		}
		else
		{
			if (col == n) {
				col = 0;
			}
			if (row < 0) {
				row = n - 1;
			}
			
		}
		if (*(*(square->array + row) + col) == 0)
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
	fputc(square->size, file);

	int ** arr = square->array;
	int row, col, curValue = 0;
	for (row = 0; row < square->size; row++) {
		fputc('\n', file);
		for (col = 0; col < square->size; col++) {
			if (col > 0) {
				fputc(',', file);
			}
			char c = *(*(arr + row) + col);
			fputc(c, file);
		}
	}
}