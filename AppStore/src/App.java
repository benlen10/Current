
import java.util.*;
public class App implements Comparable<App> {
	
	//Constructor data members
	private User developer;
	private String appId;
	private String appName;
	private String category;
	private double price;
	private long uploadTimestamp;
	//Custom data members
	private long downloadCount;
	private ArrayList<Short> ratings1 = new ArrayList<Short>();
	private ArrayList<User> ratings2 = new ArrayList<User>();
	

	public App(User developer, String appId, String appName, String category,
			double price, long uploadTimestamp) throws IllegalArgumentException {
		this.developer=developer;
		this.appId=appId;
		this.appName=appName;
		this.category=category;
		this.price = price;
		this.uploadTimestamp = uploadTimestamp;
	}

	public User getDeveloper() {
		return developer;
	}

	public String getAppId() {
		return appId;
	}

	public String getAppName() {
		return appName;
	}

	public String getCategory() {
		return category;
	}

	public double getPrice() {
		return price;
	}

	public long getUploadTimestamp() {
		return uploadTimestamp;
	}

	public void download(User user) {		
		downloadCount++;
		System.out.println("App downloaded");
	}

	public void rate(User user, short rating) throws IllegalArgumentException {
		ratings1.add(rating);
		ratings2.add(user);
	}

	public long getTotalDownloads() {
		return downloadCount;
	}

	public double getAverageRating() {
		Iterator<Short> it1 = ratings1.iterator();
		int total = 0;
		while(it1.hasNext()){
			total =+ it1.next();
		}
		return (total/ratings1.size());

	}
	
	public double getRevenueForApp() {
		return (downloadCount * price);
	}


	public double getAppScore() {
		return (getRevenueForApp()*downloadCount);
	}

	@Override
	public int compareTo(App otherApp) {
		if(uploadTimestamp > otherApp.getUploadTimestamp()){
			return 1;
		}
		else{
			return -1;
		}
		}
	}


