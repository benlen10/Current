
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

/* This is a callback function. The data arguments are ignored
 * in this example. More on callbacks below. */
static void hello( GtkWidget *widget,
                   gpointer   data )
{
    
	sendCommand(host, port, command, response);
	g_print ("Hello World2\n");
}

static gboolean delete_event( GtkWidget *widget,
                              GdkEvent  *event,
                              gpointer   data )
{
    /* If you return FALSE in the "delete-event" signal handler,
     * GTK will emit the "destroy" signal. Returning TRUE means
     * you don't want the window to be destroyed.
     * This is useful for popping up 'are you sure you want to quit?'
     * type dialogs. */

    g_print ("delete event occurred\n");

    /* Change TRUE to FALSE and the main window will be destroyed with
     * a "delete-event". */

    return TRUE;
}

/* Another callback */
static void destroy( GtkWidget *widget,
                     gpointer   data )
{
    gtk_main_quit ();
}


//--------------PANNED Methods------------------------
static GtkWidget *create_list( void )
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
        gchar *msg = g_strdup_printf ("Message #%d", i);
        gtk_list_store_append (GTK_LIST_STORE (model), &iter);
        gtk_list_store_set (GTK_LIST_STORE (model), 
	                    &iter,
                            0, msg,
	                    -1);
	g_free (msg);
    }
   
    cell = gtk_cell_renderer_text_new ();

    column = gtk_tree_view_column_new_with_attributes ("Messages",
                                                       cell,
                                                       "text", 0,
                                                       NULL);
  
    gtk_tree_view_append_column (GTK_TREE_VIEW (tree_view),
	  		         GTK_TREE_VIEW_COLUMN (column));

    return scrolled_window;
}
   
/* Add some text to our text widget - this is a callback that is invoked
when our window is realized. We could also force our window to be
realized with gtk_widget_realize, but it would have to be part of
a hierarchy first */

static void insert_text( GtkTextBuffer *buffer )
{
   GtkTextIter iter;
 
   gtk_text_buffer_get_iter_at_offset (buffer, &iter, 0);

   gtk_text_buffer_insert (buffer, &iter,   
    "From: pathfinder@nasa.gov\n"
    "To: mom@nasa.gov\n"
    "Subject: Made it!\n"
    "\n"
    "We just got in this morning. The weather has been\n"
    "great - clear but cold, and there are lots of fun sights.\n"
    "Sojourner says hi. See you soon.\n"
    " -Path\n", -1);
}
   
/* Create a scrolled text area that displays a "message" */
static GtkWidget *create_text( void )
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
   insert_text (buffer);

   gtk_widget_show_all (scrolled_window);

   return scrolled_window;
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
	GtkWidget *users;
	GtkWidget *rooms;
	GtkWidget *hpaned;
	GtkWidget *vpaned;
	GtkWidget *list;
	GtkWidget *list2;
    GtkWidget *text;
	GtkWidget *text2;
	
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
	button2 = gtk_button_new_with_label ("Exit");
	button3 = gtk_button_new_with_label ("Create Account");
	button4 = gtk_button_new_with_label ("New Button");
	hpaned = gtk_hpaned_new ();
	vpaned = gtk_vpaned_new ();
	
	//ADD to hpaned PANEL
	list = create_list ();
    gtk_paned_add1 (GTK_PANED (hpaned), list);
    gtk_widget_show (list);
   
	list2 = create_list ();
    gtk_paned_add2 (GTK_PANED (hpaned), list2);
    gtk_widget_show (list2);
	
	//ADD to vpand PANEL
	
	text = create_text ();
    gtk_paned_add1 (GTK_PANED (vpaned), text);
    gtk_widget_show (text);
   
    text2 = create_text ();
    gtk_paned_add2 (GTK_PANED (vpaned), text2);
    gtk_widget_show (text2);
   

	
    //Set the Widget sizes
	gtk_widget_set_size_request (GTK_WIDGET (window), 450, 500);
	
	
    /* When the button receives the "clicked" signal, it will call the
     * function hello() passing it NULL as its argument.  The hello()
     * function is defined above. */

    g_signal_connect (button1, "clicked",
		      G_CALLBACK (hello), NULL);
    
    /* This will cause the window to be destroyed by calling
     * gtk_widget_destroy(window) when "clicked".  Again, the destroy
     * signal could come from here, or the window manager. */
    g_signal_connect_swapped (button2, "clicked",
			      G_CALLBACK (gtk_widget_destroy),
                              window);
							  
							 
    
    /* This packs the button into the window (a gtk container). */
    //gtk_container_add (GTK_CONTAINER (window), button);
	//gtk_container_add (GTK_CONTAINER (window), button2);
	
	//-------------BOXES------------------------
	vert = gtk_vbox_new (FALSE, 0);
	horiz1 = gtk_hbox_new (FALSE, 0);
	horiz2 = gtk_hbox_new (FALSE, 0);

	gtk_box_pack_start (GTK_BOX (horiz1), list, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz1), list, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button1, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button2, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (horiz2), button3, TRUE, FALSE, 0);
	gtk_container_add (GTK_CONTAINER (vert), hpaned);

	//gtk_box_pack_start (GTK_BOX (vert), horiz1, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (vert), vpaned, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (vert), horiz2, TRUE, FALSE, 0);
	gtk_box_pack_start (GTK_BOX (vert), button4, TRUE, FALSE, 0);

	gtk_container_add (GTK_CONTAINER (window), vert);
    
    /* The final step is to display the widgets. */
	gtk_widget_show (vert);
	gtk_widget_show (horiz1);
	gtk_widget_show (horiz2);
    gtk_widget_show (button1);
	gtk_widget_show (button2);
	gtk_widget_show (button3);
	gtk_widget_show (button4);
	//gtk_widget_show (users);
	//gtk_widget_show (rooms);
    gtk_widget_show (window);
	gtk_widget_show (hpaned);
	gtk_widget_show (vpaned);
    
    /* All GTK applications must have a gtk_main(). Control ends here
     * and waits for an event to occur (like a key press or
     * mouse event). */
    gtk_main ();
	
	
	
	

	return 0;
}




 
