import java.io.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;
//import Operation.OP;

public class Server {
    public Database dat;
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
			docName = sb.toString();

			
			
			rows = Integer.parseInt(Character.toString((char) file.read()));         //Parse rows
			c = (char) file.read();
			cols =  Integer.parseInt(Character.toString((char) file.read()));  
			sb = new StringBuilder();
			c = (char) file.read();
			c = (char) file.read();
			boolean stat = true;
			while(stat){  
			while((c!=',')&&stat){       
				sb.append(c);
				c = (char) file.read();
				//System.err.printf("%c", c);
				if(c=='\n'){
					stat = false;
				}
			}
			users.add(new User(sb.toString()));                      //Parse Users
			//System.err.printf("USER:%s\n",sb.toString());
			sb = new StringBuilder();
			c = (char) file.read();
			}
			//System.err.printf("Name: %s Rows: %d Cols: %d\n", docName, rows, cols);
			dat.addDocument(new Document(docName, rows, cols, users));
			i++;
		}
		 boolean stat2=true;
														//PROCESS OPERATIONS 
		while(stat2){
			rows = 0;
			cols = 0;
			constant = 0;
			sb = new StringBuilder();         	
			//System.err.println("LOOP");
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
		//sb.append(c);
		int u=0;
		while((c!=',')&&(c!='\n')&&(u!=-1)){     
			
			sb.append(c);
			u = file.read();
			c = (char) u;
		}
		//System.err.printf("UVALUE:%d   %c\n",u,c);
		op = sb.toString();
		if(u==-1){   //Break while loop  
			stat2 = false;
			//System.err.println("FALSE");
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
		//c = (char) file.read();
		rows = 0;
		cols = 0;
		constant = 0;
		if((!op.equals(("undo")))&&(!op.equals("redo"))){
			//System.err.printf("OP EQUALS: %s.\n", op);
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

		//System.err.printf("Name: %s User: %s OP: %s Timestamp: %d Rows: %d Cols: %d Constant: %d\n", docName, user, op, timestamp,rows, cols, constant);
	
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
