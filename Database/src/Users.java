
public class Users {
	public String name;
	private String pass;
	public String message;
	
	 void deleteUser(){
		name = "";
		pass = "";
		message = "";
	}


public String getPass(){
	return pass;
}

public void setPass(String p){
	pass = p;
	System.out.println("Setter: Pass changed");
}

}
