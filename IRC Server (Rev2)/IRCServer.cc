
const char * usage =
"                                                               \n"
"IRCServer:                                                   \n"
"                                                               \n"
"Simple server program used to communicate multiple users       \n"
"                                                               \n"
"To use it in one window type:                                  \n"
"                                                               \n"
"   IRCServer <port>                                          \n"
"                                                               \n"
"Where 1024 < port < 65536.                                     \n"
"                                                               \n"
"In another window type:                                        \n"
"                                                               \n"
"   telnet <host> <port>                                        \n"
"                                                               \n"
"where <host> is the name of the machine where talk-server      \n"
"is running. <port> is the port number you used when you run    \n"
"daytime-server.                                                \n"
"                                                               \n";

#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <time.h>


#include "IRCServer.h"
#include "Room.h"

int QueueLength = 5;
int userCount = 0;
Room rooms[100];
int roomCount = 0;

int
IRCServer::open_server_socket(int port) {

	// Set the IP address and port for this server
	struct sockaddr_in serverIPAddress; 
	memset( &serverIPAddress, 0, sizeof(serverIPAddress) );
	serverIPAddress.sin_family = AF_INET;
	serverIPAddress.sin_addr.s_addr = INADDR_ANY;
	serverIPAddress.sin_port = htons((u_short) port);
  
	// Allocate a socket
	int masterSocket =  socket(PF_INET, SOCK_STREAM, 0);
	if ( masterSocket < 0) {
		perror("socket");
		exit( -1 );
	}

	// Set socket options to reuse port. Otherwise we will
	// have to wait about 2 minutes before reusing the sae port number
	int optval = 1; 
	int err = setsockopt(masterSocket, SOL_SOCKET, SO_REUSEADDR, 
			     (char *) &optval, sizeof( int ) );
	
	// Bind the socket to the IP address and port
	int error = bind( masterSocket,
			  (struct sockaddr *)&serverIPAddress,
			  sizeof(serverIPAddress) );
	if ( error ) {
		perror("bind");
		exit( -1 );
	}
	
	// Put socket in listening mode and set the 
	// size of the queue of unprocessed connections
	error = listen( masterSocket, QueueLength);
	if ( error ) {
		perror("listen");
		exit( -1 );
	}

	return masterSocket;
}

void
IRCServer::runServer(int port)
{
	int masterSocket = open_server_socket(port);

	initialize();
	
	while ( 1 ) {
		
		// Accept incoming connections
		struct sockaddr_in clientIPAddress;
		int alen = sizeof( clientIPAddress );
		int slaveSocket = accept( masterSocket,
					  (struct sockaddr *)&clientIPAddress,
					  (socklen_t*)&alen);
		
		if ( slaveSocket < 0 ) {
			perror( "accept" );
			exit( -1 );
		}
		
		// Process request.
		processRequest( slaveSocket );		
	}
}

int
main( int argc, char ** argv )
{
	// Print usage if not enough arguments
	if ( argc < 2 ) {
		fprintf( stderr, "%s", usage );
		exit( -1 );
	}
	
	// Get the port from the arguments
	int port = atoi( argv[1] );

	IRCServer ircServer;
	
	

	// It will never return
	ircServer.runServer(port);
	
}


