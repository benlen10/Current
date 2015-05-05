#include <iostream>
#include <string>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include "guest.h"

const int maxGuests = 500;
int guestCount = 0;
char guests[maxGuests][100];


using namespace std;

add_name(){
	char * temp = (char*) malloc(100);
	printf("Enter Guest Name\n");
	fgets(temp,99, stdin);
	strcpy(guests[guestCount],temp);
}
edit(){
	printf("Edit");
}

remove(){
	printf("Remove");
}


checkin(){
	printf("Checkin");
}

checkout(){
	printf("Checkout");
}

main(){
	printf("Guest List Software Prototype\n");

	while(1){
	char * tmp = (char*) malloc(100);
	printf("Available Actions: <ADD> <REMOVE> <EDIT> <CHECKIN> <CHECKOUT> <EXIT>\n");
	fgets(tmp,99, stdin);
	printf("Input: %s",tmp);
	
	if(!strcmp(tmp,"ADD\n")){
		add_name();
	}
	else if(!strcmp(tmp,"REMOVE\n")){
		remove();
	}
	else if(!strcmp(tmp,"EDIT\n")){
		edit();
	}
	else if(!strcmp(tmp,"CHECKIN\n")){
		checkin();
	}
	else if(!strcmp(tmp,"CHECKOUT\n")){
		checkout();
	}
	else if(!strcmp(tmp,"EXIT\n")){
		printf("Exiting Program");
		return 0;
	}
	else{
		printf("Invalid Command\n");
		return 0;
	}
	
	}

	
	
	
}


