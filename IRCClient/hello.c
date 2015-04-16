
#include <gtk/gtk.h>
#include <time.h>
//#include <curses.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>

#define MAX_RESPONSE (10 * 1024)

char * user;
char * password;
char * host;
char * sport;
int port;
char response[MAX_RESPONSE];
static char curuser[1000];
static char curpass[1000];
static char curroom[1000];
static int lastMessage;
GtkTextBuffer *buffer1;
GtkTextBuffer *buffer2;
GtkTextBuffer *buffer3;
GtkTextBuffer *buffer4;
//char command[1000];

int open_client_socket(char * host, int port) {
	// Initialize socket address structure
	struct  sockaddr_in socketAddress;
	
	// Clear sockaddr structure
	memset((char *)&socketAddress,0,sizeof(socketAddress));
	
	// Set family to Internet 
	socketAddress.sin_family = AF_INET;
	
	// Set port
	socketAddress.sin_port = htons((u_short)port);
	
	// Get host table entry for this host
	struct  hostent  *ptrh = gethostbyname(host);
	if ( ptrh == NULL ) {
		perror("gethostbyname");
		exit(1);
	}
	
	// Copy the host ip address to socket address structure
	memcpy(&socketAddress.sin_addr, ptrh->h_addr, ptrh->h_length);
	
	// Get TCP transport protocol entry
	struct  protoent *ptrp = getprotobyname("tcp");
	if ( ptrp == NULL ) {
		perror("getprotobyname");
		exit(1);
	}
	
	// Create a tcp socket
	int sock = socket(PF_INET, SOCK_STREAM, ptrp->p_proto);
	if (sock < 0) {
		perror("socket");
		exit(1);
	}
	
	// Connect the socket to the specified server
	if (connect(sock, (struct sockaddr *)&socketAddress,
		    sizeof(socketAddress)) < 0) {
		perror("connect");
		exit(1);
	}
	
	return sock;
}


int sendCommand(char *  host, int port, char * command, char * response) {
	
	int sock = open_client_socket( host, port);

	if (sock<0) {
		return 0;
	}

	// Send command
	write(sock, command, strlen(command));
	write(sock, "\r\n",2);

	//Print copy to stdout
	write(1, command, strlen(command));
	write(1, "\r\n",2);

	// Keep reading until connection is closed or MAX_REPONSE
	int n = 0;
	int len = 0;
	while ((n=read(sock, response+len, MAX_RESPONSE - len))>0) {
		len += n;
	}
	response[len]=0;

	printf("response:\n%s\n", response);

	close(sock);

	return 1;
}
	
void
printUsage()
{
	printf("Usage: client <host> <port>\n");
	exit(1);
}

//------------------GTK FUNCTIONS------------------------------


static gboolean delete_event( GtkWidget *widget,
                              GdkEvent  *event,
                              gpointer   data )
{
    g_print ("delete event occurred\n");
    return FALSE;
}

/* Another callback */
static void destroy( GtkWidget *widget,
                     gpointer   data )
{
    gtk_main_quit ();
}


//--------------PANNED Methods------------------------
static GtkWidget *create_list1( void )
{

    GtkWidget *scrolled_window;
    GtkWidget *tree_view;
    GtkListStore *model;
    GtkTreeIter iter;
    GtkCellRenderer *cell;
    GtkTreeViewColumn *column;

    int i;
   
    /* Create a new scrolled window, with scrollbars only if needed */
    scrolled_window = gtk_scrolled_window_new (NULL, NULL);
    gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
				    GTK_POLICY_AUTOMATIC, 
				    GTK_POLICY_AUTOMATIC);
   
    model = gtk_list_store_new (1, G_TYPE_STRING);
    tree_view = gtk_tree_view_new ();
    gtk_container_add (GTK_CONTAINER (scrolled_window), tree_view);
    gtk_tree_view_set_model (GTK_TREE_VIEW (tree_view), GTK_TREE_MODEL (model));
    gtk_widget_show (tree_view);
   
    /* Add some messages to the window */
    for (i = 0; i < 10; i++) {
        gchar *msg = g_strdup_printf ("Room #%d", i);
        gtk_list_store_append (GTK_LIST_STORE (model), &iter);
        gtk_list_store_set (GTK_LIST_STORE (model), &iter,0, msg,-1);

	                    
	g_free (msg);
    }
   
    cell = gtk_cell_renderer_text_new ();

    column = gtk_tree_view_column_new_with_attributes ("Rooms",
                                                       cell,
                                                       "text", 0,
                                                       NULL);
  
    gtk_tree_view_append_column (GTK_TREE_VIEW (tree_view),
	  		         GTK_TREE_VIEW_COLUMN (column));

    return scrolled_window;
}


