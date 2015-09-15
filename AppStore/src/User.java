import java.util.*;

public class User {

	//Constructor data members
	private String email;
	private String password;
	private String firstName;
	private String lastName;
	private String country;
	private String type;
	//Custom data members
	private List<App> downloadList = new ArrayList<App>();
	private List<App> uploadList = new ArrayList<App>();

	public User(String email, String password, String firstName,
			String lastName, String country, String type)
			throws IllegalArgumentException {
		
		this.email=email;
		this.password=password;
		this.firstName=firstName;
		this.lastName=lastName;
		this.country = country;
		this.type = type;
	}

	public String getEmail() {
		return email;
	}

	public boolean verifyPassword(String testPassword) {
		if(testPassword.equals(password)){
		return true;
		}
		else{
			return false;
		}
	}

	public String getFirstName() {
		return firstName;
	}

	public String getLastName() {
		return lastName;
	}

	public String getCountry() {
		return country;
	}

	public boolean isDeveloper() {
		if(type.equals("developer")){
			return true;
		}
		else{
			return false;
		}
	}

	public void subscribeAsDeveloper() {
		type = "developer";
	}

	public void download(App app) { 
		downloadList.add(app);
	}

	public void upload(App app) {
		uploadList.add(app);
	}
	
	public List<App> getAllDownloadedApps() {
		return downloadList;
	}
	
	public List<App> getAllUploadedApps() {
		return uploadList;
	}
		
}
