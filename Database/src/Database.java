import java.util.Scanner;

public class Database {
	 static String[][] passwords;
	
	
	public static void main(String args[]){
		Scanner s = new Scanner(System.in); 
		System.out.printf("------Enter Username------\n");
		String tmp = s.nextLine();
		System.out.printf("------Enter Password------\n");
		String tmp1 = s.nextLine();
		System.out.printf("Echo User: %s,  Pass:%s", tmp,tmp1);
		
	}
	
}