//----------ENTRY Functions (Built In)-------------------
static void enter_callback( GtkWidget *widget,
                            GtkWidget *entry )
{
  const gchar *entry_text;
  entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
  //printf ("Entry contents: %s\n", entry_text);
}

static void entry_toggle_editable( GtkWidget *checkbutton,
                                   GtkWidget *entry )
{
  gtk_editable_set_editable (GTK_EDITABLE (entry),
                             GTK_TOGGLE_BUTTON (checkbutton)->active);
}

static void entry_toggle_visibility( GtkWidget *checkbutton,
                                     GtkWidget *entry )
{
  gtk_entry_set_visibility (GTK_ENTRY (entry),
			    GTK_TOGGLE_BUTTON (checkbutton)->active);
}


//----------------------------------------------
   
/* Add some text to our text widget - this is a callback that is invoked
when our window is realized. We could also force our window to be
realized with gtk_widget_realize, but it would have to be part of
a hierarchy first */

static void insert_text1( char * str )
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer1, &iter, 0);

   gtk_text_buffer_insert (buffer1, &iter, str, -1);
}

static void insert_text2( char * str )
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer2, &iter, 0);

   gtk_text_buffer_insert (buffer2, &iter, str, -1);
}

static void insert_text3( char * str ) //Rooms
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer3, &iter, 0);

   gtk_text_buffer_insert (buffer3, &iter, str, -1);
}

static void insert_text4( char * str )  //Users
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer4, &iter, 0);

   gtk_text_buffer_insert (buffer4, &iter, str, -1);
}
   
/* Create a scrolled text area that displays a "message" */
static GtkWidget *create_text1( void )
{
   GtkWidget *scrolled_window;
   GtkWidget *view;
   

   view = gtk_text_view_new ();
   buffer1 = gtk_text_view_get_buffer (GTK_TEXT_VIEW (view));

   scrolled_window = gtk_scrolled_window_new (NULL, NULL);
   gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
		   	           GTK_POLICY_AUTOMATIC,
				   GTK_POLICY_AUTOMATIC);

   gtk_container_add (GTK_CONTAINER (scrolled_window), view);
   //insert_text1 (buffer1);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
}

static GtkWidget *create_text2( void )
{
   GtkWidget *scrolled_window;
   GtkWidget *view;

   view = gtk_text_view_new ();
   buffer2 = gtk_text_view_get_buffer (GTK_TEXT_VIEW (view));

   scrolled_window = gtk_scrolled_window_new (NULL, NULL);
   gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
		   	           GTK_POLICY_AUTOMATIC,
				   GTK_POLICY_AUTOMATIC);

   gtk_container_add (GTK_CONTAINER (scrolled_window), view);
   //insert_text2 (buffer2);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
}

static GtkWidget *create_text3( void )
{
   GtkWidget *scrolled_window;
   GtkWidget *view;
   

   view = gtk_text_view_new ();
   buffer3 = gtk_text_view_get_buffer (GTK_TEXT_VIEW (view));

   scrolled_window = gtk_scrolled_window_new (NULL, NULL);
   gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
		   	           GTK_POLICY_AUTOMATIC,
				   GTK_POLICY_AUTOMATIC);

   gtk_container_add (GTK_CONTAINER (scrolled_window), view);
   //insert_text3 (buffer3);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
}

