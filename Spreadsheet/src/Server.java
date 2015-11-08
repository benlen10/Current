//Title:            Spreadsheet Server (Project 3)
//Files:            Database.java, Document.java, EmptyQueueException.java, EmptyStackException.java, Operation.java, QueueADT.Java,
//                    SimpleQueue.java, SimpleStack.java, StackADT.java, User.java, WAL.java
//Semester:     Fall 2015
//
//Author:         Ben Lenington
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

import java.io.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;
//import Operation.OP;

public class Server {
	/*
	 * The main class for the spreadsheet program responsible for the program flow and parsing the input file.
	 */
    public Database dat;
    private String inputFileName;
    private String outputFileName;
    PrintWriter writer;
    private List<Operation> ops;
    public Server(String inputFileName, String outputFileName) {
    	this.inputFileName = inputFileName;
    	this.outputFileName = outputFileName;
       dat = new Database();
       ops = new ArrayList<Operation>();     
       try{
    	  writer = new PrintWriter(new BufferedWriter(new FileWriter(outputFileName)), true);
       }
       catch(IOException e){
    	   System.err.printf("IOException caught");
       }
       
    }

    public void run(){
    	//Preform the essential server processes in the two methods
        initialize();
        process();
    }

    private void initialize() {
    	//Parse the input text file creating Document and User objects.
    	try{
    	BufferedReader file = new BufferedReader(new FileReader(inputFileName));
    	StringBuilder sb = new StringBuilder();  
		char c = ' ' ;
		int tmp;
		int rows = 0;
		int cols = 0;
		int constant = 0;
		long timestamp;
		int i =0;
		int argCount = 0;    //Keep track of the number of args following the operation
		int docCount = Integer.parseInt(Character.toString((char) file.read()));
		String docName, op,user;
		Operation.OP opp;
		opp = Operation.OP.ADD;
		List<User> users = new ArrayList<User>();
		
		c = (char) file.read();
		c = (char) file.read();
		c = (char) file.read();
		while(i<docCount){
			
			while(c!=','){                               //Parse DocName
				
				
				sb.append(c);
				c = (char) file.read();
				
			}
			Iterator<Document> it = dat.getDocumentList().iterator();        //Check to see if the document name is a duplicate
			while(it.hasNext()){
					if(it.next().getDocName().equals(sb.toString())){
						throw new IllegalArgumentException();
					}
				}

			docName = sb.toString();

			
			
			rows = Integer.parseInt(Character.toString((char) file.read()));         //Parse rows
			c = (char) file.read();
			cols =  Integer.parseInt(Character.toString((char) file.read()));  
			sb = new StringBuilder();
			c = (char) file.read();
			c = (char) file.read();
			boolean stat = true;       //A boolean value to detect when a '\n' char is hit
			while(stat){  
			while((c!=',')&&stat){       
				sb.append(c);
				c = (char) file.read();
				if(c=='\n'){
					stat = false;
				}
			}
			boolean dupUser = false;        //Determines if a userId already exists in the user database
			
			Iterator<Document> itt = dat.getDocumentList().iterator();        //Check to see if userId is a duplicate
			while(itt.hasNext()){
				Iterator<String> it2 = itt.next().getAllUserIds().iterator();
				while(it2.hasNext()){
					if(it2.next().equals(sb.toString())){
						dupUser=true;
					}
				}
			}
			if(!dupUser){
				users.add(new User(sb.toString()));                      //Parse Users
			}
			
			sb = new StringBuilder();
			c = (char) file.read();
			}
	
			dat.addDocument(new Document(docName, rows, cols, users));
			i++;
		}
		 boolean stat2=true;  //A boolean value to break the outer while loop once the end of the file is reached.
		 
														//PROCESS OPERATIONS 
		while(stat2){
			rows = 0;
			cols = 0;
			constant = 0;
			sb = new StringBuilder();         	
		while(c!=','){       
			sb.append(c);
			c = (char) file.read();
			
		}
		timestamp = Integer.parseInt(sb.toString());                    //Timestamp
		
		sb = new StringBuilder();              
		c = (char) file.read();
		while(c!=','){                                           
			sb.append(c);
			c = (char) file.read();
		}
		user = sb.toString();                                  //Pase User
		sb = new StringBuilder();              
		c = (char) file.read();
		while(c!=','){                                           
			sb.append(c);
			c = (char) file.read();
		}
		docName = sb.toString();                                  //Pase docName
		
		sb = new StringBuilder();              
		c = (char) file.read();
		int u=0;
		while((c!=',')&&(c!='\n')&&(u!=-1)){     
			
			sb.append(c);
			u = file.read();
			c = (char) u;
		}
		op = sb.toString();
		if(u==-1){   //Break while loop  
			stat2 = false;
		}
		
		if(c=='\n'){
			op = sb.substring(0, sb.length()-1);
			c = (char) file.read();

		}
	
		
		
		if(op.equals("set")){
			opp = Operation.OP.SET;
		} else if(op.equals("clear")){
			opp = Operation.OP.CLEAR;
		} else if(op.equals("add")){
			opp = Operation.OP.ADD;
		} else if(op.equals("sub")){
			opp = Operation.OP.SUB;
		} else if(op.equals("mul")){
			opp = Operation.OP.MUL;
		} else if(op.equals("div")){
			opp = Operation.OP.DIV;
		} else if(op.equals("undo")){
			opp = Operation.OP.UNDO;
		} else if(op.equals("redo")){
			opp = Operation.OP.REDO;
		}
		
		u = 0;
		rows = 0;
		cols = 0;
		constant = 0;
		if((!op.equals(("undo")))&&(!op.equals("redo"))){
		sb = new StringBuilder();
		rows =  Integer.parseInt(Character.toString((char) file.read()));           //Parse rows
		argCount++;
		c = (char) file.read();
		
		
		cols = Integer.parseInt(Character.toString((char) file.read()));           //Parse cols
		argCount++; 
		c = (char) file.read();
		
		
		if(!op.equals("clear")){
		constant = Integer.parseInt(Character.toString((char) file.read()));         //Parse constant
		argCount++;
		c = (char) file.read();
		}
		u = file.read();
		c = (char) file.read();
		}
	
		if(argCount==0){
		ops.add(new Operation(docName, user, opp, timestamp));
		}
		else if(argCount==2){
			ops.add(new Operation(docName, user, opp, rows, cols, timestamp));
    	
    	}
		else if(argCount==3){
			ops.add(new Operation(docName, user, opp, rows, cols, constant, timestamp));
    	}
		argCount=0;
		}
    	}
		catch(IOException e){
			e.printStackTrace();
		}
    }

    private void process() {
    	//Process the operations in the ops queue one by one
       Iterator<Operation> it = ops.iterator();
       while(it.hasNext()){  
     	  writer.println(dat.update(it.next()));
     	  }
       }


    public static void main(String[] args){
    	//The starting point for the program which passes in the text file names when creating a new Server object.
        if(args.length != 2){
            System.out.println("Usage: java Server [input.txt] [output.txt]");
            System.exit(0);
        }
        Server server = new Server(args[0], args[1]);
        server.run();
    }
}
