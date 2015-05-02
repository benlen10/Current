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

main(){
	printf("Guest List Software Prototype\n");
	
	char * tmp = (char*) malloc(100);
	printf("Available Actions: <ADD> <REMOVE> <EDIT> <CHECKIN> <CHECKOUT>\n");
	fgets(tmp,99, stdin);
	if(!strcmp(tmp,"ADD")){
		add_name();
	}
	
	
}