static GtkWidget *create_text4( void )
{
   GtkWidget *scrolled_window;
   GtkWidget *view;
   

   view = gtk_text_view_new ();
   buffer4 = gtk_text_view_get_buffer (GTK_TEXT_VIEW (view));

   scrolled_window = gtk_scrolled_window_new (NULL, NULL);
   gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
		   	           GTK_POLICY_AUTOMATIC,
				   GTK_POLICY_AUTOMATIC);

   gtk_container_add (GTK_CONTAINER (scrolled_window), view);
   //insert_text4 (buffer4);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
}


//-------------CUSTOM Send to server functions-----------

static void send_message( GtkWidget *widget,
                            GtkWidget *entry ){
					   const gchar *entry_text;
		entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
  
  char * command = (char*) malloc(1000);
  sprintf(command,"SEND-MESSAGE %s %s %s %s",curuser,curpass,curroom, entry_text);
	sendCommand(host, port, command, response);
	if(strcmp(response,"OK\r\n")==0){
	printf ("Sent: %s\n", entry_text);
	}
	else{
		printf("%s",response);
		insert_text2(response);
	}
				   }
				   
static void create_room( GtkWidget *widget,
                            GtkWidget *entry ){
								const gchar *entry_text;
     entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
	 char * command = (char*) malloc(1000);
	 sprintf(command,"CREATE-ROOM %s %s %s",curuser,curpass,entry_text);
	sendCommand(host, port, command, response);
	printf ("Create Room: %s\n", entry_text);
	//Possibly remove-----------
	sprintf(command,"ENTER-ROOM %s %s %s",curuser,curpass,entry_text);
	sendCommand(host, port, command, response);
	if(strcmp(response,"OK\r\n")==0){
	strcpy(curroom,entry_text);
	//-------------------------------
  	printf ("Enter Room: %s\n", entry_text);
	//Add to ROOM list
	sprintf(command,"%s\n",entry_text);
	insert_text3(command);
	}
	else{
		printf("%s",response);
		insert_text2(response);
	}
	
	 }
				   
static void enter_room( GtkWidget *widget,
                            GtkWidget *entry ){
								const gchar *entry_text;
     entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
	 char * command = (char*) malloc(1000);
	sprintf(command,"ENTER-ROOM %s %s %s",curuser,curpass,entry_text);
	sendCommand(host, port, command, response);
	if(strcmp(response,"OK\r\n")==0){
	strcpy(curroom,entry_text);
  printf ("Enter Room: %s\n", entry_text);
sprintf(command,"GET-USERS-IN-ROOM %s %s %s",curuser,curpass,entry_text);
	sendCommand(host, port, command, response);  
	insert_text4(response);
	}
	else{
		printf("%s",response);
		insert_text2(response);
	}
				   }
				   
static void leave_room( GtkWidget *widget,
                            GtkWidget *entry ){
								const gchar *entry_text;
     entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
	 char * command = (char*) malloc(1000);
	 sprintf(command,"LEAVE-ROOM %s %s %s",curuser,curpass,entry_text);
	sendCommand(host, port, command, response);
	if(strcmp(response,"OK\r\n")==0){
	strcpy(curroom,"");
  printf ("Left room\n", entry_text);	
  }
	else{
		printf("%s",response);
		insert_text2(response);
	}
				   }
				   
static void create_account( GtkWidget *widget,
                            GtkWidget *entry ){
								char * command = (char*) malloc(1000);
								char user[100];
								char pass[100];
							//GtkWidget *ent = g_object_get_data (context_object, "entry");
							//GtkWidget *ent2 = g_object_get_data (context_object, "entry2");							
								
								
								const char *entry_text  = gtk_entry_get_text (GTK_ENTRY (entry));
								//const char *entry_text2 = gtk_entry_get_text (GTK_ENTRY (ent2));
									strcat(entry_text,"_");
									int x = 0;
									int y = 0;
								while(entry_text[y] != '_'){
									user[x] = entry_text[y];
									x++;
									y++;
								}
								user[x] = '\0';
								x=0;
								y++;
								while(entry_text[y] != '_'){
									pass[x] = entry_text[y];
									x++;
									y++;
								}
								pass[x] = '\0';
								x=0;
								strcpy(curuser,user);
								strcpy(curpass,pass);
					sprintf(command,"ADD-USER %s %s",user,pass);
				sendCommand(host, port, command, response);	
				if(strcmp(response,"OK\r\n")==0){
					   printf ("Create Account: %s %s\n", user,pass);
						 }
								else{
								printf("%s",response);
								insert_text2(response);
								}
				   }
				   
