#include "stdafx.h"
#include <string>
#include <stdio.h>
#include<iostream>
using namespace std;

void load(string str);
void locate(string str);


int main()
{
	printf(">");
	//Fetch an input command from the user
	char str[100];
	string loadStr = ("load");
	string locateStr = ("locate");
	fgets(str, 100, stdin);
	string input(str);
	printf("\nINPUT: %s", input.c_str());

	if (input.find(loadStr) != string::npos) {
		string::size_type    start_position = 0;
		string::size_type    end_position = 0;
		string               found_text;
		start_position = input.find("<");
		if (start_position != string::npos)
		{
			++start_position; // start after the double quotes.
							  // look for end position;
			end_position = input.find(">");
			if (end_position != string::npos)
			{
				found_text = input.substr(start_position, end_position - start_position);
			}
		}
		printf("Found: %s", found_text.c_str());
		fgets(str, 100, stdin);
		load(found_text);
	}

	if (input.find(locateStr) != string::npos) {
		string str1;
		locate(str1);
	}

	FILE *fp;
}

void load(string str) {

}

void locate(string str) {

}

