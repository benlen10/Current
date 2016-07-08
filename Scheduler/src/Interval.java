//Main Class File:   Scheduler
//File:                  Interval.java
//Semester:          Fall 2015

//Author:         Ben Leninton
//Email:           lenington@wisc.edu
//CS Login:      lenington
//Lecturer's Name:  Jim Skrentny

/**
	Outlines the structure of the Interval class
*/


public interface Interval extends Comparable<Interval>{
	/**
     * Returns the start of the interval.
     * @return the start
     */
	long getStart();
	
	/**
     * Returns the end of the interval.
     * @return the end
     */
	long getEnd();
	
	/**
     * Returns whether there is an overlap between the two intervals.
     * @return if there is an overlap between the intervals.
     */
	boolean overlap(Interval otherInterval);
}
