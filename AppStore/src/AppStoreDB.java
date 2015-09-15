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
		
		User u;
		Iterator<User> it = users.iterator();
		while(it.hasNext()){
			u = it.next();
			if(u.getEmail().equals(email)){
				System.err.println("User already exists in database");   //Custom error message
				throw new IllegalArgumentException();
			}
		}
		
		u = new User(email,password,firstName,
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
		                                   //Check for dup app id
		App ap;
		Iterator<App> it = apps.iterator();
			while(it.hasNext()){
				ap = it.next();
				if(ap.getAppId().equals(appId)){
					System.err.println("App Id already exists");
					throw new IllegalArgumentException();
				}
			}
		
		App a = new App(uploader, appId, appName, category,
				 price, timestamp);
		apps.add(a);
		uploader.upload(a);
		return a;

	}
	
	public void downloadApp(User user, App app) {	
		if(hasUserDownloadedApp(user,app)){
			System.err.println("User has already downloaded this app");
			return;
		}
		else{
		app.download(user);
		user.download(app);
		}
	}
	
	public void rateApp(User user, App app, short rating) {
		if (hasUserDownloadedApp(user, app)){
		app.rate(user, rating);
		}
		else{
			System.err.println("User has not downloaded app");
			return;
		}
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
		if(category==null){
			while(it.hasNext()){
				a = it.next();
				if(a.getPrice()==0){
					free.add(a);
				}
			}	
		}
		else{
		while(it.hasNext()){
			a = it.next();
			if((a.getPrice()==0)&&(a.getCategory().equals(category))){
				free.add(a);
			}
		}
		}
		
		App a1;
		App a2;
		App tmp;
		int i = 0;
		boolean status = true;
		boolean trigger = false;
		while(status==true){
			trigger=false;
			i = 0;
			while(i<(free.size()-1)){
			a1 = free.get(i);
			a2 = free.get(i+1);
			if(a1.getAppScore()<a2.getAppScore()){
				tmp = a1;
				a1=a2;
				a2=tmp;
				trigger = true;
			}
			i++;
			}
			if(trigger==false){
				status=false;
			}
		}
		
		return free;
		
	}
	
	public List<App> getTopPaidApps(String category) {
		List<App> paid = new ArrayList();
		App a;
		Iterator<App> it = apps.iterator();
		if(category == null){
			while(it.hasNext()){
				a = it.next();
				if(a.getPrice()!=0){
					paid.add(a);
				}
			}	
		}
		else{
		while(it.hasNext()){
			a = it.next();
			if((a.getPrice()!=0)&&(a.getCategory().equals(category))){
				paid.add(a);
			}
		}
		}
		App a1;
		App a2;
		App tmp;
		int i = 0;
		boolean status = true;
		boolean trigger = false;
		while(status==true){
			trigger=false;
			i = 0;
			while(i<(paid.size()-1)){
			a1 = paid.get(i);
			a2 = paid.get(i+1);
			if(a1.getAppScore()<a2.getAppScore()){
				tmp = a1;
				a1=a2;
				a2=tmp;
				trigger = true;
			}
			i++;
			}
			if(trigger==false){
				status=false;
			}
		}
		return paid;
	}
	
	public List<App> getMostRecentApps(String category) {
		List<App> recent = new ArrayList();
		App a;
		Iterator<App> it = apps.iterator();
		if(category == null){
			while(it.hasNext()){
				a = it.next();
				recent.add(a);
			}	
		}
		else{
		while(it.hasNext()){
			a = it.next();
			if(a.getCategory().equals(category)){
				recent.add(a);
			}
		}
		}

		App a1;
		App a2;
		App tmp;
		int i = 0;
		boolean status = true;
		boolean trigger = false;
		while(status==true){
			System.out.printf("Loop(Outer)");
			trigger=false;
			i = 0;
			while(i<(recent.size()-1)){
				System.out.printf("Loop(inter)");
			a1 = recent.get(i);
			a2 = recent.get(i+1);
			if(a1.getUploadTimestamp()<a2.getUploadTimestamp()){
				tmp = a1;
				recent.set(i, a2);
				recent.set((i+1), tmp);
				trigger = true;
			}
			i++;
			}
			if(trigger==false){
				status=false;
			}
		}                     
		return recent;
	}
}
