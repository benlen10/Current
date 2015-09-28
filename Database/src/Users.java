
public class Users {
	public String name;
	private String pass;
	public String message;
	
	 void deleteUser(){
		name = "";
		pass = "";
		message = "";
	}


String getP(){
	return pass;
}

public void setP(String p){
	pass = p;
	//System.out.println("Setter: Pass changed");
}

}
