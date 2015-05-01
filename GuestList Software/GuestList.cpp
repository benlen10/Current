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

main(){
	printf("Guest List Software Prototype\n");
	
	char * tmp = (char*) malloc(100);
	fgets(tmp,99, stdin);
	printf("Enter Guest Name\n");
	strcpy(guests[guestCount],tmp);
	
	
}