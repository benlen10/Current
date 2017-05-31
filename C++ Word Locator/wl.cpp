// File: wl.cpp
// 
//  Description: This program stores individual words from a text file in a 
//  tree structure. This program allows you to determine the location of 
//  a specific word occurrence 
//  Student Name: Benjamin Lenington
//  UW Campus ID: 9070894390
//  email: lenington@wisc.edu

#include <string.h>
#include <cstring>
#include <algorithm>
#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include<iostream>
#include <sstream>
#include <iostream>
#include <fstream>
using namespace std;

//Macro definitions 
#define MAX_INPUT_LENGTH 100
#define MAX_DUP_WORDS 200
#define BUFFER_SIZE 1000

// <summary>
// The structure for a single node of within the binary tree
// </summary>
struct TreeNode {
	string item;
	int location[MAX_DUP_WORDS];
	int count;
	TreeNode *left;
	TreeNode *right;
	TreeNode(string str, int n) {
		location[1] = n;
		count = 1;
		item = str;
		left = NULL;
		right = NULL;
	}
};

//The root node for the binary tree
TreeNode *root;

//Function prototypes
void load(string str);
void locate(string str, int n);
bool isValidInt(string input);
TreeNode * TreeContains(TreeNode *root, string item);
void treeInsert(TreeNode *&root, string newItem, int n);
int SearchTree(TreeNode *root, string item, int n);
void DeleteTree(TreeNode *root);
void CleanString(string &str);

// <summary>
// Entry point for the program. Handles user input within an infinte loop
// </summary>
int main()
{
	char str[MAX_INPUT_LENGTH];
	root = NULL;

	while (1) {
		//Fetch an input command from the user and convert to a string
		printf(">");
		fgets(str, MAX_INPUT_LENGTH, stdin);
		string origInput(str);
		string input(str);

		//Convert the input to lower case
		transform(input.begin(), input.end(), input.begin(), ::tolower);

		//If input contains "locate"
		if (input.find("load") != string::npos) {
			string filename;
			istringstream iss(origInput);
			getline(iss, filename, ' ');
			getline(iss, filename, '\n');
			//Check for extra content
			if (filename.find(" ") != string::npos) {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
			else if (filename.length() > 0) {
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
		else if (input.find("locate") != string::npos) {
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
				int n;
				sscanf(count.c_str(), "%d", &n);
				locate(word, n);
			}
			else {
				fprintf(stderr, "ERROR: Invalid command\n");
			}
		}

		//If input contains "new"
		else if (input.find("new") != string::npos) {
			DeleteTree(root);
			root = NULL;
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

// <summary>
// Load the text file into a binary tree
// </summary>
void load(string str) {
	int location = 1;
	ifstream file;
	file.open(str.c_str());
	if (!file.good()) {
		return;
	}
	while (!file.eof())
	{
		char buf[BUFFER_SIZE];
		file.getline(buf, BUFFER_SIZE);
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

			//Convert the word to a lower case string and remove extra punctuation
			string str(words[i]);
			transform(str.begin(), str.end(), str.begin(), ::tolower);
			CleanString(str);

			//Check if the tree already contains the word
			TreeNode * node = TreeContains(root, str);
			if(node != NULL){
				//If duplicate word is found
				node->count++;
				node->location[node->count] = location;
			}
			else {
				//Word does not exist. Insert new entry
				treeInsert(root, str, location);
			}
			location++;
		}
	}
}

// <summary>
// Attempt to locate the specified occourance of a word
// Print the location of the word if found. Otherwise print an error.
// </summary>
void locate(string str, int n) {
	//Bad input check
	if (n < 1) {
		fprintf(stderr, "ERROR: Invalid command\n");
		return;
	}

	int result = SearchTree(root, str, (n));
	if (result >= 0) {
		printf("%d\n", SearchTree(root, str, (n)));
	}
	else {
		fprintf(stderr, "No matching entry\n");
	}
}

// <summary>
// Return true if the string represents a vaild int
// </summary>
bool isValidInt(string input) {
return (input.find_first_not_of("0123456789") == string::npos);
}

// <summary>
// Insert a new node into the binary tree
// </summary>
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

// <summary>
// Return a pointer to the node if the word exists.
// Otherwise return null
// </summary>
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

// <summary>
// Search the tree for the specific word occourance and return the location. 
// Return -1 if the specified occourance is not found
// </summary>
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

// <summary>
// Delete the entire tree, node by node
// </summary>
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

// <summary>
// Remove punctuation from the string
// </summary>
void CleanString(string &str) {
	char list[] = { '.', ',', '!', ';', '?'};
	for (unsigned int i = 0; i < strlen(list); ++i) {
		str.erase(std::remove(str.begin(), str.end(), list[i]), str.end());
	}
}
