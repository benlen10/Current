import java.util.Scanner;
import java.io.*;

public class Database {

	static Users[] userarray = new Users[10];
	 static int usercount = 0;
	 static String tmp;
	 static String tmp2;
	 
	
	
	public static void main(String args[]){
		
		load();
		Scanner s = new Scanner(System.in); 
		addAdmin();                        //Create inital admin account for first login.
		System.out.println("Enter Username:");
		String tmp = s.nextLine();
		System.out.println("Enter Password:");
		String tmp1 = s.nextLine();
		System.out.printf("Echo User: %s,  Pass:%s\n", tmp,tmp1);
		


		System.out.println("Options: View, Add, Remove, Edit");
		tmp = s.nextLine();
		System.out.printf("Input:%s\n",tmp);
		
		//Parse Options
		if (tmp.equals("Add")){
			add();
		}
		else if(tmp.equals("Remove")){
			remove();
			
		}
		else if(tmp.equals("View")){
			view();
			
		}
		else if(tmp.equals("Edit")){
			edit();
		}
		else{
			System.out.printf("Unreconized command");
			return;
		}
		}
			
	
	//Methods
	
		public static  void load(){
		File f = new File("C:/java/userdata.txt");
		try{
		f.createNewFile();
		BufferedReader br = new BufferedReader(new FileReader("C:/java/userdata.txt"));
		char c = (char) br.read();
		String user = "";
		String pass = "";
		String message = "";
		boolean status = true;
		
		
		if(c!='*'){
			System.out.println("Invalid or corrupt userdata.txt");           //Userdata.txt integrity check 
			return;
		}
		
		while(status == true){
		userarray[usercount] = new Users();   //Initialize current position
		
		
		StringBuilder sb = new StringBuilder();
		while((c!='|')&&(c!='\0')&&(c!='\n')){  //User parse
			sb.append(c);
			c = (char) br.read();
			
		}
		
		userarray[usercount].name = sb.toString();
		//System.out.printf("User#%d, Name: %s\n", usercount, userarray[usercount].name);
		c = (char) br.read();

		 sb = new StringBuilder();
		while((c!='|')&&(c!='\0')&&(c!='\n')){  //Pass Parse
			sb.append(c);
			c = (char) br.read();
			
		}
		userarray[usercount].setP(sb.toString());
		//System.out.printf("User#%d, Pass: %s\n", usercount, userarray[usercount].getP());
		c = (char) br.read();

		sb = new StringBuilder();
		while((c!='|')&&(c!='\0')&&(c!='\n')){  //Message parse
			sb.append(c);
			c = (char) br.read();
			
		}
		userarray[usercount].message = sb.toString();
		//System.out.printf("User#%d, Message: %s\n", usercount, userarray[usercount].message);
		int x = 0;
		while((c!='*')&&(x<10)){
		c = (char) br.read();
		x++;
		if(c=='#'){
			status = false;
		}
		
		//System.out.printf("Char:%c\n",c);
		}
		c = (char) br.read();
		usercount++;
		}
		
		System.out.printf("Current usercount: %d\n", usercount);
		}
		catch (IOException e){
			e.printStackTrace();
		}
		

	}
	
	public static void addAdmin(){
		userarray[0] = new Users();
		userarray[0].name = "admin";
		userarray[0].setP("admin");
		userarray[0].message = "Inital Account";
		usercount++;
		System.out.println("Inital admin user created");
	}
	
	public static void add(){
		userarray[usercount] = new Users();
		Scanner s = new Scanner(System.in);
		System.out.printf("Enter new user name");
		tmp = s.nextLine();
		userarray[usercount].name = tmp;
		System.out.printf("Enter new password");
		tmp = s.nextLine();
		userarray[usercount] = new Users();
		userarray[usercount].setP(tmp);
		usercount++;
		System.out.printf("Summary: User: %s Pass: %s Message: %s", userarray[usercount].name, userarray[usercount].getP(), userarray[usercount].message );
	}
	public static void view(){
		for(int i = 0; i<usercount; i++){
		System.out.printf("User %d: %s\n", i, userarray[i].name);
		}
	}
	
	
	public static void remove(){
		System.out.printf("Enter user name to remove");
		Scanner s = new Scanner(System.in);
		tmp = s.nextLine();
		for(int i =0; i<usercount; i++){
			if(userarray[i].name == tmp){
				System.out.println("Found User");
				userarray[i].deleteUser();
				System.out.println("User secuessfully removed");
			}
		}	
	}

	
	public static void edit(){
		System.out.printf("Enter user to edit");
		Scanner s = new Scanner(System.in);
		tmp = s.nextLine();
		for(int i =0; i<usercount; i++){
			if(userarray[i].name == tmp){
				System.out.println("Options to edit: Name, Pass, Message");
				
				if (tmp=="Name"){
					System.out.println("Enter new name");
					tmp = s.nextLine();
					userarray[i].name = tmp;
					System.out.println("Name change successful");
				}
				else if(tmp == "Pass"){
					System.out.println("Enter new pass");
					tmp = s.nextLine();
					userarray[i].setP(tmp);
					System.out.println("Password change successful");
					
				}
				else if(tmp == "Message"){
					System.out.println("Enter new message");
					tmp = s.nextLine();
					userarray[i].message = tmp;
					System.out.println("Message updated");
				}
				else{
					System.out.println("Unreconized command");
					return;
				}
				
	}
		}
	}
}

	
		
