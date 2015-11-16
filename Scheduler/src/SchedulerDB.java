import java.text.Format;
import java.text.SimpleDateFormat;
import java.util.*;



public class SchedulerDB {
	public List<Resource> resources;
	
	SchedulerDB(){
		resources = new ArrayList<Resource>();
	}
	
	public boolean addResource(String resource){
		if(resource!=null){                                               //Check for dup
		resources.add(new Resource(resource));
		return true;
		}
		else{
			return false;
		}
	}
	
	public boolean removeResource(String r){
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(r)){
				resources.remove(res);
				return true;
			}
		}
		return false;
	}
	
	public boolean addEvent(String r, long start, long end, String name, 
			String organization, String description){
		Event e = new Event(start, end,  name,r ,organization, description);
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(r)){
				res.addEvent(e);
				return true;
			}
		}
		return false;
	}
	
	public boolean deleteEvent(long start, String resource){
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(resource)){
						return res.deleteEvent(start);
					}
		}
		return false;
	}
	
	public Resource findResource(String r){
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(r)){
				return res;
			}
		}
		return null;
	}
	
	public List<Resource> getResources(){
		return resources;
	}
	
	public List<Event> getEventsInReource(String resource){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(resource)){
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					events.add(it2.next());
				}
			}
		}
		return events;
	}
	
	public List<Event> getEventsInRange(long start, long end){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event e = it2.next();
					if((e.getStart()>=start)&&(e.getEnd()<=end)){
					events.add(e);
					}
				}
		}
		return events;
	}	
	
	public List<Event> getEventsInRangeInReource(long start, long end, String resource){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(resource)){
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event e = it2.next();
					if((e.getStart()>=start)&&(e.getEnd()<=end)){
					events.add(e);
					}
				}
			}
		}
		return events;
	}
	
	public List<Event> getAllEvents(){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event e = it2.next();
					events.add(e);
				}
		}
		return events;
	}
	
	public List<Event> getEventsForOrg(String org){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event e = it2.next();
					if(e.getOrganization().equals(org)){
					events.add(e);
					}
				}
		}
		return events;
	}
}
