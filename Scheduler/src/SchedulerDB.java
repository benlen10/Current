import java.text.Format;
import java.text.SimpleDateFormat;
import java.util.*;



public class SchedulerDB {
	//TODO add private data members
	
	SchedulerDB(){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("Constructor not implemented.");
	}
	
	public boolean addResource(String resource){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("addResource() not implemented.");
	}
	
	public boolean removeResource(String r){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("removeResource() not implemented.");
	}
	
	public boolean addEvent(String r, long start, long end, String name, 
			String organization, String description){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("addEvent() not implemented.");
	}
	
	public boolean deleteEvent(long start, String resource){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("deleteEvent() not implemented.");
	}
	
	public Resource findResource(String r){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("findResource() not implemented.");
	}
	
	public List<Resource> getResources(){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("getResources() not implemented.");
	}
	
	public List<Event> getEventsInReource(String resource){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("getEventsInReource() not implemented.");
	}
	
	public List<Event> getEventsInRange(long start, long end){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("getEventsInRange() not implemented.");
	}	
	
	public List<Event> getEventsInRangeInReource(long start, long end, String resource){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("getEventsInRangeInReource() not implemented.");
	}
	
	public List<Event> getAllEvents(){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("getAllEvents() not implemented.");
	}
	
	public List<Event> getEventsForOrg(String org){
		//TODO Remove this exception and implement the method
		throw new RuntimeException("getEventsForOrg() not implemented.");
	}
}
