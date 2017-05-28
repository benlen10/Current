#include "stdafx.h"
#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include<iostream>
#include <sstream>
#include <regex>
#include <iostream>
#include <fstream>
using namespace std;

struct TreeNode {
	string item;
	int location[100];
	int count;
	TreeNode *left;
	TreeNode *right;
	TreeNode(string str, int n) {
		location[0] = n;
		count = 1;
		item = str;
		left = NULL;
		right = NULL;
	}
};

//Function prototypes
void load(string str);
void locate(string str, int n);
bool isValidInt(string input);
TreeNode * TreeContains(TreeNode *root, string item);
void treeInsert(TreeNode *&root, string newItem, int n);
int SearchTree(TreeNode *root, string item, int n);
void DeleteTree(TreeNode *root);

TreeNode *root;

int main()
{
	char str[100];
	root = NULL;

	while (1) {
		//Fetch an input command from the user and convert to a string
		printf(">");
		fgets(str, 100, stdin);
		string input(str);

		//Convert the command to lower case
		string command(str);
		transform(command.begin(), command.end(), command.begin(), tolower);

		//If input contains "locate"
		if (command.find("load") != string::npos) {
			string filename;
			istringstream iss(input);
			getline(iss, filename, ' ');
			getline(iss, filename, '\n');
			//Check for extra content
			if (filename.find(" ") != string::npos) {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
			else if (filename.length() > 0) {
				printf("FILENAME: '%s'", filename.c_str());
				if (root != NULL) {
					DeleteTree(root);
					root = NULL;
				}
				load(filename);
			}
			else {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
		}

		//If input contains "locate"
		else if (command.find("locate") != string::npos) {
			istringstream stream(input);
			string word;
			string count;
			getline(stream, word, ' ');
			getline(stream, word, ' ');
			getline(stream, count, '\n');
			//Check for extra content
			if (count.find(" ") != string::npos) {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
			else if ((count.length() > 0) && isValidInt(count)) {
				int n = stoi(count);
				printf("LOCATE: %s (%d)", word.c_str(), n);
				locate(word, n);
			}
			else {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
		}

		//If input contains "new"
		else if (command.find("new") != string::npos) {
			DeleteTree(root);
			root = NULL;
		}

		//If input contains "end"
		else if (command.find("end") != string::npos) {
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
	int location = 0;
	ifstream file;
	file.open(str);
	if (!file.good()) {
		return;
	}
	while (!file.eof())
	{
		char buf[1000];
		file.getline(buf, 1000);
		int n = 0;
		const char* words[25] = {};
		words[0] = strtok(buf, " ");
		if (words[0])
		{
			for (n = 1; n < 25; n++)
			{
				words[n] = strtok(0, " ");
				if (!words[n]) { break; }	
			}
		}
		for (int i = 0; i < n; i++) {
			string str(words[i]);
			TreeNode * node = TreeContains(root, str);
			if(node != NULL){
				//If duplicate word is found
				node->location[node->count] = location;
				node->count++;
				printf("UPDATE: %s NEW COUNT: %d LOCATION: %d\n", words[i], node->count, location);
			}
			else {
				//Word does not exist. Insert new entry
				printf("INSERT: %s\n", words[i]);
				treeInsert(root, str, location);
			}
			location++;
		}
	}
}


void locate(string str, int n) {
	//Bad input check
	if (n < 1) {
		fprintf(stderr, "ERROR: Invalid command\n");
		return;
	}

	int result = SearchTree(root, str, (n-1));
	if (result >= 0) {
		printf("%d", SearchTree(root, str, (n-1)));
	}
	else {
		fprintf(stderr, "No matching entry");
	}
}


bool isValidInt(string input) {
return (input.find_first_not_of("0123456789") == string::npos);
}

//Only used to inset new items
void treeInsert(TreeNode *&root, string newItem, int n) {
	if (root == NULL) {
		root = new TreeNode(newItem, n);
		return;
	}
	else if (newItem < root->item) {
		treeInsert(root->left, newItem, n);
	}
	else {
		treeInsert(root->right, newItem, n);
	}
}  

//Search the tree for a word and return the node if found
TreeNode * TreeContains(TreeNode *root, string item) {
	if (root == NULL) {
		return NULL;
	}
	else if ((item == root->item)) {
		return root;
	}
	else if (item < root->item) {
		return TreeContains(root->left, item);
	}
	else {
		return TreeContains(root->right, item);
	}
} 

//SEarch the tree for the specific word occourance and return location
int SearchTree(TreeNode *root, string item, int n) {
	if (root == NULL) {
		return -1;
	}
	else if ((item == root->item)) {
		return root->location[n];
	}
	else if (item < root->item) {
		return SearchTree(root->left, item, n);
	}
	else {
		return SearchTree(root->right, item, n);
	}
}

void DeleteTree(TreeNode *root) {
	if (root != NULL)
	{
		DeleteTree(root->left);
		DeleteTree(root->right);
		delete(root);
		if (root->left != NULL)
			root->left = NULL;
		if (root->right != NULL)
			root->right = NULL;
		root = NULL;
	}
}
