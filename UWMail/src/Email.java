import java.util.Date;
import java.text.SimpleDateFormat;

public class Email {
	private Date date;
	private String messageID;
	private String subject;
	private String from;
	private String to;
	private String inReplyTo;
	private ListADT<String> body;
	private ListADT<String>references;
	

  public Email(Date date, String messageID, String subject, String from, String to, ListADT<String> body) {
	  this.date=date;
	  this.messageID=messageID;
	  this.subject=subject;
	  this.from=from;
	  this.to=to;
	  this.body=body;
  }

  public Email(Date date, String messageID, String subject, String from, String to, ListADT<String> body, String inReplyTo, ListADT<String> references) {
	  this.date=date;
	  this.messageID=messageID;
	  this.subject=subject;
	  this.from=from;
	  this.to=to;
	  this.body=body;
	  this.inReplyTo=inReplyTo;
	  this.references = references;
  }

  public Date getDate() {
    return date;
  }

  public String getMessageID() {
    return messageID;
  }

  public String getSubject() {
    return subject;
  }

  public String getFrom() {
    return from;
  }

  public String getTo() {
    return to;
  }

  public ListADT<String> getBody() {
    return body;
  }

  public String getInReplyTo() {
    return inReplyTo; 
  }

  public ListADT<String> getReferences() {
    return references;
  }
} 
