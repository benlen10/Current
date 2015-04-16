
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
char command[1000];

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
        gtk_list_store_set (GTK_LIST_STORE (model), 
	                    &iter,
                            0, msg,
	                    -1);
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

static GtkWidget *create_list2( void )
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
        gchar *msg = g_strdup_printf ("User #%d", i);
        gtk_list_store_append (GTK_LIST_STORE (model), &iter);
        gtk_list_store_set (GTK_LIST_STORE (model), 
	                    &iter,
                            0, msg,
	                    -1);
	g_free (msg);
    }
   
    cell = gtk_cell_renderer_text_new ();

    column = gtk_tree_view_column_new_with_attributes ("Users",
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

static void insert_text1( GtkTextBuffer *buffer )
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer, &iter, 0);

   gtk_text_buffer_insert (buffer, &iter,   
    "Mary: Hi Everybody!\n"
	"Superman: Hello there."
    , -1);
}

static void insert_text2( GtkTextBuffer *buffer )
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer, &iter, 0);

   gtk_text_buffer_insert (buffer, &iter,   
    "Type a message:"
    , -1);
}
   
/* Create a scrolled text area that displays a "message" */
static GtkWidget *create_text1( void )
{
   GtkWidget *scrolled_window;
   GtkWidget *view;
   GtkTextBuffer *buffer;

   view = gtk_text_view_new ();
   buffer = gtk_text_view_get_buffer (GTK_TEXT_VIEW (view));

   scrolled_window = gtk_scrolled_window_new (NULL, NULL);
   gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
		   	           GTK_POLICY_AUTOMATIC,
				   GTK_POLICY_AUTOMATIC);

   gtk_container_add (GTK_CONTAINER (scrolled_window), view);
   insert_text1 (buffer);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
}

static GtkWidget *create_text2( void )
{
   GtkWidget *scrolled_window;
   GtkWidget *view;
   GtkTextBuffer *buffer;

   view = gtk_text_view_new ();
   buffer = gtk_text_view_get_buffer (GTK_TEXT_VIEW (view));

   scrolled_window = gtk_scrolled_window_new (NULL, NULL);
   gtk_scrolled_window_set_policy (GTK_SCROLLED_WINDOW (scrolled_window),
		   	           GTK_POLICY_AUTOMATIC,
				   GTK_POLICY_AUTOMATIC);

   gtk_container_add (GTK_CONTAINER (scrolled_window), view);
   insert_text2 (buffer);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
}

//-------------CUSTOM Send to server functions-----------

static void send_message( GtkWidget *widget,
                            GtkWidget *entry ){
					   const gchar *entry_text;
		entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
  printf ("Sent: %s\n", entry_text);	
				   }
				   
static void create_room( GtkWidget *widget,
                            GtkWidget *entry ){
								const gchar *entry_text;
     entry_text = gtk_entry_get_text (GTK_ENTRY (entry));
  printf ("Create Room: %s\n", entry_text);	
				   }
				   
static void create_account( GtkWidget *widget,
                            GObject *context_object ){
							GtkWidget *ent = g_object_get_data (context_object, "entry");
							GtkWidget *ent2 = g_object_get_data (context_object, "entry2");							
								
								
								const char *entry_text  = gtk_entry_get_text (GTK_ENTRY (ent));
								const char *entry_text2 = gtk_entry_get_text (GTK_ENTRY (ent2));
					   printf ("Create Account: %s %s\n", entry_text,entry_text2);	
				   }

//--------------CUSTOM Window functions-------------------

static void create_room_window( GtkWidget *widget,
                   gpointer   data ){

	GtkWidget *window1;
	GtkWidget *vbox, *hbox;
    GtkWidget *entry;
    GtkWidget *button;
    GtkWidget *check;
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
                                   
    button = gtk_button_new_with_label ("Create");
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (create_room), entry);
				  
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
	GtkWidget *entry2;
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
    gtk_entry_set_text (GTK_ENTRY (entry), "Username");
    tmp_pos = GTK_ENTRY (entry)->text_length;
    gtk_editable_insert_text (GTK_EDITABLE (entry), " ", -1, &tmp_pos);
    gtk_editable_select_region (GTK_EDITABLE (entry),
			        0, GTK_ENTRY (entry)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry, TRUE, TRUE, 0);
    gtk_widget_show (entry);
	
	//PASS Entry
	entry2 = gtk_entry_new ();
    gtk_entry_set_max_length (GTK_ENTRY (entry2), 50);
    g_signal_connect (entry2, "activate",
		      G_CALLBACK (enter_callback),
		      entry2);
    gtk_entry_set_text (GTK_ENTRY (entry2), "Password");
    tmp_pos = GTK_ENTRY (entry2)->text_length;
    gtk_editable_insert_text (GTK_EDITABLE (entry2), " ", -1, &tmp_pos);
    gtk_editable_select_region (GTK_EDITABLE (entry2),
			        0, GTK_ENTRY (entry2)->text_length);
    gtk_box_pack_start (GTK_BOX (vbox), entry2, TRUE, TRUE, 0);
    gtk_widget_show (entry2);

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
                                   
    button = gtk_button_new_with_label ("Create");
	fprintf(stderr,"LOC1");
	g_object_set_data (context_object, "entry", entry);
	g_object_set_data (context_object, "entry2", entry2);
