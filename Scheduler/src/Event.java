

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
	
	Event(long start, long end, String name, String resource, String organization, String description){
		this.start = start;
		this.end =end;
		this.name=name;
		this.resource=resource;
		this.organization = organization;
		this.description=description;
	}

	@Override
	public long getStart(){
		return start;
	}

	@Override
	public long getEnd(){
			return end;
	}
	
	public String getOrganization(){
			return organization;
	}

	@Override
	public int compareTo(Interval o) {
		if(start< o.getStart()){
			return -1;
		}
		else {
			return 1;
		}
	}
	
	public boolean equals(Event e) {
		if(start == e.getStart()){
			return true;
		}
		else{
			return false;
		}
	}

	@Override
	public boolean overlap(Interval e) {
		if((e.getStart()>start)&&(e.getStart()<end)){
			return true;
		}
		else if((start>e.getStart())&&(start<e.getEnd())){
			return true;
		}
		else{
			return false;
		}
	}
	
	public String toString(){
		StringBuilder sb = new StringBuilder();
		sb.append(name);
		sb.append(String.format("By: %s", organization));
		sb.append(String.format("In: %s", resource));
		sb.append(String.format("Start: %d", start));
		sb.append(String.format("End: %d", end));
		sb.append(String.format("Description: %s", description));
		return sb.toString();
	}
}
