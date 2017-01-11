#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Structure representing Square
// size: dimension(number of rows/columns) of the square
// array: 2D array of integers
typedef struct _Square {
	int size;
	int array[100][100];//int **array;
} Square;

int get_square_size();
Square * generate_magic(int size);
void write_to_file(Square * square, char *filename);
void printNum(int n, FILE * file);

int main(int argc, char *argv[])
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
	char * imp = malloc(sizeof(char) * 10);
	gets(imp);
	int magicSize = atoi(imp);

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
	//Create a new Square object and allocate space for the array
	Square  * square = malloc(sizeof(Square));
	//square->array = (int**) malloc(size * sizeof(square->array) + (size * (size * sizeof(*square->array))));
	square->size = size;

	//Set all values to zero intitially 
	int row, col, curValue = 0;
	for (row = 0; row < size; row++) {
		for (col = 0; col < size; col++) {
			square->array[row][col] = 0;//*((square->array + row) + col) = 0;
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
		if(square->array[row][col]) //if (*(*(square->array + row) + col) == 0)
		{
			col = col - 2;
			row++;
			continue;
		}
		else {
			n++;
			square->array[row][col] = n;//*(*(square->array + row) + col) = n;
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
	FILE * file = fopen("magicWrite.txt", "w+");

	//Write the square size as the first line of the file
	//printf("SIZE: %c", sz);
	int sz = (square->size + '0');
	fputc(sz,file);

	int row, col, curValue = 0;
	for (row = 0; row < square->size; row++) {
		fputc( '\n', file);
		for (col = 0; col < square->size; col++) {
			if (col > 0) {
				fputc(',',file);
			}
			int num = square->array[row][col];
			//printf("ROW: %d COL: %d VAL: %d\n", row, col, tmp);
			char c = ' ';
			if(num<10){
			c = (square->array[row][col] + '0'); //*(*(square->array + row) + col);
			fputc(c, file);
			}
			else{
				printNum(num, file);
			}
			
		}
	}
}

void printNum(int n, FILE * file){
	//Base case
	if(n<1){
		return;
	}
	printNum( n/10, file);
	fputc(((n%10) + '0'), file);
}