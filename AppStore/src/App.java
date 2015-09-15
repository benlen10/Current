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
	private List<AppRating> ratings = new ArrayList();
	

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
		AppRating r = new AppRating(this,user,rating);
		ratings.add(r);
		
	}

	public long getTotalDownloads() {
		return downloadCount;
	}

	public double getAverageRating() {
		if(ratings.size()==0){
			return 0;
		}
		Iterator<AppRating> it = ratings.iterator();
		int total = 0;
		while(it.hasNext()){
			total =+ it.next().getRating();
		}
		return (total/ratings.size());

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


