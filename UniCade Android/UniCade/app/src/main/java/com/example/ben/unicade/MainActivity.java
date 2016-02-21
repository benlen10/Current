package com.example.ben.unicade;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.ArrayAdapter;

import java.util.ArrayList;


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
    

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        populateList();
    }

    public void generateGameList(){
    }


    public void populateList(){
        ArrayList a = new ArrayList();
        a.add("Game 1");
        a.add(("Game 2"));
        a.add(("Game 2"));
        a.add(("Game 2"));
        a.add(("Game 2"));
        a.add(("Game 2"));
        a.add(("Game 2"));
        ListView lv = (ListView)findViewById(R.id.listView);
        ArrayAdapter<String> myarrayAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, a);
        lv.setAdapter(myarrayAdapter);
        lv.setTextFilterEnabled(true);
    }
}
