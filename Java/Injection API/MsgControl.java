import com.google.gson.Gson;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.JSONArray;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.ArrayList;
import java.io.FileWriter;
import java.io.IOException;
import java.net.URL;
import java.net.HttpURLConnection;
import java.net.ProtocolException;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.InputStream;
import java.io.BufferedReader;
import java.io.InputStreamReader;

public class MsgControl {

    Gson gson = new Gson();
    Map obj = new LinkedHashMap();
    boolean printStackTrace = false;
    String errorCode = null;
    ArrayList<String> messageResults;
    String transactionReciept = null;
    String apiEndpointUrl = null;

    public void setApiKey(String s) {
        obj.put("ApiKey", s);
    }
    public void setUrl(String s) {
    	apiEndpointUrl = s;
    }

    public void setServerId(String s) {
        obj.put("ServerID", s);
    }

    public boolean send(Msg m) {
    	m.message.put("To", m.recipients);
    	ArrayList messages = new ArrayList();
    	messages.add(m.message);
    	obj.put("Messages", messages);
    	String json;
    	json = gson.toJson(obj);
    	System.out.println(json);
    	StringBuilder response = new StringBuilder();
        HttpURLConnection conn = null;
        try {
            URL url = new URL(apiEndpointUrl);
            conn = (HttpURLConnection) url.openConnection();
            try {
                conn.setRequestMethod("POST");
                conn.setDoOutput(true);
                conn.setDoInput(true);
                conn.setUseCaches(false);
                conn.setAllowUserInteraction(false);
                conn.setRequestProperty("Content-Type", "application/json");
            } catch (ProtocolException e) {
                if (printStackTrace == true) {
                    e.printStackTrace();
                }
                return false;
            }
            OutputStream out = conn.getOutputStream();
            OutputStreamWriter wr = null;
            try {
                wr = new OutputStreamWriter(out);
                wr.write(json);
            } catch (IOException e) {
                if (printStackTrace == true) {
                    e.printStackTrace();
                }
                return false;
            } finally {
                if (wr != null) {
                    wr.close();
                }
            }
            InputStream in = conn.getInputStream();
            BufferedReader rd = null;
            try {
                rd = new BufferedReader(new InputStreamReader(in));
                String responseSingle = null;
                while ((responseSingle = rd.readLine()) != null) {
                    response.append(responseSingle);
                }
            } catch (IOException e) {
                if (printStackTrace == true) {
                    e.printStackTrace();
                }
                return false;
            } finally {
                if (rd != null) {
                    rd.close();
                }
            }
        } catch (IOException e) {
            if (printStackTrace == true) {
                e.printStackTrace();
            }
            return false;
        } finally {
            if (conn != null) {
                conn.disconnect();
            }
        }

        JSONObject results = null;
        try {
            results = (JSONObject) new JSONParser().parse(response.toString());
            errorCode = results.get("ErrorCode").toString();
            messageResults = new ArrayList<String>();
            JSONArray mr = (JSONArray)results.get("MessageResults");
            int l = mr.size();
            for (int c = 0; c < l; c++) {
                messageResults.add(mr.get(c).toString());
            }
            transactionReciept = results.get("TransactionReciept").toString();
        } catch (Exception e) {
        }
        return true;

    }

    public void printStackTrace(boolean b) {
        printStackTrace = b;
    }

    public String getErrorCode() {
        return errorCode;
    }

    public ArrayList<String> getMessageResults() {
        return messageResults;
    }

    public String getTransactionReciept() {
        return transactionReciept;
    }
}