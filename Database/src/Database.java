import java.util.Scanner;
import java.io.*;

public class Database {
	 static String[][] passwords;
	 static Users[] userarray = new Users[10];
	 static int usercount = 0;
	 static String tmp;

	
	
	public static void main(String args[]){
		Scanner s = new Scanner(System.in); 
		addAdmin();                        //Create inital admin account for first login.
		System.out.printf("Enter Username\n");
		String tmp = s.nextLine();
		System.out.printf("Enter Password\n");
		String tmp1 = s.nextLine();
		System.out.printf("Echo User: %s,  Pass:%s\n", tmp,tmp1);
		
		/*for (int i = 0; i<passwords.length; i++){
			System.out.printf("loop");
			
		}
		*/
		load();
		System.out.printf("Options: Add, Remove, Edit");
		tmp = s.nextLine();
		
		//Parse Options
		if (tmp=="Add"){
			add();
		}
		else if(tmp == "Remove"){
			remove();
			
		}
		else if(tmp == "Edit"){
			edit();
		}
		else{
			System.out.printf("Unreconized command");
			return;
		}
		}
			
	
	//Methods
	
	static public void load(){
		File f = new File("C:/java/pass.txt");
		try{
		f.createNewFile();
		}
		catch (IOException e){
			e.printStackTrace();
		}
		return; 
	}
	
	public static void addAdmin(){
		userarray[0].name = "admin";
		userarray[0].setP("admin");
		userarray[0].message = "Inital Account";
		System.out.println("Inital admin user created");
	}
	
	public static void add(){
		Scanner s = new Scanner(System.in);
		System.out.printf("Enter new user name");
		tmp = s.nextLine();
		userarray[usercount].name = tmp;
		System.out.printf("Enter new password");
		tmp = s.nextLine();
		userarray[usercount].setP(tmp);
		usercount++;
		System.out.printf("Summary: User: %s Pass: %s Message: %s", userarray[usercount].name, userarray[usercount].getP(), userarray[usercount].message );
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

	
		