void
IRCServer::processRequest( int fd )
{
	// Buffer used to store the comand received from the client
	const int MaxCommandLine = 1024;
	char commandLine[ MaxCommandLine + 1 ];
	int commandLineLength = 0;
	int n;
	
	// Currently character read
	unsigned char prevChar = 0;
	unsigned char newChar = 0;
	
	//
	// The client should send COMMAND-LINE\n
	// Read the name of the client character by character until a
	// \n is found.
	//

	// Read character by character until a \n is found or the command string is full.
	while ( commandLineLength < MaxCommandLine &&
		read( fd, &newChar, 1) > 0 ) {

		if (newChar == '\n' && prevChar == '\r') {
			break;
		}
		
		commandLine[ commandLineLength ] = newChar;
		commandLineLength++;

		prevChar = newChar;
	}
	//fprintf(stderr,"RAW INPUT:%s\n",commandLine);
	
	
	// Add null character at the end of the string
	// Eliminate last \r
	commandLineLength--;
        commandLine[ commandLineLength ] = 0;

	printf("RECEIVED: %s\n", commandLine);

	printf("The commandLine has the following format:\n");
	printf("COMMAND <user> <password> <arguments>. See below.\n");

	char * command = (char*) malloc(1000);
	char * user =  (char*) malloc(1000);
	char * password =  (char*) malloc(1000);
	char * args =  (char*) malloc(1000);
	char * args2 =  (char*) malloc(1000);


	//PARSE RAW INPUT STRING 

	if(strstr(commandLine, "ADD-USER")!=NULL){                //ADD-USER
	strcpy(command,"ADD-USER");
	int x =8;
	int y = 0;
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(x<commandLineLength){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}

	password[y] = '\0';
	y = 0;
	}
	
	else if(strstr(commandLine, "GET-ALL-USERS")!=NULL){           //GET-ALL-USERS
	strcpy(command,"GET-ALL-USERS");
	int x =13;
	int y = 0;
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	while(x<commandLineLength){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	}
	
	else if(strstr(commandLine,"CREATE-ROOM")!=NULL){              //CREATE-ROOM
		strcpy(command,"CREATE-ROOM");
	int x =11;
	int y = 0;

	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	
	
	while(x<commandLineLength){       //GET args
	args[y] = commandLine[x];
	y++;
	x++;
	}
	args[y] = '\0';
	y = 0;
	}
	
	else if(strstr(commandLine, "LIST-ROOMS")!=NULL){                //LIST-ROOMS
	strcpy(command,"LIST-ROOMS");
	int x =10;
	int y = 0;
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	while(x<commandLineLength){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	}

	else if(strstr(commandLine,"ENTER-ROOM")!=NULL){                      //ENTER ROOM
		strcpy(command,"ENTER-ROOM");
	int x =10;
	int y = 0;

	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	
	
	while(x<commandLineLength){       //GET args
	args[y] = commandLine[x];
	y++;
	x++;
	}
	args[y] = '\0';
	y = 0;
	}
	
	else if(strstr(commandLine, "LEAVE-ROOM")!=NULL){                     //LEAVE-ROOM
	strcpy(command,"LEAVE-ROOM");
	int x =10;
	int y = 0;
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	}
	
	else if(strstr(commandLine,"SEND-MESSAGE")!=NULL){                 //SEND-MESSAGE
		strcpy(command,"SEND-MESSAGE");
	int x =12;
	int y = 0;

	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Room
	args[y] = commandLine[x];
	y++;
	x++;
	}
	y = 0;
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(x<commandLineLength){       //GET Message
	args2[y] = commandLine[x];
	y++;
	x++;
	}
	args2[y] = '\0';
	y = 0;
	}
	
	
	else if(strstr(commandLine,"GET-MESSAGES")!=NULL){              //GET-MESSAGES
		strcpy(command,"GET-MESSAGES");
	int x =12;
	int y = 0;

	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Args
	args[y] = commandLine[x];
	y++;
	x++;
	}
	args[y] = '\0';
	y = 0;
	
	
	while(x<commandLineLength){       //GET args2
	args2[y] = commandLine[x];
	y++;
	x++;
	}
	args2[y] = '\0';
	y = 0;
	}
	
	else if(strstr(commandLine,"GET-USERS-IN-ROOM")!=NULL){                      //GET-USERS-IN-ROOM
		strcpy(command,"GET-USERS-IN-ROOM");
	int x =17;
	int y = 0;

	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}

	while(commandLine[x]!= ' '){       //GET User
	user[y] = commandLine[x];
	y++;
	x++;
	}
	user[y] = '\0';
	y = 0;
	
	while(commandLine[x]== ' '){      //Skip middle spaces
	x++;
	}
	
	while(commandLine[x]!= ' '){       //GET Password
	password[y] = commandLine[x];
	y++;
	x++;
	}
	password[y] = '\0';
	y = 0;
	
	
	while(x<commandLineLength){       //GET args
	args[y] = commandLine[x];
	y++;
	x++;
	}
	args[y] = '\0';
	y = 0;
	}
	

	else if(strstr("LEAVE-ROOM",commandLine)!=NULL){
		strcpy(command,"LEAVE-ROOM");
	}

	else if(strstr("SEND-MESSAGE",commandLine)!=NULL){
		strcpy(command,"SEND-MESSAGE");
	}

	else if(strstr("GET-MESSAGE",commandLine)!=NULL){
		strcpy(command,"GET-MESSAGE");
	}

	else if(strstr("GET-USERS-IN-ROOM",commandLine)!=NULL){
		strcpy(command,"GET-USERS-IN-ROOM");
	}
	
	else if(strstr("GET-ALL-USERS",commandLine)!=NULL){
		strcpy(command,"GET-ALL-USERS");
	}
	
	else{
		strcpy(command,"Unknown");
	}
															 //CHECK Commands
	if (!strcmp(command, "ADD-USER")) {
		
		addUser(fd, user, password, args);
	}
	else if (!strcmp(command, "ENTER-ROOM")) {
		enterRoom(fd, user, password, args);
	}
	else if (!strcmp(command, "LEAVE-ROOM")) {
		leaveRoom(fd, user, password);
	}
	else if (!strcmp(command, "SEND-MESSAGE")) {
		sendMessage(fd, user, password, args,args2);
	}
	else if (!strcmp(command, "GET-MESSAGES")) {
		getMessages(fd, user, password, args,args2);
	}

	else if (!strcmp(command, "GET-USERS-IN-ROOM")) {
		getUsersInRoom(fd, user, password, args);
	}
	else if (!strcmp(command, "CREATE-ROOM")) {
		createRoom(fd, user, password, args);
	}
	else if (!strcmp(command, "LIST-ROOMS")) {
		listRooms(fd, user, password);
	}
	else if (!strcmp(command, "GET-ALL-USERS")) {
		getAllUsers(fd, user, password, args);
	}
	else {
		const char * msg =  "UNKNOWN COMMAND\r\n";
		write(fd, msg, strlen(msg));
	}

	// Send OK answer
	//const char * msg =  "OK\n";
	//write(fd, msg, strlen(msg));

	close(fd);	
}

