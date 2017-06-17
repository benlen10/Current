/**
 * @author Benjamin Lenington (lenington@wisc.edu)
 *
 * @section LICENSE
 * Copyright (c) 2012 Database Group, Computer Sciences Department, University of Wisconsin-Madison.
 */
#include <string>

#pragma region Functions

 /**
  * Generate a new sqlite3 tables for the USDA Nutrition database
 **/
void generateTable();

/**
 * Parse each USDA database text file in the current working directory to populate the tables
**/
void populateTable();

/**
 * Parse a raw string and replace '~' chars with '\"'
 * Set the string to "NULL" if the input is invalid or empty
 * @param str			the string to parse
**/
std::string parseString(const char * str);

/**
 * Parse a raw string and return the string representation of a decimal value
 * Set the string to "NULL" if the input is invalid or empty
 * @param str			the string to parse
**/
std::string parseDecimal(const char * str);

/**
* Parse string (with ~ symbols) as a decimal without quotes
* Set the string to "NULL" if the input is invalid or empty
* @param str			the string to parse
**/
std::string parseStringAsDecimal(const char * str);

/**
* Parse a raw string and add quotes to the string
* Set the string to "NULL" if the input is invalid or empty
* @param str			the string to parse
**/
std::string parseStringAndAddQuotes(const char * str);

/**
* Modified version of the C strtok function
* If no chars exist between the current delims, return NULL
* @param str			the string to parse
* @param delims			the delim chars to split the string
**/
char * tokenize(char * str, char const * delims);

/**
* A placeholder callback function for the sqlite3_exec function
* @param NotUsed		dummey void pointer
* @param argc			current arg count
* @param argv			function arguments
* @param azColName	    col names for the current SQL call
**/
static int callback(void *NotUsed, int argc, char **argv, char **azColName);

#pragma endregion