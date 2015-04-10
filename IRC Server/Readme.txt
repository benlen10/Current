This is a simple IRC (Internet Relay Chat) Server program.
It will take a port number as an argument and run untill killed manually.
It will store the current users and passwords into passwords.txt
This program is based around multiple rooms that users can create, enter and send messages in.
After 100 messages in a room they will begin to wraparound.
ARGUMENTS:
ADD-USER <USER> <PASSWD>
GET-ALL-USERS <USER> <PASSWD>
CREATE-ROOM <USER> <PASSWD> <ROOM>
LIST-ROOMS <USER> <PASSWD>
ENTER-ROOM <USER> <PASSWD> <ROOM>
LEAVE-ROOM <USER> <PASSWD>
SEND-MESSAGE <USER> <PASSWD> <MESSAGE> <ROOM>
GET-USERS-IN-ROOM <USER> <PASSWD> <ROOM>
GET-MESSAGES <USER> <PASSWD> <LAST-MESSAGE-NUM> 
