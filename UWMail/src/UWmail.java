import java.util.zip.ZipEntry;
import java.util.zip.ZipException;
import java.util.zip.ZipFile;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.lang.Integer;
import java.lang.NumberFormatException;
import java.util.*;
import java.util.Date;
import java.util.Locale;
import java.text.SimpleDateFormat;
import java.text.DateFormat;
import java.text.ParseException;

public class UWmail {
  private static UWmailDB uwmailDB = new UWmailDB();
  private static final Scanner stdin = new Scanner(System.in);
  


  public static void main(String args[]) {
    if (args.length != 1) {
      System.out.println("Usage: java UWmail [EMAIL_ZIP_FILE]");
      System.exit(0);
    }

    loadEmails(args[0]);

    displayInbox();
  }

  private static void loadEmails(String fileName) {
			 String messageID = "";
			 String subject = "";
			 String from = "";
			 String to = "";
			 String inReplyTo = "";
			 ListADT<String> body = new DoublyLinkedList<String>();
			 ListADT<String> references =  new DoublyLinkedList<String>();
			 Date date = new Date();
			 DateFormat df = new SimpleDateFormat("EEE, dd MMM yyyy HH:mm:ss Z", Locale.US);
	     	
     		

		
		
	  try (ZipFile zf = new ZipFile(fileName);) {
	        //follow this approach for using <? extends ZipEntry>, even though we will not cover this in class.
	        Enumeration<? extends ZipEntry> entries = zf.entries();
	        
	        while(entries.hasMoreElements()) {
	        	
	          ZipEntry ze = entries.nextElement();
	          if(ze.getName().endsWith(".txt")) {

	            InputStream in = zf.getInputStream(ze);
	            Scanner sc = new Scanner(in);
	            StringBuilder sb = new StringBuilder();
	            StringBuilder sb2 = new StringBuilder();
	            
	            
	            while(sc.hasNextLine()) {
	              sb.append(sc.nextLine());
	              sb.append("\n");
	            }
	            
	            String content = sb.toString();
	            int i = 0;
	            boolean stat = true;
	            
	         messageID = "";                              //Reset local vars
	   		 subject = "";
	   		 from = "";
	   		 to = "";
	   		 inReplyTo = "";
	   		body = new DoublyLinkedList<String>();
	   		references =  new DoublyLinkedList<String>();
	            
	            
	            
	            	
	            	if(content.charAt(0) == 'I'){
	            	
	            	while(content.charAt(i) != ('>')){
	            	if(content.charAt(i) == ('<')){              // PARSE inReplyTo
	            		i++;
	            		while(content.charAt(i) != ('>')){
	            			sb2.append(content.charAt(i));
	            			i++;
	            			}
	            		inReplyTo = sb2.toString();
	            		
	            		
	            		break;
	            		}
	            	i++;
	            	}
	            	
	            	i++;
	            	sb2 = new StringBuilder();
	            	
	            	while(stat){
	            	if(content.charAt(i) == ('<')){              // PARSE References
	            		i++;
	            			while(stat){
	            				while(content.charAt(i) != ('>')){
	            					sb2.append(content.charAt(i));
	            					i++;
		            			}
	            			sb2.append(content.charAt(i));
	            			references.add(sb2.toString().substring(0, (sb2.toString().length()-1)));
	            			sb2 = new StringBuilder();
	            			if(content.charAt(i+1)=='\n'){
	            				stat = false;
	            			}
	            			else{
	            				while(content.charAt(i) != ('<')){  //
	            					i++;
	            				}
	            			}
	            			i++;
	            			}
	            	}
	            	i++;
	            	}

	            	
	            	i++;
	            	sb2 = new StringBuilder();	 
	            	stat = true;
	            	}
	            	
	            	//END of initial if method

	            	
	            	while(stat){
	            		if(content.charAt(i) == (' ')){              // PARSE Date
	            			i++;
		            		while(content.charAt(i) != ('\n')){
		            			sb2.append(content.charAt(i));
		            			i++;
		            			}
		            		try{
		            		date = df.parse(sb2.toString());  
		            		}catch(ParseException e){
		            			System.out.println("Caught an error parsing the date!");
		            		}
		            		
		            

	
		            		stat = false;
		            		
		            		}
	            		i++;
	            		}
	                                                
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	stat = true;
		            	
		            	while(stat){
			            	if(content.charAt(i) == ('<')){              // PARSE messageID
			            		i++;
			            		while(content.charAt(i) != ('>')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			}
			            		messageID = sb2.toString();
			            		stat = false;
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	stat = true;
		            	
		            	while(stat){
			            	if(content.charAt(i) == (' ')){              // PARSE Subject
			            		i++;
			            		while(content.charAt(i) != ('\n')){
			            			sb2.append(content.charAt(i));
			            			i++;	            			
			            			}
			            		subject = sb2.toString();
			            		stat=false;
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	stat = true;
		            	
		            	while(stat){
			            	if(content.charAt(i) == (' ')){              // PARSE From
			            		i++;
			            		while(content.charAt(i) != ('\n')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			}
			            		from = sb2.toString();
			            		stat=false;
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	stat = true;
		            	
		            	while(stat){
			            	if(content.charAt(i) == (' ')){              // PARSE To
			            		while(content.charAt(i) != ('\n')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			}
			            		to = sb2.toString();
			            		stat = false;
			            		}
			            	i++;
			            	}
		            
		            	
		            	sb2 = new StringBuilder();
		            	
		            	while(content.charAt(i) != ('\n')){
	            			sb2.append(content.charAt(i));              //Parse Body
	            			i++;
	            			}
		            	body.add(sb2.toString());
		            	
		            	
		            	
		            	while(i<content.length()){
		            		sb2.append(content.charAt(i));
	            			i++;
		            	}
		            	body.add(sb2.toString().substring(0, (sb2.toString().length()-1))); 
		            	
		            	
		            	uwmailDB.addEmail(new Email(date, messageID, subject, from, to, body, inReplyTo, references) );
	          }
	        }
	        
	        
	      } catch (ZipException e) {
	        System.out.println("A .zip format error has occurred for the file.");
	        System.exit(1);
	      } catch (IOException e) {
	        System.out.println("An I/O error has occurred for the file.");
	        System.exit(1);
	      } catch (SecurityException e) {
	        System.out.println("Unable to obtain read access for the file.");
	        System.exit(1);
	      }
	    }
  

  private static void displayInbox(){
    boolean done = false;
    DateFormat dateWithoutTime = new SimpleDateFormat("MMM d"); 
  
    Iterator<Conversation> it = uwmailDB.getInbox().iterator();
    boolean t = true;
    int x = 0;
    Conversation c1;
    System.out.println("Inbox:");
	System.out.println("--------------------------------------------------------------------------------");
	
	if(!it.hasNext()){
		System.out.println("No conversations to view.");
	}
	
    while(t){
    	if(it.hasNext()){
    		c1 = it.next();
    	}
    	else{
    		t=false;
    		break;
    	}                                                                     

    	System.out.printf("[%d] %s (%s)\n",x,c1.get((c1.size()-1)).getSubject(),dateWithoutTime.format(c1.get(0).getDate()));
    	//it2= it2.next();
    	x++;
    }
    
    while (!done) 
    {
      System.out.print("Enter option ([#]Open conversation, [T]rash, " + 
          "[Q]uit): ");
      String input = stdin.nextLine();

      if (input.length() > 0) 
      {

        int val = 0;
        boolean isNum = true;

        try {
          val = Integer.parseInt(input);
        } catch (NumberFormatException e) {
          isNum = false;
        }

        if(isNum) {
          if(val < 0) {
            System.out.println("The value can't be negative!");
            continue;
          } else if (val >= uwmailDB.getInbox().size()) {
            System.out.println("Not a valid number!");
            continue;
          } else {
            displayConversation(val);
            continue;
          }
          
        }
            
        if(input.length()>1)
        {
          System.out.println("Invalid command!");
          continue;
        }

        switch(input.charAt(0)){
          case 'T':
          case 't':
            displayTrash();
            break;

          case 'Q':
          case 'q':
            System.out.println("Quitting...");
            done = true;
            break;

          default:  
            System.out.println("Invalid command!");
            break;
        }
      } 
    } 
    System.exit(0);
  }

  private static void displayTrash(){
    boolean done = false;
    DateFormat dateWithoutTime = new SimpleDateFormat("MMM d"); 
  
    Iterator<Conversation> it = uwmailDB.getTrash().iterator();
    Conversation tr;
    int x = 0;
    System.out.println("Trash:");
    System.out.println("--------------------------------------------------------------------------------");
    
    if(!it.hasNext()){
    	System.out.println("No conversations to show." );
    }
    else{
    while(it.hasNext()){
    	tr=it.next();
    	System.out.printf("[%d] %s %s\n",x,tr.get((tr.size()-1)).getSubject(),dateWithoutTime.format(tr.get((tr.size()-1)).getDate()));
    	x++;
    }
    }
  
    
    while (!done) 
    {
      System.out.print("Enter option ([I]nbox, [Q]uit): ");
      String input = stdin.nextLine();

      if (input.length() > 0) 
      {
        if(input.length()>1)
        {
          System.out.println("Invalid command!");
          continue;
        }

        switch(input.charAt(0)){
          case 'I':
          case 'i':
            displayInbox();
            break;

          case 'Q':
          case 'q':
            System.out.println("Quitting...");
            done = true;
            break;

          default:  
            System.out.println("Invalid command!");
            break;
        }
      } 
    } 
    System.exit(0);
  }

  private static void displayConversation(int val) {
   
	  int v=val;
    boolean done = false;
    DateFormat dateWithoutTime = new SimpleDateFormat("MMM d"); 
    
    if(val>uwmailDB.getInbox().size()){
    	done = true;
    }
  

    Conversation c = uwmailDB.getInbox().get(v);
   

    while (!done) 
    {
    	 c = uwmailDB.getInbox().get(v);
    	 
    	 //Print Above (Future Messages)
    	 //System.out.printf("Convo  Size: %d\n\n)", uwmailDB.getInbox().get(v).size());
    	int size = (c.size()-1);
    	if (size>2){
    		size=0;
    	}
    	System.out.printf("SUBJECT: %s\n",c.get((c.size()-1)).getSubject());   
    	System.out.println("--------------------------------------------------------------------------------");
    	//System.err.printf("V VALUE: %d\n",v);
    	int k; 

    	for(k=(c.size()-1); (k > c.getCurrent());k--){
    		System.out.printf("%s | %s | %s\n",c.get(k).getFrom(),c.get(k).getBody().get(0),dateWithoutTime.format(c.get(k).getDate()));
    		System.out.println("--------------------------------------------------------------------------------");

    	}
    	                                                                 //Print Current Message
    	System.out.printf("From: %s\n",c.get(c.getCurrent()).getFrom());
    	System.out.printf("To:%s\n",c.get(c.getCurrent()).getTo());
    	System.out.printf("%s\n\n",dateWithoutTime.format(c.get(c.getCurrent()).getDate()));      
    	System.out.printf("%s\n",c.get(c.getCurrent()).getBody().get(1));
    	System.out.println("--------------------------------------------------------------------------------");
    	
for(k=(c.getCurrent()-1); k >= 0;k--){
		System.out.printf("%s | %s | %s\n",c.get(k).getFrom(),c.get(k).getBody().get(0),dateWithoutTime.format(c.get(k).getDate()));
		System.out.println("--------------------------------------------------------------------------------");
    	}
    	
    	
System.out.print("Enter option ([N]ext email, [P]revious email, " +
        "[J]Next conversation, [K]Previous\nconversation, [I]nbox, " +
        "[#]Move to trash, [Q]uit): ");
    String input = stdin.nextLine();

      if (input.length() > 0) 
      {

        if(input.length()>1)
        {
          System.out.println("Invalid command!");
          continue;
        }

        switch(input.charAt(0)){
          case 'P':
          case 'p':
           
        	  c = uwmailDB.getInbox().get(v);
        	  if(((c.getCurrent()<(c.size())-1))&&(c.size()>1)){

        		  c.moveCurrentBack();

        	  }
        	 
        	  
            displayConversation(v);
            break;
          case 'N':
          case 'n':
           
        	  c = uwmailDB.getInbox().get(v);
        	  if((c.getCurrent()!=0)&&(c.size()>1)){

        		  c.moveCurrentForward();
        	  }
        	 
        	
            displayConversation(v);
            break;
          case 'J':
          case 'j':
           
        	  if(v==(uwmailDB.getInbox().size())-1){
        		  displayInbox();
                  return;
        	  }
        	  else{
        		  v++;
        	 c = uwmailDB.getInbox().get(v);
        }
            break;

          case 'K':
          case 'k':
        
        	  if(v==0){
        		  displayInbox();
                  return;
        	  }
        	  else{
        		  v--;
        	 c = uwmailDB.getInbox().get(v);
        }
            break;

          case 'I':
          case 'i':
            displayInbox();
            return;

          case 'Q':
          case 'q':
            System.out.println("Quitting...");
            done = true;
            break;

          case '#':
        	  uwmailDB.deleteConversation(v);
        	  displayInbox();
            return;

          default:  
            System.out.println("Invalid command!");
            break;
        }
      } 
    } 
    System.exit(0);
  }

}

