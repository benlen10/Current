
public class MalformedEmailException extends Exception{
static int malformedCount;

	MalformedEmailException(){       //Default constructor  
		malformedCount++;
	}
}
