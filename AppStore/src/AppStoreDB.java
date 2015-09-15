import java.util.*;


public class AppStoreDB {
	
	//Private data members
	private List<User> users = new ArrayList();
	private List<App> apps = new ArrayList();
	private List<String> categories = new ArrayList();
	

	public AppStoreDB() {
		
	}

	public User addUser(String email, String password, String firstName,
			String lastName, String country, String type)
			throws IllegalArgumentException {
		
		User u = new User(email,password,firstName,
			lastName, country, type);
		users.add(u);
		return u;
		
		//FIX!!!! Implement check for existing dup user
	}
	
	public void addCategory(String category) {
		categories.add(category);
	}
	
	public List<String> getCategories() {
		return categories;
	}
	
	public User findUserByEmail(String email) {
		User u;
		Iterator<User> it = users.iterator();
		while(it.hasNext()){
			u = it.next();
			if(u.getEmail().equals(email)){
				return u;
			}
		}
		System.out.println("Email not found");
		return null;
		
	}
	
	public App findAppByAppId(String appId) {
		App a;
		Iterator<App> it = apps.iterator();
		while(it.hasNext()){
			a = it.next();
			if(a.getAppId().equals(appId)){
				return a;
			}
		}
		System.out.println("appId not found");
		return null;
	}
	
	public User loginUser(String email, String password) {
		User u;
		Iterator<User> it = users.iterator();
		while(it.hasNext()){
			u = it.next();
			if(u.getEmail().equals(email)){
				if (u.verifyPassword(password)){
					return u;
				}
				return null;
			}
		}
		System.out.println("Email not found");
		return null;
	}

	public App uploadApp(User uploader, String appId, String appName,
			String category, double price, 
			long timestamp) throws IllegalArgumentException {
		
		if(!(uploader.isDeveloper())){
			System.err.println("User is not a developer");
			return null;
		}
		
		App a = new App(uploader, appId, appName, category,
				 price, timestamp);
		apps.add(a);
		uploader.upload(a);
		return a;

	}
	
	public void downloadApp(User user, App app) {		
		app.download(user);
		user.download(app);
	}
	
	public void rateApp(User user, App app, short rating) {
		app.rate(user, rating);
	}
	
	public boolean hasUserDownloadedApp(User user, App app) {		
		List<App> downloads = user.getAllDownloadedApps();
		Iterator<App> it = downloads.iterator();
		while(it.hasNext()){
			if(app == it.next()){
				return true;
			}
		}
		return false;
	}

			
	public List<App> getTopFreeApps(String category) {
		List<App> free = new ArrayList();
		App a;
		Iterator<App> it = apps.iterator();
		while(it.hasNext()){
			a = it.next();
			if(a.getPrice()==0){
				free.add(a);
			}
		}
		//FIX: Actually sort list by score before returning
		return free;
		
	}
	
	public List<App> getTopPaidApps(String category) {
		List<App> paid = new ArrayList();
		App a;
		Iterator<App> it = apps.iterator();
		while(it.hasNext()){
			a = it.next();
			if(a.getPrice()!=0){
				paid.add(a);
			}
		}
		//FIX: Actually sort list by score before returning
		return paid;
	}
	
	public List<App> getMostRecentApps(String category) {
		List<App> recent = new ArrayList();
		App a;
		long newest = 0;
		Iterator<App> it = apps.iterator();
		while(it.hasNext()){
			a = it.next();
			if(a.getUploadTimestamp()>newest){
				recent.add(a);
			}
		}
                                //FIX: Actually sort
		return recent;
	}
}
