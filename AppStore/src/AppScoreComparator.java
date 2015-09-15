import java.util.Comparator;

public class AppScoreComparator implements Comparator<App> {
	
	@Override
	public int compare(App app1, App app2) {
		if(app1.getAppScore()>app2.getAppScore()){
			return 1;
		}
		else{
			return -1;
		}
	}

}