static void login( GtkWidget *widget,
                            GtkWidget *entry ){
								char * command = (char*) malloc(1000);
								char user[100];
								char pass[100];
							//GtkWidget *ent = g_object_get_data (context_object, "entry");
							//GtkWidget *ent2 = g_object_get_data (context_object, "entry2");							
								
								
								const char *entry_text  = gtk_entry_get_text (GTK_ENTRY (entry));
								//const char *entry_text2 = gtk_entry_get_text (GTK_ENTRY (ent2));
									strcat(entry_text,"_");
									int x = 0;
									int y = 0;
								while(entry_text[y] != '_'){
									user[x] = entry_text[y];
									x++;
									y++;
								}
								user[x] = '\0';
								x=0;
								y++;
								while(entry_text[y] != '_'){
									pass[x] = entry_text[y];
									x++;
									y++;
								}
								pass[x] = '\0';
								x=0;
								strcpy(curuser,user);
								strcpy(curpass,pass);
							
					   printf ("Login Successful\n");	
				   }
				   
static void logout( GtkWidget *widget,
                            GtkWidget *entry ){
							
								strcpy(curuser,"");
								strcpy(curpass,"");
							
					   printf ("Logout Successful \n");	
				   }

//--------------CUSTOM Window functions-------------------

static void create_room_window( GtkWidget *widget,
                   gpointer   data ){

	GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
	gint tmp_pos;
	window1 = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_widget_set_size_request (GTK_WIDGET (window1), 200, 100);
    gtk_window_set_title (GTK_WINDOW (window1), "Create Room");
    g_signal_connect (window1, "destroy",
                      G_CALLBACK (gtk_widget_destroy), NULL);
					  
    g_signal_connect_swapped (window1, "delete-event",
                              G_CALLBACK (gtk_widget_destroy), 
                              window1);
							  vbox = gtk_vbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (window1), vbox);
    gtk_widget_show (vbox);

    entry = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry), 50);
    g_signal_connect (entry, "activate",
		      G_CALLBACK (enter_callback),
		      entry);
    gtk_entry_set_text (GTK_ENTRY (entry), "");
    tmp_pos = GTK_ENTRY (entry)->text_length;
   
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);

    hbox = gtk_hbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (vbox), hbox);
    gtk_widget_show (hbox);
                                  
    
                                   
    button = gtk_button_new_with_label ("Create");
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (create_room), entry);
				  
    gtk_box_pack_start (GTK_BOX (vbox), button, TRUE, TRUE, 0);
    gtk_widget_set_can_default (button, TRUE);
    gtk_widget_grab_default (button);
    gtk_widget_show (button);
    gtk_widget_show (window1);			   
}

static void enter_room_window( GtkWidget *widget,
                   gpointer   data ){

	GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
	gint tmp_pos;
	window1 = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_widget_set_size_request (GTK_WIDGET (window1), 200, 100);
    gtk_window_set_title (GTK_WINDOW (window1), "Enter Room");
    g_signal_connect (window1, "destroy",
                      G_CALLBACK (gtk_widget_destroy), NULL);
					  
    g_signal_connect_swapped (window1, "delete-event",
                              G_CALLBACK (gtk_widget_destroy), 
                              window1);
							  vbox = gtk_vbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (window1), vbox);
    gtk_widget_show (vbox);

    entry = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry), 50);
    g_signal_connect (entry, "activate",
		      G_CALLBACK (enter_callback),
		      entry);
    gtk_entry_set_text (GTK_ENTRY (entry), "");
    tmp_pos = GTK_ENTRY (entry)->text_length;
   
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);

    hbox = gtk_hbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (vbox), hbox);
    gtk_widget_show (hbox);
                                  
    
                                   
    button = gtk_button_new_with_label ("OK");
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (enter_room), entry);
				  
    gtk_box_pack_start (GTK_BOX (vbox), button, TRUE, TRUE, 0);
    gtk_widget_set_can_default (button, TRUE);
    gtk_widget_grab_default (button);
    gtk_widget_show (button);
    gtk_widget_show (window1);			   
}

