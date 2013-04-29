/*      Note: In order for this to function, you need to include the 'gson' and
 *      'json.simple' libraries.  They can be found at:
 * 
 *          gson:   https://code.google.com/p/google-gson/downloads/list
 *          json.simple:    https://code.google.com/p/json-simple/
 */

public class MsgMain {
    public static void main(String args[]) {

    	MsgControl c = new MsgControl();        
        Msg m = new Msg();
        
        m.setFrom("Sender Friendly Name","sender@example.com");
        m.setSubject("Testing...");
        m.setHtmlBody("<h1>The html portion of the message</h1><p>Test paragraph.</p>");
        m.addRecipient("Recipient Friendly Name","recipient@example.com");
               
        c.setServerId("YOUR SERVER ID HERE");
        c.setApiKey("YOUR SERVER API KEY HERE");        
        c.setUrl("https://inject.socketlabs.com/api/v1/email/");        
        c.printStackTrace(true);
        
        if (c.send(m)) {
          System.out.println("Error Code:\t" + c.getErrorCode());
          System.out.println("Message Results:\t" + c.getMessageResults());
          System.out.println("Transaction Reciept:\t" + c.getTransactionReciept());
        }        
        else {
            System.out.println("Connection error.");
        } 
    }
}