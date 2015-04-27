#include <iostream>
#include <string>
#include "users.h"

using namespace std;



Users::Users(void){
	cout<< "Creating new User";
}


Users::set_pass(string ps){
	this->pass = ps;
}

main(){
	
	cout << "Welcome to the user database\n";
	while(1){
	cout << "Available options to type: (1)Create (2)Login (3) Exit\n";
	string imp;
	cin >> imp;
	if (imp== "Create"){
		Users User1;
		cout << "Enter User Name\n";
		cin >> User1.name;
		cout << "User Name:" << User1.name;
		cout << "Enter Password:\n";
		string tmp;
		cin >> tmp;
		User1.set_pass(tmp);
	}
	
	else if (imp == "Login"){
		cout << "Enter user name\n";
		cin
	}
	
	else if (imp == "Exit"){
		cout << "Terminating Program\n";
		return 0;
	}
	else{
		cout << "Invalid Input";
	}

	
}
	return 0;
	
}
	
	