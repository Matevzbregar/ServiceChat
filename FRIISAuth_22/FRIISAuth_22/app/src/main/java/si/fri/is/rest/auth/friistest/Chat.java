package si.fri.is.rest.auth.friistest;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.provider.Settings;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

public class Chat extends AppCompatActivity {

    private String username;
    private String password;
    private int idMessage;
    private ListView list;
    private ArrayList<String> msgs;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_chat2);

        //disablemsg((EditText) findViewById(R.id.msg));

        Intent myIntent = getIntent();

        username = myIntent.getStringExtra("username");
        password = myIntent.getStringExtra("password");

        TextView info = (TextView) findViewById(R.id.prijav);
        info.setText("Prijavljen uporabnik: " + username);



        Button btnUpdate = (Button) findViewById(R.id.osvezi);
        btnUpdate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                RESTTaskUpdate update = new RESTTaskUpdate();
                update.execute(String.valueOf(idMessage+1));
            }
        });

        Button btnSend = (Button) findViewById(R.id.poslji);
        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                EditText et = (EditText) findViewById(R.id.sporocilo);
                String msg = (et.getText().toString());

                RESTTaskSend Send = new RESTTaskSend();
                Send.execute(String.valueOf(msg));

                et.getText().clear();

                Activity activity = Chat.this;
                InputMethodManager inputMethodManager = (InputMethodManager) activity.getSystemService(Activity.INPUT_METHOD_SERVICE);
                inputMethodManager.hideSoftInputFromWindow(activity.getCurrentFocus().getWindowToken(), 0);

                RESTTaskUpdate update = new RESTTaskUpdate();
                update.execute(String.valueOf(idMessage+1));
            }
        });

        list = (ListView) findViewById(R.id.msg);


        Chat.RESTTask restTask = new Chat.RESTTask();
        restTask.execute();

    }

    private void updateList() {
        list.setAdapter(new ArrayAdapter(this,  android.R.layout.simple_list_item_1, msgs));
    }


    private void disablemsg(EditText editText) {
        editText.setFocusable(false);
        editText.setEnabled(false);
        editText.setCursorVisible(false);
        editText.setKeyListener(null);
    }


    private class RESTTask extends AsyncTask<Void, Void, ArrayList<String>> {
        private static final String SERVICE_URL2 = "http://localhost:61233/Service1.svc/Messages";
        private static final String SERVICE_URL = "http://chatt.somee.com/Service1.svc/Messages";


        @Override
        protected ArrayList<String> doInBackground(Void... params) {
            DefaultHttpClient hc = new DefaultHttpClient();
            msgs = new ArrayList<>();


            try {
                HttpPost request = new HttpPost(SERVICE_URL);
                request.setHeader("Accept", "application/json");
                request.setHeader("Authorization", username + ":" + password);
                String webResult = EntityUtils.toString(hc.execute(request).getEntity());

                JSONArray jsonArray = new JSONArray(webResult);
                for (int i = 0; i < jsonArray.length(); i++) {
                    JSONObject jsonObject = jsonArray.getJSONObject(i);
                    /*result += String.format("%s %s: %s\n",
                            jsonObject.getString("Username"),
                            jsonObject.getString("Time"),
                            jsonObject.getString("Text"));*/

                    msgs.add(String.format("%s %s: %s\n",
                            jsonObject.getString("Username"),
                            jsonObject.getString("Time"),
                            jsonObject.getString("Text")));

                    idMessage = jsonObject.getInt("Id");

                }
            } catch (Exception e) {
                e.printStackTrace();
            }

            return msgs;
        }

        @Override
        protected void onPostExecute(ArrayList result) {
            updateList();
            //((TextView)findViewById(R.id.msg)).setText(result);
        }
    }

    private class RESTTaskUpdate extends AsyncTask<String, Void, ArrayList<String>> {
        private static final String SERVICE_URL = "http://chatt.somee.com/Service1.svc/Messages/";
        private static final String SERVICE_URL2 = "http://localhost:61233/Service1.svc/Messages/";

        @Override
        protected ArrayList<String> doInBackground(String... params) {
            DefaultHttpClient hc = new DefaultHttpClient();
            //msgs = new ArrayList<>();
            int idMsg = Integer.parseInt(params[0]);

            try {
                HttpGet request = new HttpGet(SERVICE_URL+idMsg);
                request.setHeader("Accept", "application/json");
                request.setHeader("Authorization", username + ":" + password);
                String webResult = EntityUtils.toString(hc.execute(request).getEntity());

                JSONArray jsonArray = new JSONArray(webResult);
                for (int i = 0; i < jsonArray.length(); i++) {
                    JSONObject jsonObject = jsonArray.getJSONObject(i);
                    msgs.add(String.format("%s %s: %s\n",
                            jsonObject.getString("Username"),
                            jsonObject.getString("Time"),
                            jsonObject.getString("Text")));


                    idMessage = jsonObject.getInt("Id");

                }
            } catch (Exception e) {
                e.printStackTrace();
            }

            return msgs;
        }

        @Override
        protected void onPostExecute(ArrayList<String> result) {
            updateList();
        }
    }

    private class RESTTaskSend extends AsyncTask<String, Void, Void> {
        private static final String SERVICE_URL = "http://chatt.somee.com/Service1.svc/Send/";

        @Override
        protected Void doInBackground(String... params) {
            DefaultHttpClient hc = new DefaultHttpClient();
            //msgs = new ArrayList<>();
            String idMsg = params[0];
            try {
                HttpPost request = new HttpPost(SERVICE_URL+idMsg);
                request.setHeader("Accept", "application/json");
                request.setHeader("Authorization", username + ":" + password);
                hc.execute(request);

            } catch (Exception e) {
                e.printStackTrace();
            }
            return null;
        }
    }

}
