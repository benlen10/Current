// Main File:        generate_magic.c
// This File:        generate_magic.c
// Other Files:      
// Semester:         CS 354 Fall 2016
//
// Author:           Benjamin Lenington
// Email:            lenington@wisc.edu
// CS Login:         lenington

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
	if (argc != 2 ) {
		printf("FATAL ERROR: Invalid argument count\n");
		return -1;
	}
	if(strlen(*(argv+1))>1000){
		printf("FATAL ERROR: Filename exceeds max length\n");
		return -1;
	}

	// Check input arguments to get filename	
	char * filename = *(argv+1);

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
	printf("Enter size of magic square, must be odd\n");
	char * imp = malloc(sizeof(char) * 10);
	if(imp == NULL){
		printf("FATAL ERROR: Malloc failed to allocate memory\n");
		return -1;
	}
	fgets(imp, 3, stdin); //Read numbers up to 99. Three digits numbers will be trimmed to 2 digits
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
		printf("Size must be an odd number >= 3.\n");
		return -1;
	}
}

// generate_magic constructs a magic square of size n
// using the Siamese algorithm and returns the Square struct
//Param: size = specifies the size of magic square to generate
Square * generate_magic(int size)
{
	//Create a new Square object and dynamically allocate space for the array in the heap
	Square  * square = malloc(sizeof(Square));
	if(square == NULL){
		printf("FATAL ERROR: Malloc failed to allocate memory\n");
		return NULL;
	}
	square->size = size;

	square->array = malloc((size*2) * sizeof(int));  //(size*2) is essential to support square sizes up to 99
	if(square->array == NULL){
		printf("FATAL ERROR: Malloc failed to allocate memory\n");
		return NULL;
	}

	//Allocate every row of the 2D Matrix 
	for(int a = 0; a<size; a++){
		*(square->array + a) =  malloc((size*2) * sizeof(int));
		if(*(square->array + a) == NULL){
			printf("FATAL ERROR: Malloc failed to allocate memory\n");
			return NULL;
		}
	}

	//Set all matrix values to zero intitially  
	int row, col;
	for (row = 0; row < size; row++) {
		for (col = 0; col < size; col++) {
			*(*(square->array + row) + col) = 0;
		}
	}
	//Generate magic square values using the Siamese method (Based upon Wikipedia algorithm)
	int n = 1;
	int max = size*size;
	row = size / 2; 
	col = size - 1; 

	while (n <= max)// Loop while cur value is less that the magic square size^2
	{
		if (row == -1 && col == size)  
		{
			//Adjust row and col counters if you hit a corner of the matrix
			col = size - 2;
			row = 0;
		}
		else
		{
			if (col == size) {  //Reset col position if you've hit the end of the matrix
				col = 0;
			}
			if (row < 0) {  //Jump to the last row of the matrix if the col counter is negative
				row = size - 1;
			}			
		}
		if(*(*(square->array + row) + col))
		{
			col = col - 2;
			row++;
			continue;  //After incrementing row, skip to the next oop iteration
		}
		else {
			n++;
			//If the matrix value is not currently set set to the cur n value after incrementing n
			*(*(square->array + row) + col) = n;  
		}
		row--; //Iterate diagionally through matrix
		col++;
	}
	return square;
}

// write_to_file opens up a new file(or overwrites the existing file)
// and writes out the square in the format expected by verify_magic.c
//Param: square = The square stuct to write to the output file
//Param: filename = specifies the filename to save the square too
void write_to_file(Square * square, char *filename)
{
	//Create a new file with the specfied or overwrite the existing file
	FILE * file = fopen(filename, "w+");

	if(file == NULL){
		printf("FATAL ERROR: File Not Found\n");
		return;
	}

	//Write the square size as the first line of the file
	if(square->size<10){
	int sz = (square->size + '0');
	fputc(sz,file);
	}
	else{  //Handle multi digit numbers
		printNum(square->size, file);
	}

	int row, col;
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

//This recursive helper function will print a multi digit int value to the specified output file
//Param: n = the int value to print to the output file
//Param: file = The output file reference to print to
void printNum(int n, FILE * file){
	//Base case
	if(n<1){
		return;
	}
	printNum( n/10, file);
	fputc(((n%10) + '0'), file);
}