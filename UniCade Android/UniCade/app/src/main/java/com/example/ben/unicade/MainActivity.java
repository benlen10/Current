package com.example.ben.unicade;

import android.app.Fragment;
import android.app.FragmentManager;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;

import java.io.File;
import java.util.ArrayList;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.TextView;
import android.widget.ImageView;
import android.app.FragmentTransaction;


public class MainActivity extends AppCompatActivity {

    public static Database dat;
    public static String databasePath = "C:\\UniCade\\Databse.txt";
    public static String romPath = "C:\\UniCade\\ROMS";
    public static String mediaPath = "C:\\UniCade\\Media";
    public static String emuPath = "C:\\UniCade\\Emulators";
    public static String prefPath = "C:\\UniCade\\Preferences.txt";
    public static User curUser;
    public static int coins = 0;
    public static boolean playtimeRemaining = true;
    public static String userLicenseName;
    public static String userLicenseKey;
    public static boolean validLicense;
    public static Console curConsole;
    public static Game curGame;
    public static MainActivity obj;
    Spinner spinner;
    public static TextView t1;
    public static TextView t2;
    public static TextView t3;
    public static TextView t4;  //Title
    public static TextView t6;
    public static TextView t7;
    public static ImageView i1;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        dat = new Database();
        obj = this;
         spinner = (Spinner) findViewById(R.id.spinner);
         t1 = (TextView) findViewById(R.id.textView);
        t2 = (TextView) findViewById(R.id.textView2);
        t3 = (TextView) findViewById(R.id.textView3);
        t4 = (TextView) findViewById(R.id.textView4);

        t6 = (TextView) findViewById(R.id.textView6);
        t7 = (TextView) findViewById(R.id.textView7);
        i1 = (ImageView) findViewById(R.id.imageView);
        final Spinner spinner = (Spinner) findViewById(R.id.spinner);
        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id) {
                String con = spinner.getSelectedItem().toString();
                for(Console c : dat.consoleList){
                    if(c.getName().equals(con)){
                        curConsole = c;
                        break;
                    }
                }
                updateGameList();

            }

            @Override
            public void onNothingSelected(AdapterView<?> parentView) {
                // your code here
            }

        });
        final ListView listView = (ListView) findViewById(R.id.listView);
        listView.setOnItemClickListener(new OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position,
                                    long id) {          //Game Clicked

                    String item = (String) (listView.getItemAtPosition(position));
                    for (Game g : curConsole.getGameList()) {
                        if (g.getTitle().equals(item)) {
                            curGame = g;
                            t4.setText(("Title: " + g.getTitle()));
                            t1.setText(("Release Date"+ g.getReleaseDate()));
                            t2.setText(("Publisher: " + g.getPublisher()));
                            t3.setText(("ESRB Rating: " + g.getEsrb()));


                            if (g.getEsrb().equals("Everyone"))
                            {
                                i1.setImageResource(R.drawable.everyone);
                            }
                            else if (g.getEsrb().equals("Everyone 10+"))
                            {
                                i1.setImageResource(R.drawable.everyone10);
                            }
                            else if (g.getEsrb().equals("Teen"))
                            {
                                i1.setImageResource(R.drawable.teen);
                            }
                            else if (g.getEsrb().equals("Mature"))
                            {
                                i1.setImageResource(R.drawable.mature);
                            }
                            else if (g.getEsrb().equals("Adults Only (AO)"))
                            {
                                i1.setImageResource(R.drawable.ao);;
                            }
                            else{
                                i1.setImageResource(0);
                            }

                        }
                    }




            }
        });
        FileOps.loadDatabase();
    }




    public void saveDatabase(View v){
        FileOps.saveDatabase();
    }

    public void loadDatabase(View v){
        FileOps.loadDatabase();
    }

    public void launchSettings(View view){
        startActivity(new Intent(getApplicationContext(), SettingsWindow.class));
    }

    public void launchInfoWindow(View view){
        startActivity(new Intent(getApplicationContext(), DetailedInfo.class));
    }



    public void updateGameList(){
        ArrayList<String> games = new ArrayList<String>();
        for(Game g : curConsole.getGameList()){
            games.add(g.getTitle());
        }
        ListView lv = (ListView)findViewById(R.id.listView);
        ArrayAdapter<String> myarrayAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, games);
        lv.setAdapter(myarrayAdapter);
        lv.setTextFilterEnabled(true);
        t7.setText("Current Console Games: " + curConsole.gameCount);
        t4.setText(("Title:"));
        t1.setText(("Release Date:" ));
        t2.setText(("Publisher:" ));
        t3.setText(("ESRB Rating:"));
        i1.setImageResource(0);

    }

    public void populateConsoleList(){

        //spinner.setOnItemSelectedListener(this);
        ArrayList<String> al = new ArrayList<String>();
        for(Console c : dat.consoleList){
            al.add(c.getName());
            for(Game g : c.getGameList()){
                dat.totalGameCount++;
            }
        }
        t6.setText("Total Game Count: " + dat.totalGameCount);
        ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, al);


        dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        spinner.setAdapter(dataAdapter);
    }




}