fprintf(stderr,"LOC1");
	
    gtk_signal_connect (GTK_OBJECT (button), "clicked",
                        GTK_SIGNAL_FUNC (create_account), context_object);
				  
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
    gtk_widget_set_size_request (GTK_WIDGET (window1), 200, 100);
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
    GtkWidget *button1;
	GtkWidget *button2;
	GtkWidget *button3;
	GtkWidget *button4;
	GtkWidget *vert;
    GtkWidget *horiz1;
	GtkWidget *horiz2;
	GtkWidget *horiz3;
	GtkWidget *hpaned;
	GtkWidget *vpaned;
	GtkWidget *list;
	GtkWidget *list2;
    GtkWidget *text;
	GtkWidget *text2;
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
    button1 = gtk_button_new_with_label ("Send");
	button2 = gtk_button_new_with_label ("Create Room");
	button3 = gtk_button_new_with_label ("Create Account");
	button4 = gtk_button_new_with_label ("Exit");
	hpaned = gtk_hpaned_new ();
	vpaned = gtk_vpaned_new ();
	
	//ADD to hpaned PANEL
	list = create_list1 ();
    gtk_paned_add1 (GTK_PANED (hpaned), list);
    gtk_widget_show (list);
   
	list2 = create_list2 ();
    gtk_paned_add2 (GTK_PANED (hpaned), list2);
    gtk_widget_show (list2);
	
	//ADD to vpand PANEL
	
	text = create_text1 ();
    gtk_paned_add1 (GTK_PANED (vpaned), text);
    gtk_widget_show (text);
   
    text2 = create_text2 ();
    gtk_paned_add2 (GTK_PANED (vpaned), text2);
    gtk_widget_show (text2);
	
	//Create LABELS
	label1 = gtk_label_new("Rooms");
	label2 = gtk_label_new("Users");
   

	
    //Set the Widget sizes
	gtk_widget_set_size_request (GTK_WIDGET (window), 450, 500);
	
	
    /* When the button receives the "clicked" signal, it will call the
     * function hello() passing it NULL as its argument.  The hello()
     * function is defined above. */

    g_signal_connect (button1, "clicked",
		      G_CALLBACK (send_message_window), NULL);
			  
	g_signal_connect (button2, "clicked",
		      G_CALLBACK (create_room_window), NULL);
			  
	g_signal_connect (button3, "clicked",
		      G_CALLBACK (create_account_window), NULL);
    
    /* This will cause the window to be destroyed by calling
     * gtk_widget_destroy(window) when "clicked".  Again, the destroy
     * signal could come from here, or the window manager. */
    g_signal_connect_swapped (button4, "clicked",
			      G_CALLBACK (gtk_widget_destroy),
                              window);
							  
							 
	
	//-------------BOXES------------------------
	vert = gtk_vbox_new (FALSE, 0);
	horiz1 = gtk_hbox_new (FALSE, 0);
	horiz2 = gtk_hbox_new (FALSE, 0);
	
	gtk_box_pack_start (GTK_BOX (horiz1), label1, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz1), label2, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button1, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button2, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button3, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button4, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button4, TRUE, FALSE, 0);
	gtk_container_add (GTK_CONTAINER (vert), horiz1);
	gtk_container_add (GTK_CONTAINER (vert), hpaned);

	gtk_box_pack_start (GTK_BOX (vert), vpaned, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (vert), horiz2, TRUE, FALSE, 0);
	

	gtk_container_add (GTK_CONTAINER (window), vert);
    
    /* The final step is to display the widgets. */
	gtk_widget_show (vert);
	gtk_widget_show (horiz1);
	gtk_widget_show (horiz2);
    gtk_widget_show (button1);
	gtk_widget_show (button2);
	gtk_widget_show (button3);
	gtk_widget_show (button4);
	gtk_widget_show (label1);
	gtk_widget_show (label2);
    gtk_widget_show (window);
	gtk_widget_show (hpaned);
	gtk_widget_show (vpaned);
    
    /* All GTK applications must have a gtk_main(). Control ends here
     * and waits for an event to occur (like a key press or
     * mouse event). */
    gtk_main ();
	

	return 0;
}
