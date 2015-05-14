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
	int i = 0;
	int date;
	int status;
	printf("Specify what user to edit/n");
	fgets(tmp,99,stdin);
	
	while(i<guestCount){
		if(!strcmp(guests[i].name,tmp)){
			break;
		}
		i++;
	}
	if(!strcmp(guests[i].name,tmp)){
		printf("ERROR: Guest not found");
			return 0;
		}
	printf("Editing Guest: %s",guests[i].name);
	printf("Usage: <NAME> <DATE> <STATE> <STATUS>");
	fgets(tmp,99,stdin);
	if(!strcmp(tmp,"NAME\n")){
		printf("Enter new name");
		fgets(tmp,99,stdin);
		strcpy(guests[i].name,tmp);
		return 0;
	}
	else if(!strcmp(tmp,"DATE\n")){
		printf("Enter new birth date");
		scanf("%d", &date);
		guests[i].date = date;
		return 0;
	}
	else if(!strcmp(tmp,"STATE\n")){
		printf("Enter new state");
		fgets(tmp,99,stdin);
		strcpy(guests[i].state,tmp);
		return 0;
	}
	else if(!strcmp(tmp,"STATUS\n")){
		printf("Enter new status");
		scanf("%d", &status);
		guests[i].status = status;
		return 0;
	}

	else{
		printf("Invalid Command\n");
		return 0;
	}
	
}

remove(){
	int i = 0;
	char * tmp = (char*) malloc(100);
	printf("Type guest to remove/n");
	fgets(tmp,99,stdin);
	while(i<guestCount){
		if(!strcmp(guests[i].name,tmp)){
			break;
		}
		i++;
	}
	if(!strcmp(guests[i].name,tmp)){
		printf("ERROR: Guest not found");
			return 0;
		}
		strcpy(guests[i].name,NULL);
		printf("Successfully removed guest");
}


checkin(){
	//Status 1 signifies guest if currently checked in
	char * tmp = (char*) malloc(100);
	int status;
	int i = 0;
	printf("Specify what user to edit/n");
	fgets(tmp,99,stdin);
	
	while(i<guestCount){
		if(!strcmp(guests[i].name,tmp)){
			break;
		}
		i++;
	}
	if(!strcmp(guests[i].name,tmp)){
		printf("ERROR: Guest not found");
			return 0;
		}
		guests[i].status = 1;
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
	printf("LOC1");
	
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


