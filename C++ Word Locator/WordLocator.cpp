#include "stdafx.h"
#include <string>
#include <stdio.h>
#include<iostream>
using namespace std;

//Function prototypes
void load(string str);
void locate(string str);
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
			string filename = parse(input);
			printf("Found: %s", filename.c_str());
			fgets(str, 100, stdin);
			load(filename);
		}

		//If input contains "locate"
		if (input.find("locate") != string::npos) {
			string word = parse(input);
			locate(word);
		}

		//If input contains "new"
		if (input.find("new") != string::npos) {
		}

		//If input contains "end"
		if (input.find("end") != string::npos) {
			return 0;
		}
	}
	return 0;
}

void load(string str) {

	FILE *fp;
}

void locate(string str) {

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