void
IRCServer::initialize()
{
	// Open password file
  passFile = fopen("password.txt", "a+");
	

	// Initialize users
	char * str = (char*) malloc(1000);
	char text[10000]; 
	int y = 0;
	int x = 0;
	while(fgets(str, 100, passFile) != NULL){
		y = 0;
		while(*str!='^'){
			users[x][y] = *str;
			y++;
			str++;
		}
		users[x][y] = '\0';
		userCount++;
		x++;
		}
		
	char * temp = (char*) malloc(1000);             //SORT USers
	int stat = 0;
	int st = 0;
	x=0;
	while(st == 0){
		stat = 0;
		x=0;
	while(x<(userCount-1)){
		if(strcmp(users[x],users[x+1])>0){

			stat = 1;
			strcpy(temp,users[x]);
			strcpy(users[x],users[x+1]);
			strcpy(users[x+1],temp);
		}
		x++;
	}
	if(stat==0){
		st = 1;
	}
	}

}

bool
IRCServer::checkPassword(int fd, const char * user, const char * password) {
	// Here check the password
	passFile = fopen("password.txt", "r+");
	//fprintf(stderr,"LOC1");
	char * str = (char*) malloc(1000);
	char * str2 = (char*) malloc(1000);
	strcat(str2,user);
	strcat(str2,"^");
	strcat(str2,password);
	strcat(str2,"\n");
	
	
	while(fgets(str, 1000, passFile) != NULL){
		//fprintf(stderr,"LOC2");
		//fprintf(stderr,"PASS:%s%d CHECK:%s%d\n",str2,strlen(str2),str,strlen(str));
		if(strcmp(str2,str)==0){
	return true;
		}
	}
	
		
		fclose(passFile);
		return false;
}

