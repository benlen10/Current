
#ifndef ROOM
#define ROOM


class Room {
	// Add any variables you need

private:
	

public:
	char name[100];
	char messages[100][100];
	char messageUser[100][100];
	char users[100][100];
	int messageCount = 0;
	int userCount = 0;
};

#endif