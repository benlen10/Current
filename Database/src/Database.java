import java.util.Scanner;
import java.io.*;

public class Database {
	 static String[][] passwords;
	 Users[] userarray = new Users[10];

	
	
	public static void main(String args[]){
		Scanner s = new Scanner(System.in); 
		System.out.printf("------Enter Username------\n");
		String tmp = s.nextLine();
		System.out.printf("------Enter Password------\n");
		String tmp1 = s.nextLine();
		System.out.printf("Echo User: %s,  Pass:%s", tmp,tmp1);
		
		for (int i = 0; i<passwords.length; i++){
			System.out.printf("loop");
			
		}
		System.out.printf("File Test\n");
		load();
		System.out.printf("Options: Add, Remove, Edit");
		tmp = s.nextLine();
		if (tmp=="Add"){
			System.out.printf("Add");
		}
		else if(tmp == "Remove"){
			System.out.printf("Remove");
			
		}
		else if(tmp == "Edit"){
			System.out.printf("Edit");
		}
		else{
			System.out.printf("Unreconized command");
		}
		}
			
	
	static public void load(){
		File f = new File("C://Windows/test.txt");
		return; 
			
		
	}
	
	public void addAdmin(){
		
	}
	
}