void
IRCServer::addUser(int fd, const char * user, const char * password, const char * args)
{
	//fprintf(stderr,"PASS:%s",password);
	
	int x = 0;
	while(x<userCount){
		if(strcmp(user, users[x]) == 0){             //Check if user already exists
			write(fd, "DENIED\r\n", 9);
					fprintf(stderr,"User Already Exists\n");
			return;
		}
		x++;
	}
	//fprintf(stderr,"RECIEVED: User: %s Pass %s",user,password);
	x = 0;
	strcpy(users[userCount],user);            //Add to master users list;
	userCount++;
	passFile = fopen("password.txt", "a");
	fprintf(passFile,"%s^%s\n",user,password);
	fclose(passFile);                       	//SORT Users
	
	char * temp = (char*) malloc(1000);
	int stat = 0;
	int st = 0;
	x=0;
	if(userCount>1){
	while(st == 0){
		stat = 0;
		x=0;
		
	while(x<(userCount-1)){
		if(strcmp(users[x],users[x+1])>0){
			stat = 1;
			strcpy(temp,users[x]);
			strcpy(users[x],users[x+1]);
			strcpy(users[x+1],temp);
		}
		x++;
	}
	if(stat==0){
		st = 1;
	}
	}
	}
	
	
		
	
	
	fprintf(stderr,"User Added\n");
	write(fd, "OK\r\n", 5);
	return;		
}

void
IRCServer::createRoom(int fd, const char * user, const char * password, const char * args)
{
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	int y = 0;
	while(y<roomCount){
		if(strcmp(rooms[y].name, args)==0){
			write(fd, "DENIED\r\n",9);
			fprintf(stderr,"Room Already Exists\n");
			return;
		}
		y++;
	}
		
		strcpy(rooms[roomCount].name,args);		
		write(fd, "OK\r\n",5);
		fprintf(stderr,"Room Created\n");
		roomCount++;
		return;
	
}

void
IRCServer::listRooms(int fd, const char * user, const char * password)
{
	char * str = (char*) malloc(1000);
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	int y = 0;
	while(y<roomCount){
		fprintf(stderr,"Room:%s\n",rooms[y].name);
		sprintf(str,"%s\r\n",rooms[y].name);
			write(fd, str, strlen(str));	
			y++;
			}
			write(fd, "\r\n",3);
			return;

}

void
IRCServer::enterRoom(int fd, const char * user, const char * password, const char * args)
{
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	char * temp = (char*) malloc(1000); 
	
	int x = 0;
		int y = 0;
	
	
	x = 0;
	y=0;
	while(x<roomCount){
		if(strcmp(rooms[x].name,args)==0){
			y=0;
	while(y<rooms[x].userCount){
	if(strcmp(rooms[x].users[y],user)==0){
	fprintf(stderr,"User already in room\r\n");
				write(fd, "OK\r\n",5);
	return;
	}
	y++;
	}
			strcpy(rooms[x].users[rooms[x].userCount],user);
	rooms[x].userCount++;
	//fprintf(stderr,"Increasing User Count: %d\n",rooms[x].userCount);
	write(fd, "OK\r\n",5);
	fprintf(stderr,"User Entered Room\n");
	
	int stat = 0;
	int st = 0;
	
	if(rooms[x].userCount>1){                         //SORT USers
		fprintf(stderr,"SORT Room users\n");
	while(st == 0){
		stat = 0;
		int y =0;
	while(y<(rooms[x].userCount-1)){
		if(strcmp(rooms[x].users[y],rooms[x].users[y+1])>0){

			stat = 1;
			strcpy(temp,rooms[x].users[y]);
			strcpy(rooms[x].users[y],rooms[x].users[y+1]);
			strcpy(rooms[x].users[y+1],temp);
		}
		y++;
	}
	if(stat==0){
		st = 1;
	}
	}
	}
	
	return;
	
		}
		x++;
	}
	

	char * str = "ERROR (No room)\r\n";
				write(fd, str,strlen(str));
	return;
}

void
IRCServer::leaveRoom(int fd, const char * user, const char * password)
{
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	int x = 0;
		int y = 0;
	while(x<roomCount){
	y=0;
	while(y<rooms[x].userCount){
	if(strcmp(rooms[x].users[y],user)==0){
	rooms[x].users[y][0] = '\0';
	fprintf(stderr,"User Removed From Room");
	write(fd, "OK\r\n",5);
	return;
	}
	y++;
	}
	x++;
	}	
	
	char * str = "ERROR (No user in room)\r\n";
				write(fd, str,strlen(str));
	return;
}



