#include <iostream>
#include <string>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include "guest.h"

const int maxGuests = 500;
int guestCount = 0;
Guest guests[maxGuests];


using namespace std;

add_name(){
	char * temp = (char*) malloc(100);
	int date = 0;
	printf("Enter Guest Name\n");
	fgets(temp,99, stdin);
	strcpy(guests[guestCount].name,temp);
	printf("Enter Birth Date\n");
	scanf("%d", &date);
	guests[guestCount].date = date;
	guestCount++;
	printf("Successfully Added %s\n",temp);
}
edit(){
	char * tmp = (char*) malloc(100);
	printf("Specify what user to edit/n");
	fgets(tmp,99,stdin);
	int i = 0;
	
	while(i<guestCount){
		if(!strcmp(guests[i].name,tmp)){
			break;
		}
		i++;
	}
	printf("Editing Guest: %s",guests[i].name);
	printf("Usage: <NAME> <DATE> <STATE> <STATUS>");
	fgets(tmp,99,stdin);
	if(!strcmp(tmp,"NAME\n")){
		add_name();
	}
	else if(!strcmp(tmp,"NAME\n")){
		printf("Enter new name");
		return 0;
	}
	else if(!strcmp(tmp,"STATE\n")){
		printf("Enter new state");
		return 0;
	}
	else if(!strcmp(tmp,"STATUS\n")){
		printf("Enter new status");
		return 0;
	}

	else{
		printf("Invalid Command\n");
		return 0;
	}
	
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


