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
	
	//Fetch an input command from the user
	char str[100];
	string loadStr = ("load");
	string locateStr = ("locate");

	while (1) {
		printf(">");
		fgets(str, 100, stdin);
		string input(str);

		if (input.find(loadStr) != string::npos) {
			string filename = parse(input);
			printf("Found: %s", filename.c_str());
			fgets(str, 100, stdin);
			load(filename);
		}

		if (input.find(locateStr) != string::npos) {
			string word = parse(input);
			locate(word);
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

