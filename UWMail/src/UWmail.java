import java.util.Date;
import java.util.Enumeration;
import java.util.Scanner;
import java.util.zip.ZipEntry;
import java.util.zip.ZipException;
import java.util.zip.ZipFile;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.lang.Integer;
import java.lang.NumberFormatException;
import java.util.*;

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
	     Date date;
		 String messageID;
		 String subject;
		 String from;
		 String to;
		 String inReplyTo;
		 ListADT<String> body = new DoublyLinkedList();
		 ListADT<String> references =  new DoublyLinkedList();

		
		
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
	            }
	            
	            String content = sb.toString();
	            int i = 0;
	            boolean stat = true;
	            
	            
	            while(stat){
	            	
	            	if(content.charAt(0) == 'I'){
	            	
	            	while(content.charAt(i) != ('>')){
	            	if(content.charAt(i) == ('<')){              // PARSE inReplyTo
	            		while(content.charAt(i) != ('>')){
	            			sb2.append(content.charAt(i));
	            			i++;
	            			}
	            		inReplyTo = sb2.toString();
	            		}
	            	i++;
	            	}
	            	
	            	i++;
	            	sb2 = new StringBuilder();
	            	
	            	while(content.charAt(i) != ('>')){
	            	if(content.charAt(i) == ('<')){              // PARSE References
	            		while(content.charAt(i) != ('>')){
	            			while(content.charAt(i) != (',')){
	            			sb2.append(content.charAt(i));
	            			i++;
	            			}
	            			references.add(sb2.toString());
	            			sb2 = new StringBuilder();
	            		}
	            		references.add(sb2.toString());  //Add final reference to ListADT
	            	}
	            	i++;
	            	}
	            	
	            	i++;
	            	sb2 = new StringBuilder();	 
	            	}
	            	else{
	            	
	            		while(stat){
	            		if(content.charAt(i) == (' ')){              // PARSE Date
		            		while(content.charAt(i) != ('\n')){
		            			sb2.append(content.charAt(i));
		            			i++;
		            		
		            			}
		            		//date = sb2.toString();  //TODO FIX: Correctly Parse Date
		            		}
	            		i++;
	            		}
	                                                
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	
		            	while(content.charAt(i) != ('>')){
			            	if(content.charAt(i) == ('<')){              // PARSE messageID
			            		while(content.charAt(i) != ('>')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			}
			            		messageID = sb2.toString();
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	
		            	while(content.charAt(i) != ('\n')){
			            	if(content.charAt(i) == (' ')){              // PARSE Subject
			            		while(content.charAt(i) != ('\n')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			
			            			}
			            		subject = sb2.toString();
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	
		            	while(content.charAt(i) != ('\n')){
			            	if(content.charAt(i) == (' ')){              // PARSE From
			            		while(content.charAt(i) != ('\n')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			}
			            		from = sb2.toString();
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	
		            	while(content.charAt(i) != ('\n')){
			            	if(content.charAt(i) == (' ')){              // PARSE To
			            		while(content.charAt(i) != ('\n')){
			            			sb2.append(content.charAt(i));
			            			i++;
			            			}
			            		to = sb2.toString();
			            		}
			            	i++;
			            	}
		            	
		            	i++;
		            	sb2 = new StringBuilder();
		            	
		            	
		            	while(i<content.length()){
		            		sb2.append(content.charAt(i));
	            			i++;
		            	}
		            	body.add(sb2.toString());  //TODO FIX Properly split body into List ADT
		            	
	            	}

	            }
	            
	            
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
    //TODO: Print the conversation here, according to the problem specifications
    //
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
            //
            displayConversation(val);
            break;
          case 'N':
          case 'n':
            //TODO: for this conversation, move the current email pointer 
            //  forward using Conversation.moveCurrentForward().
            //
            displayConversation(val);
            break;
          case 'J':
          case 'j':
            //TODO: Display the next conversation
            break;

          case 'K':
          case 'k':
            //TODO: Display the previous conversation
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

