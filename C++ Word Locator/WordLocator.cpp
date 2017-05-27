#include "stdafx.h"
#include <string>
#include <stdio.h>
#include<iostream>
#include <sstream>
using namespace std;

//Function prototypes
void load(string str);
void locate(string str, int n);
string parse(string input);


int main()
{
	char str[100];

	while (1) {
		//Fetch an input command from the user and convert to a string
		printf(">");
		fgets(str, 100, stdin);
		string input(str);

		//If input contains "locate"
		if (input.find("load") != string::npos) {
			string filename;
			istringstream iss(input);
			getline(iss, filename, ' ');
			getline(iss, filename, ' ');
			printf("FILENAME: %s", filename.c_str());
			load(filename);
		}

		//If input contains "locate"
		else if (input.find("locate") != string::npos) {
			istringstream iss(input);
			string word;
			string count;
			getline(iss, word, ' ');
			getline(iss, word, ' ');
			getline(iss, count, ' ');
			if (count.length() > 0) {
				int n = stoi(count);
				printf("LOCATE: %s (%d)", word.c_str(), n);
				locate(word, n);
			}
			else {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
		}

		//If input contains "new"
		else if (input.find("new") != string::npos) {
		}

		//If input contains "end"
		else if (input.find("end") != string::npos) {
			return 0;
		}

		//If input is not reconigized
		else {
			fprintf(stderr,"ERROR: Invalid command\n");
		}
	}
	return 0;
}

void load(string str) {

	FILE *fp;
}

void locate(string str, int n) {

}

string parse(string input) {
	int start = 0;
	int end = 0;
	string  result;
	start = input.find("<");
	if (start != string::npos)
	{
		++start; 				  
		end = input.find(">");
		if (end != string::npos)
		{
			result = input.substr(start, end - start);
		}
	}
	return result;
}