static void leave_room_window( GtkWidget *widget,
                   gpointer   data ){

	GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
	gint tmp_pos;
	window1 = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_widget_set_size_request (GTK_WIDGET (window1), 200, 100);
    gtk_window_set_title (GTK_WINDOW (window1), "Leave Room");
    g_signal_connect (window1, "destroy",
                      G_CALLBACK (gtk_widget_destroy), NULL);
					  
    g_signal_connect_swapped (window1, "delete-event",
                              G_CALLBACK (gtk_widget_destroy), 
                              window1);
							  vbox = gtk_vbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (window1), vbox);
    gtk_widget_show (vbox);

    entry = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry), 50);
    g_signal_connect (entry, "activate",
		      G_CALLBACK (enter_callback),
		      entry);
    gtk_entry_set_text (GTK_ENTRY (entry), "");
    tmp_pos = GTK_ENTRY (entry)->text_length;
   
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);

    hbox = gtk_hbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (vbox), hbox);
    gtk_widget_show (hbox);
                                  
    
                                   
    button = gtk_button_new_with_label ("OK");
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (leave_room), entry);
				  
    gtk_box_pack_start (GTK_BOX (vbox), button, TRUE, TRUE, 0);
    gtk_widget_set_can_default (button, TRUE);
    gtk_widget_grab_default (button);
    gtk_widget_show (button);
    gtk_widget_show (window1);			   
}

