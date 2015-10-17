import java.util.zip.ZipEntry;
import java.util.zip.ZipException;
import java.util.zip.ZipFile;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.lang.Integer;
import java.lang.NumberFormatException;
import java.util.*;
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
	     Date date = new Date();
		 String messageID = "";
		 String subject = "";
		 String from = "";
		 String to = "";
		 String inReplyTo = "";
		 ListADT<String> body = new DoublyLinkedList();
		 ListADT<String> references =  new DoublyLinkedList();

		
		
	  try (ZipFile zf = new ZipFile(fileName);) {
	        //follow this approach for using <? extends ZipEntry>, even though we will not cover this in class.
	        Enumeration<? extends ZipEntry> entries = zf.entries();
	        
	        while(entries.hasMoreElements()) {
	        	
	          ZipEntry ze = entries.nextElement();
	          if(ze.getName().endsWith(".txt")) {
	        	//  System.out.printf("NEXT MESSAGE\n\n");   //TODO FIX Remove Debug
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
		            		
		            		DateFormat df = DateFormat.getDateInstance(DateFormat.LONG, Locale.US);
		            		/*try{
		            		date = df.parse(sb2.toString());  //TODO FIX: Correctly Parse Date
		            		}catch(ParseException e){
		            			e.printStackTrace();
		            		}*/
		            		
		            //TODO debug print date
		            		//System.out.printf("RAW Date: %s\n",sb2.toString());
		            		//System.out.printf("Date: %s\n",date.toString());

	
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
		            	
		            	
		            	while(i<content.length()){
		            		sb2.append(content.charAt(i));
	            			i++;
		            	}
		            	
		            	body.add(sb2.toString());  //TODO FIX Properly split body into List ADT
		            	
		            	/*System.out.printf("InReplyTo: %s\n\n", inReplyTo);
		            	System.out.printf("Reference0: %s\n\n", references.get(0));
		            	//System.out.printf("Date: %s\n\n", date);
		            	System.out.printf("messageID: %s\n\n", messageID);
		            	System.out.printf("Subject: %s\n\n", subject);
		            	System.out.printf("From: %s\n\n", from);
		            	System.out.printf("To: %s\n\n", to);
		            	System.out.printf("Body0: %s\n\n", body.get(0));
		            	*/
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
    //TODO: print out the inbox here, according to the guidelines in the problem
    //
    Iterator<Conversation> it = uwmailDB.getInbox().iterator();
    int x = 0;

    while(it.hasNext()){
    	System.out.printf("[%d] %s (Date)\n",x,it.next().get(0).getSubject());
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
          } else if (val >= uwmailDB.size()) {
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
    //TODO: print out the trash here according to the problem specifications
    //
    Iterator<Conversation> it = uwmailDB.getInbox().iterator();
    int x = 0;

    while(it.hasNext()){
    	System.out.printf("[%d] %s (Date)\n",x,it.next().get(0).getSubject());
    	x++;
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
    //TODO: Check whether val is valid as a conversation number. If not, take
    //the user back to the inbox view and continue processing commands.
    //
    boolean done = false;
    
    if(val>uwmailDB.getInbox().size()){
    	done = true;
    }
    //TODO: Print the conversation here, according to the problem specifications
    //
    Conversation c = uwmailDB.getInbox().get(val);
    Iterator<Conversation> it = uwmailDB.getInbox().iterator();
    
    while(it.hasNext()){
    	System.out.printf("SUBJECT: %s\n",e.getSubject());
    	System.out.println("--------------------------------------------------------------------------------");
    	System.out.printf("%s | %s | (Date)\n",e.getSubject());
    	System.out.println("--------------------------------------------------------------------------------");
    	System.out.printf("From: %s\n",e.getFrom());
    	System.out.printf("From: %s\n",e.getTo());
    	System.out.printf("(Date)\n\n");      
    	System.out.printf("%s\n\n",e.getBody());
    	
    	e = it.next();
    }
    
    while (!done) 
    {
      System.out.print("Enter option ([N]ext email, [P]revious email, " + 
          "[J]Next conversation, [K]Previous conversation, [I]nbox, [#]Move " +
          "to trash, [Q]uit): ");
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
            //TODO: for this conversation, move the current email pointer back 
            //  using Conversation.moveCurrentBack().
            //DONE
        	  c.moveCurrentBack();
        	  
            displayConversation(val);
            break;
          case 'N':
          case 'n':
            //TODO: for this conversation, move the current email pointer 
            //  forward using Conversation.moveCurrentForward().
            //DONE
        	  c.moveCurrentForward();
            displayConversation(val);
            break;
          case 'J':
          case 'j':
            //TODO: Display the next conversation DONE
        	  if(it.hasNext()){
        	  c = it.next();
        	  }else{
        		  done=true;    //Return to Inbox
        	  }
            break;

          case 'K':
          case 'k':
            //TODO: Display the previous conversation
        	  if(it.hasNext()){
            	  c = it.
            	  }else{
            		  done=true;    //Return to Inbox
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
            //TODO: add delete conversation functionality. This conversation
            //should be moved to the trash when # is entered, and you should
            //take the user back to the inbox and continue processing input.
            //
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