void
IRCServer::sendMessage(int fd, const char * user, const char * password, const char * args, const char * args2)
{
	
	
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	int x = 0;
	int i = 0;
	bool status = false;
	
	
	while(x<=roomCount){
		//fprintf(stderr,"LOC0");
		//fprintf(stderr,"ROOM NAME:%s\n",args);
		//fprintf(stderr,"ROOM:%s\n",rooms[x].name);
		
		if(strstr(rooms[x].name,args)!=NULL){
			//fprintf(stderr,"ROOM:%s\n",rooms[x].name);
			//fprintf(stderr,"LOC1");
			//fprintf(stderr,"UserCount%d\n",rooms[x].userCount);
			while(i<rooms[x].userCount){             //Verify that user is in room
			
			if(strcmp(rooms[x].users[i],user)==0){
					status = true;
			}
			
			i++;
			}
			
			if(status == false){
				char * str = "ERROR (user not in room)\r\n";
				write(fd, str,strlen(str));
				return;
			}
			//fprintf(stderr,"LOC3");
			if(rooms[x].messageCount >= 100){        //Wraparound if more than 100 messages
				rooms[x].messageCount = 0;
			}
			strcpy(rooms[x].messages[rooms[x].messageCount],args2);
			strcpy(rooms[x].messageUser[rooms[x].messageCount],user);
	if(rooms[x].messageCount<100){
	rooms[x].messageCount++;
	}
	write(fd, "OK\r\n",5);
	return;
		}
		x++;
	}
	write(fd, "DENIED\r\n",9);
	fprintf(stderr,"Room Not Found\n");
	return;

}

void
IRCServer::getMessages(int fd, const char * user, const char * password, const char * args, const char * args2)
{
	char * str = (char*) malloc(1000);
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	
	int x = 0;
	int y = 0;
	int i = atoi(args);
	y = (y + i);
	int u = 0;
	bool status = false;
	while(x<roomCount){
		if(strcmp(rooms[x].name,args2)==0){
			while(u<rooms[x].userCount){             //Verify that user is in room
			
			if(strcmp(rooms[x].users[u],user)==0){
					status = true;
			}
			
			u++;
			}
			
			if(status == false){
				char * str = "ERROR (User not in room)\r\n";
				write(fd, str,strlen(str));
				return;
			}
			if(i>(rooms[x].messageCount-1)){
				sprintf(str,"NO-NEW-MESSAGES\r\n");
				write(fd, str,strlen(str));
				return;
			}
			
				
			while(y<rooms[x].messageCount){
				sprintf(str,"%d %s %s\r\n",y,rooms[x].messageUser[y],rooms[x].messages[y]);
				write(fd, str,strlen(str));
				y++;
			}
			sprintf(str,"\r\n");
			write(fd, str, strlen(str));
			return;
		}
		x++;
	}
	fprintf(stderr,"Room does not exist");
	write(fd, "DENIED\r\n",9);
	return;
	
}

void
IRCServer::getUsersInRoom(int fd, const char * user, const char * password, const char * args)
{
	char * temp = (char*) malloc(1000);
	char str[1000];
	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	
	
	int y =0;
	int x = 0;
	while(x<roomCount){
		if(strcmp(rooms[x].name,args)==0){
			

	
			while(y<rooms[x].userCount){
				if(strlen(rooms[x].users[y])>1){
	sprintf(str,"%s\r\n",rooms[x].users[y]);
write(fd, str, strlen(str));	
				}
	y++;
	}
	sprintf(str,"\r\n");
write(fd, str, strlen(str));
	
	
	return;
		}
		x++;
	}
	write(fd, "DENIED\r\n",9);
	fprintf(stderr,"Room not found\n");
	
	
}

void
IRCServer::getAllUsers(int fd, const char * user, const char * password,const  char * args)
{
	char str[1000];

	if(!(IRCServer::checkPassword(fd,user,password))){
		char * str = "ERROR (Wrong password)\r\n";
				write(fd, str,strlen(str));
	return;
	}
	
	int x = 0;
	while(x<userCount){
		
		sprintf(str,"%s\r\n",users[x]);
write(fd, str, strlen(str));		
		x++;
	
	}
	sprintf(str,"\r\n");
write(fd, str, strlen(str));
	
	return;
}