static void create_account_window( GtkWidget *widget,
                   gpointer   data ){
GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
    GtkWidget *check;
	GObject *context_object = (GObject *) malloc(1000);
	gint tmp_pos;
	window1 = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_widget_set_size_request (GTK_WIDGET (window1), 200, 100);
    gtk_window_set_title (GTK_WINDOW (window1), "Create Account");
    g_signal_connect (window1, "destroy",
                      G_CALLBACK (gtk_widget_destroy), NULL);
					  
    g_signal_connect_swapped (window1, "delete-event",
                              G_CALLBACK (gtk_widget_destroy), 
                              window1);
							  vbox = gtk_vbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (window1), vbox);
    gtk_widget_show (vbox);

	//USER Entry
    entry = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry), 50);
    g_signal_connect (entry, "activate",
		      G_CALLBACK (enter_callback),
		      entry);
    gtk_entry_set_text (GTK_ENTRY (entry), "Username_Password");
    tmp_pos = GTK_ENTRY (entry)->text_length;
    gtk_editable_insert_text (GTK_EDITABLE (entry), " ", -1, &tmp_pos);
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);
	
	

    hbox = gtk_hbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (vbox), hbox);
    gtk_widget_show (hbox);
                                  
    check = gtk_check_button_new_with_label ("Editable");
    gtk_box_pack_start (GTK_BOX (hbox), check, TRUE, TRUE, 0);
    g_signal_connect (check, "toggled",
	              G_CALLBACK (entry_toggle_editable), entry);
    gtk_toggle_button_set_active (GTK_TOGGLE_BUTTON (check), TRUE);
    gtk_widget_show (check);
    
    check = gtk_check_button_new_with_label ("Visible");
    gtk_box_pack_start (GTK_BOX (hbox), check, TRUE, TRUE, 0);
    g_signal_connect (check, "toggled",
	              G_CALLBACK (entry_toggle_visibility), entry);
    gtk_toggle_button_set_active (GTK_TOGGLE_BUTTON (check), TRUE);
    gtk_widget_show (check);
                                   
    button = gtk_button_new_with_label ("OK");

	
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (create_account), entry);
				  
    gtk_box_pack_start (GTK_BOX (vbox), button, TRUE, TRUE, 0);
    gtk_widget_set_can_default (button, TRUE);
    gtk_widget_grab_default (button);
    gtk_widget_show (button);
    
    gtk_widget_show (window1);
				   }
				   
	static void login_window( GtkWidget *widget,
                   gpointer   data ){
GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
    GtkWidget *check;
	GObject *context_object = (GObject *) malloc(1000);
	gint tmp_pos;
	window1 = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_widget_set_size_request (GTK_WIDGET (window1), 200, 100);
    gtk_window_set_title (GTK_WINDOW (window1), "Login");
    g_signal_connect (window1, "destroy",
                      G_CALLBACK (gtk_widget_destroy), NULL);
					  
    g_signal_connect_swapped (window1, "delete-event",
                              G_CALLBACK (gtk_widget_destroy), 
                              window1);
							  vbox = gtk_vbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (window1), vbox);
    gtk_widget_show (vbox);

	//USER Entry
    entry = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry), 50);
    g_signal_connect (entry, "activate",
		      G_CALLBACK (enter_callback),
		      entry);
    gtk_entry_set_text (GTK_ENTRY (entry), "Username_Password");
    tmp_pos = GTK_ENTRY (entry)->text_length;
    gtk_editable_insert_text (GTK_EDITABLE (entry), " ", -1, &tmp_pos);
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);
	
	

    hbox = gtk_hbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (vbox), hbox);
    gtk_widget_show (hbox);
                                  
    check = gtk_check_button_new_with_label ("Editable");
    gtk_box_pack_start (GTK_BOX (hbox), check, TRUE, TRUE, 0);
    g_signal_connect (check, "toggled",
	              G_CALLBACK (entry_toggle_editable), entry);
    gtk_toggle_button_set_active (GTK_TOGGLE_BUTTON (check), TRUE);
    gtk_widget_show (check);
    
    check = gtk_check_button_new_with_label ("Visible");
    gtk_box_pack_start (GTK_BOX (hbox), check, TRUE, TRUE, 0);
    g_signal_connect (check, "toggled",
	              G_CALLBACK (entry_toggle_visibility), entry);
    gtk_toggle_button_set_active (GTK_TOGGLE_BUTTON (check), TRUE);
    gtk_widget_show (check);
                                   
    button = gtk_button_new_with_label ("OK");

	
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (login), entry);
				  
    gtk_box_pack_start (GTK_BOX (vbox), button, TRUE, TRUE, 0);
    gtk_widget_set_can_default (button, TRUE);
    gtk_widget_grab_default (button);
    gtk_widget_show (button);
    
    gtk_widget_show (window1);
				   }
			   

					   
static void send_message_window( GtkWidget *widget,
                   gpointer   data ){
	fprintf(stderr,"Send Message");
	   
	GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
    GtkWidget *check;
	gint tmp_pos;
	window1 = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_widget_set_size_request (GTK_WIDGET (window1), 400, 100);
    gtk_window_set_title (GTK_WINDOW (window1), "Send Message");
    g_signal_connect (window1, "destroy",
                      G_CALLBACK (gtk_widget_destroy), NULL);
					  
    g_signal_connect_swapped (window1, "delete-event",
                              G_CALLBACK (gtk_widget_destroy), 
                              window1);
							  vbox = gtk_vbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (window1), vbox);
    gtk_widget_show (vbox);

    entry = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry), 50);
    g_signal_connect (entry, "activate",
		      G_CALLBACK (enter_callback),
		      entry);
    gtk_entry_set_text (GTK_ENTRY (entry), "hello");
    tmp_pos = GTK_ENTRY (entry)->text_length;
    gtk_editable_insert_text (GTK_EDITABLE (entry), " world", -1, &tmp_pos);
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);

    hbox = gtk_hbox_new (FALSE, 0);
    gtk_container_add (GTK_CONTAINER (vbox), hbox);
    gtk_widget_show (hbox);
                                  
    check = gtk_check_button_new_with_label ("Editable");
    gtk_box_pack_start (GTK_BOX (hbox), check, TRUE, TRUE, 0);
    g_signal_connect (check, "toggled",
	              G_CALLBACK (entry_toggle_editable), entry);
    gtk_toggle_button_set_active (GTK_TOGGLE_BUTTON (check), TRUE);
    gtk_widget_show (check);
    
    check = gtk_check_button_new_with_label ("Visible");
    gtk_box_pack_start (GTK_BOX (hbox), check, TRUE, TRUE, 0);
    g_signal_connect (check, "toggled",
	              G_CALLBACK (entry_toggle_visibility), entry);
    gtk_toggle_button_set_active (GTK_TOGGLE_BUTTON (check), TRUE);
    gtk_widget_show (check);
                                   
    button = gtk_button_new_with_label ("Send");
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (send_message), entry);
				  
    gtk_box_pack_start (GTK_BOX (vbox), button, TRUE, TRUE, 0);
    gtk_widget_set_can_default (button, TRUE);
    gtk_widget_grab_default (button);
    gtk_widget_show (button);
    
    gtk_widget_show (window1);
}

