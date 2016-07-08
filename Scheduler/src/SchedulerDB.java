import java.text.Format;
import java.text.SimpleDateFormat;
import java.util.*;

//Main Class File:   Scheduler
//File:                  SchedulerDB.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

/**
	Creates a database to store the essential data for the program including a list of resources. 
	This class also provides a variety of functions to manipulate the database
*/


public class SchedulerDB {
	public List<Resource> resources;
	
	/**
	*		Create a new databse object
	*/
	SchedulerDB(){
		resources = new ArrayList<Resource>();
	}
	
	/**
	*		Add a resource to the database
	*/
	public boolean addResource(String resource){
		if(resource!=null){                                               //Check for dup
		resources.add(new Resource(resource));
		return true;
		}
		else{
			return false;
		}
	}
	
	/**
	*		 Remove a resource from the database
	*/
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
	
	/**
	*		Add an event to the database
	*/
	public boolean addEvent(String r, long start, long end, String name, 
			String organization, String description){
		Event e = new Event(start, end,  name,r ,organization, description);
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(r)){
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event ev =it2.next();
					if(ev.overlap(e)){
						throw new IntervalConflictException();
					}
				}
				res.addEvent(e);
				return true;
			}
		}
		return false;
	}
	
	/**
	*		Delete an event from the database
	*/
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
	
	/**
	*		Find and return the specified Resource object
	*/
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
	
	/**
	*		Return a list of all available resources
	*/
	public List<Resource> getResources(){
		return resources;
	}
	
	/**
	*		Return a list of all events in the resource
	*/
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
	
	/**
	*		Return a list of all events in the time range
	*/
	public List<Event> getEventsInRange(long start, long end){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event e = it2.next();
				
					if(((e.getStart()<=end)&&(e.getStart()>=start))||((e.getEnd()>=start)&&(e.getEnd()<=end))||((e.getStart()<=start)&&(e.getEnd()>=end))){  
					
					events.add(e);
					}
				}
		}
		return events;
	}	
	
	/**
	*		Return a list of all the events in the time range and specified resource
	*/
	public List<Event> getEventsInRangeInReource(long start, long end, String resource){
		List<Event> events = new ArrayList<Event>();
		Iterator<Resource> it = resources.iterator();
		while(it.hasNext()){
			Resource res =it.next();
			if(res.getName().equals(resource)){
				Iterator<Event> it2 = res.iterator();
				while(it2.hasNext()){
					Event e = it2.next();
					if(((e.getStart()<=end)&&(e.getStart()>=start))||((e.getEnd()>=start)&&(e.getEnd()<=end))||((e.getStart()<=start)&&(e.getEnd()>=end))){  
					events.add(e);
					}
				}
			}
		}
		return events;
	}
	
	
	/**
	*		Return a list of all events
	*/
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
	
	/**
	*		Return a list of all events for a specific org 
	*/
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
