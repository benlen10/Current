// Main File:        verify_magic.c
// This File:        verify_magic.c
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
	int ** array;
} Square;

Square * construct_square(char *filename);
int verify_magic(Square * square);

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

	// Construct square from the given file
	char * filename = *(argv+1);
	Square * square = construct_square(filename);

	if(square == NULL){
		printf("false\n");
		return -1; //IF construct_square returns an error value, print false and exit program	
	}

	// Verify if it's a magic square and print true or false
	if (verify_magic(square) == 1) {
		printf("true\n");
	}
	else {
		printf("false\n");
	}
	return 0;
}

// construct_square reads the input file to initialize a square struct
// from the contents of the file and returns the square.
// Param: filename = the filename for the output txt file
Square * construct_square(char *filename)
{
	// Open and read the file
	FILE *file = fopen(filename, "r");

	if(file == NULL){
		printf("FATAL ERROR: File Not Found\n");
		return NULL;
	}
	
	//Declare a temporary variable that will store the raw chars from each line from the input file	
 	char * line = malloc(sizeof(char) * 1000); 
	 if(line == NULL){
		printf("FATAL ERROR: Malloc failed to allocate memory\n");
		return NULL;
	}

	int size = 0;  //Size of the magic square

	//Get the square size from the first line of the file (Max supported square size = 99)
	fgets(line, 3, file);
	size  = atoi(line); //Handle multi digit square sizes > 9

	//Check for invalid square sizes
	if((size%2==0)||size<1||size>99){
		fprintf(stderr, "FATAL ERROR: Invalid magic square size\n");
		return NULL;
	}

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

	int row, col,x;  //Declare temp variables for use within the loops. Var x tracks the position of multi digit numbers > 10

	// Read the rest of the file to fill up the square
	//fgets(line, 1000, file);
	for (row = 0; row < size; row++) {
		fgets(line, 1000, file);
		for (col = 0; col < size; col++) {
			if (*line == ',') { //Skip over any commas
				line++;
			}
			//Check for invalid chars (including a duplicate comma) after skipping a comma
			if(((*line) - '0')<0 || ((*line) - '0')>9){
				printf("FATAL ERROR: File contains invalid char\n");
				return NULL;
			}

			//Handle single digit numbers
			if(*(line + 1) == ',' || ((*(line + 1) - '0')<0) || ((*(line + 1) - '0')>9)){
				*(*(square->array + row) + col) =  (*line - '0');
			}
			else{ //Handle numbers >10
				char * digits = malloc(sizeof(char) * 20); //Allocate space for square sizes up to 99
				if(digits == NULL){
					printf("FATAL ERROR: Malloc failed to allocate memory\n");
					return NULL;
				}
				x=0;
				while((((*(line) - '0')>=0) && ((*(line) - '0')<=9))){
					//Check for invalid chars when parsing multi digit numbers
					if((*line - '0')<0 || (*line - '0')>9){
						printf("FATAL ERROR: File contains invalid char\n");
						return NULL;
					}
					*(digits + x) = *line;	
					x++;
					line++;
				}
			*(digits + x) = '\0';
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
//Param: square = the square struct that is to be verfied by the function
int verify_magic(Square * square)
{
	int row, col, c;  //Declare temp variables that are used within the verification loops
	
	int curSum, magicSum = 0; //curSum tracks the current sum for each iteration of the loop while magic sum holds the total for each row, col and diag. 

	// Verify all all cols within each row are equal
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
	
	//Verify that all rows within each col are equal
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

	//Verify main diagonal
	curSum = 0;
	for (row = 0, col = 0; row < square->size; row++, col++) {
		c = *(*(square->array + row) + col);
		curSum += c;
	}
	if(curSum!=magicSum){
		return 0;
	}

	//Verify secondary diagonal
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
