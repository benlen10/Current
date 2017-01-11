#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <iostream>
#include <fstream>

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

int main1(int argc, char *argv[])
{
	//Check for bad input
	if (argc < 1) {
		return -1;
	}

	// Check input arguments to get filename
	
	char * filename = argv[0];
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
	write_to_file(square, "temp");

	return 0;
}

// get_square_size prompts the user for the magic square size
// checks if it is an odd number >= 3 and returns the number
int get_square_size()
{
	puts("Enter size of magic square, must be odd");
	int magicSize = getchar();
	magicSize -= 48;


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
	printf("SIZE: %d\n", size);
	//Create a new Square object and allocate space for the array
	struct Square  * square = (Square*)malloc(sizeof(Square));
	square->array = (int**) malloc((size +5) * sizeof(square->array) + (size * ((size + 5) * sizeof(*square->array))));
	square->size = size;

	//Set all values to zero intitially 
	int row, col, curValue = 0;
	for (row = 0; row < size; row++) {
		printf("%s", "\nOUTER\n");
		for (col = 0; col < size; col++) {
			printf("ROW: %d COL: %d\n", row, col);
			*(*(square->array + row) + col) = 0;
		}
	}

	//Generate magic square values using the Siamese method
	row = size / 2;
	col = size - 1;

	for (int n = 1; n <= size*size; )
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
		printf("LOOP\n");
	}
	return square;
}

// write_to_file opens up a new file(or overwrites the existing file)
// and writes out the square in the format expected by verify_magic.c
void write_to_file(Square * square, char *filename)
{
	//Create a new file with the specfied or overwrite the existing file
	//FILE * file = fopen("magic.txt", "w");
	std::ofstream file;
	file.open("example.txt");

	//Write the square size as the first line of the file
	//printf("SIZE: %c", sz);
	int sz = (square->size + '0');
	file << sz;

	int row, col, curValue = 0;
	for (row = 0; row < square->size; row++) {
		file << '\n'; //fputc((int) '\n', file);
		for (col = 0; col < square->size; col++) {
			if (col > 0) {
				file << ','; //
			}
			char c = *(*(square->array + row) + col);
			file << c; //fputc(c, file);
		}
	}
}