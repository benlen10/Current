import java.io.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;
//import Operation.OP;

public class Server {
    private Database dat;
    private String inputFileName;
    private String outputFileName;
    private List<Operation> ops;
    BufferedWriter writer;
    public Server(String inputFileName, String outputFileName) {
    	this.inputFileName = inputFileName;
    	this.outputFileName = outputFileName;
       dat = new Database();
       ops = new ArrayList<Operation>();     
       try{
    	   writer = new BufferedWriter(new FileWriter(outputFileName)); 
       }
       catch(IOException e){
    	   System.err.printf("IOException caught");
       }
       
    }

    public void run(){
        initialize();
        process();
    }

    private void initialize() {
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
		int argCount = 0;
		int docCount = (char) file.read();
		String docName, op,user;
		Operation.OP opp;
		opp = Operation.OP.ADD;
		List<User> users = new ArrayList<User>();
		
		while(i<docCount){
			
			while(c!=','){                               //Parse DocName
				sb.append(c);
				c = (char) file.read();
			}
			docName = sb.toString();

			c = (char) file.read();
			sb = new StringBuilder();
			rows = (char) file.read();         //Parse rows
			c = (char) file.read();
			cols = (char) file.read();         //Parse cols
			c = (char) file.read();       
			
			while(c!='\n'){  
			while(c!=','){                               
				sb.append(c);
				c = (char) file.read();
			}
			users.add(new User(sb.toString()));
			c = (char) file.read();
			}
			dat.addDocument(new Document(docName, rows, cols, users));
			i++;
		}
		         
		while(c!=','){                                            //PROCESS OPERATIONS 
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
		while(c!=','){                                           
			sb.append(c);
			c = (char) file.read();
		}
		op = sb.toString();                                  
		
		if(op.equals("SET")){
			opp = Operation.OP.SET;
		} else if(op.equals("CLEAR")){
			opp = Operation.OP.CLEAR;
		} else if(op.equals("ADD")){
			opp = Operation.OP.ADD;
		} else if(op.equals("SUB")){
			opp = Operation.OP.SUB;
		} else if(op.equals("MUL")){
			opp = Operation.OP.MUL;
		} else if(op.equals("DIV")){
			opp = Operation.OP.DIV;
		} else if(op.equals("UNDO")){
			opp = Operation.OP.UNDO;
		} else if(op.equals("REDO")){
			opp = Operation.OP.REDO;
		}
		
		
		int u = 0;
		while((c!=',')||(u!=-1)){ 
		u =  file.read();
		c = (char) u;
		sb = new StringBuilder();
		rows =  file.read();         //Parse rows
		argCount++;
		u = file.read();
		c = (char) u;
		cols = (char) file.read();         //Parse cols
		argCount++;
		u = file.read();    
		c = (char) u;
		constant = (char) file.read();       //Parse constant
		argCount++;
		u = file.read();   
		c = (char) u;
		u=-1;                                     //Break while loop  
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
    	}
		catch(IOException e){
			e.printStackTrace();
		}
    }

    private void process() {
       Iterator<Operation> it = ops.iterator();
       while(it.hasNext()){
    	   try{
     	  writer.write(dat.update(it.next()));
    	   }
    	   catch(IOException e){
    		   System.err.println("IOException caught when writing to file (Updating Database)");
    	   }
     	  }
       }


    public static void main(String[] args){
        if(args.length != 2){
            System.out.println("Usage: java Server [input.txt] [output.txt]");
            System.exit(0);
        }
        Server server = new Server(args[0], args[1]);
        server.run();
    }
}
