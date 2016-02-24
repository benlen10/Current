package com.example.ben.unicade;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
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
    public static Console curConsole;
    public static MainActivity obj;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        dat = new Database();
        obj = this;
        populateGameList();
        FileOps.loadDefaultConsoles();
        populateConsoleList();

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
    }

    public void generateGameList(){
    }


    public void populateGameList(){
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

    public void saveDatabase(View v){
        FileOps.saveDatabase();
    }

    public void updateGameList(){
        ArrayList<String> games = new ArrayList<String>();
        for(Game g : curConsole.getGameList()){
            games.add(g.getTitle());
            System.out.println("LOOP");
        }
        ListView lv = (ListView)findViewById(R.id.listView);
        ArrayAdapter<String> myarrayAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, games);
        lv.setAdapter(myarrayAdapter);
        lv.setTextFilterEnabled(true);

    }

    public void populateConsoleList(){
        Spinner spinner = (Spinner) findViewById(R.id.spinner);
        //spinner.setOnItemSelectedListener(this);
        ArrayList<String> al = new ArrayList<String>();
        for(Console c : dat.consoleList){
            al.add(c.getName());
        }
        ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, al);


        dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        spinner.setAdapter(dataAdapter);
    }


}
