package si.fri.is.rest.auth.friistest;

import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;
import android.widget.Toast;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONObject;

public class LoginActivity extends AppCompatActivity {

    private String username;
    private String password;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Intent myIntent = getIntent();
        username = myIntent.getStringExtra("username");
        password = myIntent.getStringExtra("password");

        RESTTask restTask = new RESTTask();
        restTask.execute();
    }

    private class RESTTask extends AsyncTask<Void, Void, String> {
        private static final String SERVICE_URL = "http://chatt.somee.com/Service1.svc/Login";

        @Override
        protected String doInBackground(Void... params) {
            DefaultHttpClient hc = new DefaultHttpClient();
            String result = "";

            try {
                HttpPost request = new HttpPost(SERVICE_URL);
                request.setHeader("Accept", "application/json");
                request.setHeader("Authorization", username + ":" + password);
                String webResult = EntityUtils.toString(hc.execute(request).getEntity());
                result = webResult;
            } catch (Exception e) {
                e.printStackTrace();
            }

            return result;
        }

        @Override
        protected void onPostExecute(String result) {
            if(result.equals("true")){
                Intent intent = new Intent(LoginActivity.this, Chat.class);
                intent.putExtra("username", username);
                intent.putExtra("password", password);
                Toast.makeText(getBaseContext(), "Prijava je bila uspešna", Toast.LENGTH_SHORT).show();
                startActivity(intent);
            } else {
                Intent intent = new Intent(LoginActivity.this, Login.class);
                Toast.makeText(getBaseContext(), "Napačno uporabniško ime ali geslo", Toast.LENGTH_SHORT).show();
                startActivity(intent);
            }
        }
    }
}
