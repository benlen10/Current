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
	 * Parse a raw string and return the string representation of a string or decimal
     * Set the string to "NULL" if the input is invalid or empty
     * @param str			the string to parse
	**/
std::string parseDecimal(const char * str);

#pragma endregion