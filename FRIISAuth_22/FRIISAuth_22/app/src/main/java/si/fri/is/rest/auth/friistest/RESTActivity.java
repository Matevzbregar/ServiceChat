package si.fri.is.rest.auth.friistest;

import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONObject;

public class RESTActivity extends AppCompatActivity {

    private String username;
    private String password;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_rest);

        Intent myIntent = getIntent();
        username = myIntent.getStringExtra("username");
        password = myIntent.getStringExtra("password");

        TextView info = (TextView) findViewById(R.id.info);
        info.setText("Prijavljen uporabnik: " + username);

        RESTTask restTask = new RESTTask();
        restTask.execute();
    }

    private class RESTTask extends AsyncTask<Void, Void, String> {
        private static final String SERVICE_URL = "http://chatt.somee.com/Service1.svc/Messages";


        @Override
        protected String doInBackground(Void... params) {
            DefaultHttpClient hc = new DefaultHttpClient();
            String result = "";

            try {
                HttpPost request = new HttpPost(SERVICE_URL);
                request.setHeader("Accept", "application/json");
                request.setHeader("Authorization", username + ":" + password);
                String webResult = EntityUtils.toString(hc.execute(request).getEntity());

                JSONArray jsonArray = new JSONArray(webResult);
                for (int i = 0; i < jsonArray.length(); i++) {
                    JSONObject jsonObject = jsonArray.getJSONObject(i);
                    result += String.format("%s (%s): %s\n",
                            jsonObject.getString("Username"),
                            jsonObject.getString("Time"),
                            jsonObject.getString("Text"));
                }
            } catch (Exception e) {
                e.printStackTrace();
            }

            return result;
        }

        @Override
        protected void onPostExecute(String result) {
            ((TextView)findViewById(R.id.result)).setText(result);
        }
    }
}
