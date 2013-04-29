import java.util.LinkedHashMap;
import java.util.Map;
import java.util.ArrayList;

public class Msg {
    
    ArrayList<LinkedHashMap> recipients = new ArrayList<LinkedHashMap>();
    LinkedHashMap message = new LinkedHashMap();
        
    public void setHtmlBody(String s) {
        message.put("HtmlBody", s);
    }
    
    public void setSubject(String s) {
        message.put("Subject", s);
    }
    
    public void setFrom(String f, String e) {
        Map from = new LinkedHashMap();
        from.put("EmailAddress", e);
        from.put("FriendlyName", f);
        message.put("From", from);
    }
    
    public void addRecipient(String fn, String em) {
        LinkedHashMap recipient = new LinkedHashMap();
        recipient.put("FriendlyName", fn);
        recipient.put("EmailAddress", em);
        recipients.add(recipient);
    }
}