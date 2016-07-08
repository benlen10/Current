
//Main Class File:   Scheduler
//File:                  Event.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny


/**
 * Event represents events to be held in .
 */
public class Event implements Interval{
private long start;
private long end;
private String resource;
private String name;
private String organization;
private String description;
	
/**
 * Constructor for event object
 */
	Event(long start, long end, String name, String resource, String organization, String description){
		if((name==null)||(resource==null)||(organization==null)||(description==null)){
			throw new IllegalArgumentException();
		}
		this.start = start;
		this.end =end;
		this.name=name;
		this.resource=resource;
		this.organization = organization;
		this.description=description;
	}

	/**
	 * Return the start value
	 */
	@Override
	public long getStart(){
		return start;
	}

	
	/**
	 * Return the end value
	 */
	@Override
	public long getEnd(){
			return end;
	}
	
	/**
	 * Return the start organization
	 */
	public String getOrganization(){
			return organization;
	}

	/**
	 * Compare the start values of two Event objects
	 */
	@Override
	public int compareTo(Interval o) {
		if(start< o.getStart()){
			return -1;
		}
		else {
			return 1;
		}
	}
	
	/**
	 * Return true if the two start times are equal
	 */
	public boolean equals(Event e) {
		if(start == e.getStart()){
			return true;
		}
		else{
			return false;
		}
	}

	
	/**
	 * Return true if the two Event times overlap
	 */
	@Override
	public boolean overlap(Interval e) {
		if(((e.getStart()<=end)&&(e.getStart()>=start))||((e.getEnd()>=start)&&(e.getEnd()<=end))||((e.getStart()<=start)&&(e.getEnd()>=end))){  
			return true;
		}
		else{
			return false;
		}
	}
	
	/**
	 * Return string representation of the Event object
	 */
	public String toString(){
		StringBuilder sb = new StringBuilder();
		sb.append(name + "\n");
		sb.append(String.format("By: %s\n", organization));
		sb.append(String.format("In: %s\n", resource));
		sb.append(String.format("Start: %s\n", Scheduler.parseDate(start)));
		sb.append(String.format("End: %s\n", Scheduler.parseDate(end)));
		sb.append(String.format("Description: %s", description));
		return sb.toString();
	}
}