//-----------------GET Messages Thread-------------------



static gboolean
update_messages(GtkWidget *widget)
{
	char * command = (char*) malloc(1000);
	char * msgresponse = (char*) malloc(1000);
  //fprintf(stderr,"LOOP");
  if((strlen(curuser)>1)&&(strlen(curroom)>1)){
	  sprintf(command,"GET-MESSAGES %s %s %d %s",curuser,curpass,lastMessage, curroom);
	sendCommand(host, port, command, msgresponse);
	if(strcmp(msgresponse,"NO-NEW-MESSAGES\r\n")!=0){
	insert_text1 (msgresponse);
	lastMessage++;
	}
	  
  }
  return TRUE;
}










int
main(int argc, char **argv) {

	
	if (argc < 3) {
		printUsage();
	}

	host = argv[1];
	sport = argv[2];
	//command = argv[3];

	sscanf(sport, "%d", &port);
	
	
	
	
	//-------------------GTK METHODS--------------------------
	
	
	/* GtkWidget is the storage type for widgets */
    GtkWidget *window;
    GtkWidget *button_send;
	GtkWidget *button_new_room;
	GtkWidget *button_new_account;
	GtkWidget *button_exit;
	GtkWidget *button_enter;
	GtkWidget *button_leave;
	GtkWidget *button_login;
	GtkWidget *button_logout;
	GtkWidget *vert;
    GtkWidget *horiz1;
	GtkWidget *horiz2;
	GtkWidget *horiz3;
	GtkWidget *hpaned;
	GtkWidget *vpaned;
	GtkWidget *list;
    GtkWidget *text;
	GtkWidget *text2;
	GtkWidget *text3;
	GtkWidget *text4;
	GtkWidget *label1;
	GtkWidget *label2;
	

	
    GtkWidget *separator;
    GtkWidget *label;
    
    /* This is called in all GTK applications. Arguments are parsed
     * from the command line and are returned to the application. */
    gtk_init (&argc, &argv);
    
    /* create a new window */
    window = gtk_window_new (GTK_WINDOW_TOPLEVEL);
	
    /* When the window is given the "delete-event" signal (this is given
     * by the window manager, usually by the "close" option, or on the
     * titlebar), we ask it to call the delete_event () function
     * as defined above. The data passed to the callback
     * function is NULL and is ignored in the callback function. */
    g_signal_connect (window, "delete-event",
		      G_CALLBACK (delete_event), NULL);
    
    /* Here we connect the "destroy" event to a signal handler.  
     * This event occurs when we call gtk_widget_destroy() on the window,
     * or if we return FALSE in the "delete-event" callback. */
    g_signal_connect (window, "destroy",
		      G_CALLBACK (destroy), NULL);
    
    /* Sets the border width of the window. */
    gtk_container_set_border_width (GTK_CONTAINER (window), 10);
	
	
    
    /* Creates a new button with the label "Hello World". */
    button_send = gtk_button_new_with_label ("Send");
	button_new_room = gtk_button_new_with_label ("Create Room");
	button_new_account = gtk_button_new_with_label ("Create Account");
	button_exit = gtk_button_new_with_label ("Exit");
	button_enter = gtk_button_new_with_label ("Enter Room");
	button_leave = gtk_button_new_with_label ("Leave Room");
	button_login = gtk_button_new_with_label ("Login");
	button_logout = gtk_button_new_with_label ("Logout");
	hpaned = gtk_hpaned_new ();
	vpaned = gtk_vpaned_new ();
	
	//ADD to hpaned PANEL
	text3 = create_text3 ();
    gtk_paned_add1 (GTK_PANED (hpaned), text3);

   
	text4 = create_text4 ();
    gtk_paned_add2 (GTK_PANED (hpaned), text4);
	
	//ADD to vpand PANEL
	
	text = create_text1 ();
    gtk_paned_add1 (GTK_PANED (vpaned), text);
   
    text2 = create_text2 ();
    gtk_paned_add2 (GTK_PANED (vpaned), text2);
	
	


	//Create LABELS
	label1 = gtk_label_new("Rooms");
	label2 = gtk_label_new("Users");
   

	
    //Set the Widget sizes
	gtk_widget_set_size_request (GTK_WIDGET (window), 550, 600);
	
	
    /* When the button receives the "clicked" signal, it will call the
     * function hello() passing it NULL as its argument.  The hello()
     * function is defined above. */

    g_signal_connect (button_send, "clicked",
		      G_CALLBACK (send_message_window), NULL);
			  
	g_signal_connect (button_new_room, "clicked",
		      G_CALLBACK (create_room_window), NULL);
			  
	g_signal_connect (button_enter, "clicked",
		      G_CALLBACK (enter_room_window), NULL);
			  
	g_signal_connect (button_leave, "clicked",
		      G_CALLBACK (leave_room_window), NULL);
			  
	g_signal_connect (button_login, "clicked",
		      G_CALLBACK (login_window), NULL);
			  
	g_signal_connect (button_logout, "clicked",
		      G_CALLBACK (logout), NULL);
			  
	g_signal_connect (button_new_account, "clicked",
		      G_CALLBACK (create_account_window), NULL);
	

    
    /* This will cause the window to be destroyed by calling
     * gtk_widget_destroy(window) when "clicked".  Again, the destroy
     * signal could come from here, or the window manager. */
    g_signal_connect_swapped (button_exit, "clicked",
			      G_CALLBACK (gtk_widget_destroy),
                              window);
							  
							 
	
	//-------------BOXES------------------------
	vert = gtk_vbox_new (FALSE, 0);
	horiz1 = gtk_hbox_new (FALSE, 0);
	horiz2 = gtk_hbox_new (FALSE, 0);
	
	gtk_box_pack_start (GTK_BOX (horiz1), label1, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz1), label2, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_send, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_new_room, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_enter, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_leave, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_login, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_logout, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_new_account, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button_exit, TRUE, FALSE, 0);
	gtk_container_add (GTK_CONTAINER (vert), horiz1);
	gtk_container_add (GTK_CONTAINER (vert), hpaned);

	gtk_box_pack_start (GTK_BOX (vert), vpaned, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (vert), horiz2, TRUE, FALSE, 0);
	

	gtk_container_add (GTK_CONTAINER (window), vert);
    
    /* The final step is to display the widgets. */
	gtk_widget_show (vert);
	gtk_widget_show (horiz1);
	gtk_widget_show (horiz2);
    gtk_widget_show (button_send);
	gtk_widget_show (button_new_room);
	gtk_widget_show (button_new_account);
	gtk_widget_show (button_enter); 
	gtk_widget_show (button_leave); 
	gtk_widget_show (button_login);
	gtk_widget_show (button_logout);
	gtk_widget_show (button_exit);
	gtk_widget_show (label1);
	gtk_widget_show (label2);
    gtk_widget_show (window);
	gtk_widget_show (hpaned);
	gtk_widget_show (vpaned);
	gtk_widget_show (text);
	gtk_widget_show (text2);
	gtk_widget_show (text3);
	gtk_widget_show (text4);
	
    
    /* All GTK applications must have a gtk_main(). Control ends here
     * and waits for an event to occur (like a key press or
     * mouse event). */
	 lastMessage = 0;
	 
	g_timeout_add(5000, (GSourceFunc) update_messages, (gpointer) window);
	 
    gtk_main ();
	

	return 0;
}
