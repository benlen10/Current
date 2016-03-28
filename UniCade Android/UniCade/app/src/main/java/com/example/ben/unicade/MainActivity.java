package com.example.ben.unicade;

import android.app.AlertDialog;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.InputType;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.CheckBox;
import android.widget.EditText;
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

    public static TextView t4;  //Title
    public static TextView t6;
    public static TextView t7;
    public static ImageView i1;
    public static ImageView i7;
    public static CheckBox c6;
    public static EditText e2;
    final Context context = this;
    public static String conImage = "";
    private boolean displayUsers = false;
    private boolean displayAllGames = false;
    private boolean globalSearch = false;
    private boolean searchInProgress = false;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        dat = new Database();
        obj = this;
         spinner = (Spinner) findViewById(R.id.spinner);
         t1 = (TextView) findViewById(R.id.textView);
        t4 = (TextView) findViewById(R.id.textView4);

        t6 = (TextView) findViewById(R.id.textView6);
        t7 = (TextView) findViewById(R.id.textView7);
        i1 = (ImageView) findViewById(R.id.imageView);
        i7 = (ImageView) findViewById(R.id.imageView7);
        e2 = (EditText)  findViewById(R.id.editText2);
        c6 = (CheckBox) findViewById(R.id.checkBox6);
        t4.setHorizontallyScrolling(true);
        final Spinner spinner = (Spinner) findViewById(R.id.spinner);
        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id) {  //Console selection changed
                String con = spinner.getSelectedItem().toString();
                if(con.equals("Users")){
                        displayUsers = true;
                }
                else if(con.equals("All Games")){

                            displayAllGames = true;
                    globalSearch = true;
                    curConsole = null;

                }
                else {
                    displayAllGames = false;
                    displayUsers = false;
                    for (Console c : dat.consoleList) {
                        if (c.getName().equals(con)) {
                            curConsole = c;
                            break;
                        }
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
                    updateBasicGameInfo(item);


            }
        });
        FileOps.loadDatabase("Database.txt");
    }







    public void launchSettings(View view){
        startActivity(new Intent(getApplicationContext(), SettingsWindow.class));
    }

    public void launchInfoWindow(View view){
        if(curGame==null){
            showPopup("Error", "No Game Selected");
            return;
        }
        startActivity(new Intent(getApplicationContext(), DetailedInfo.class));
    }

    public void launchLogin(View view){
        startActivity(new Intent(getApplicationContext(), LoginActivity.class));
    }

    public void connectSQL(View view){
        SQLclass.connectSql();
    }



    public void updateGameList(){
        i7.setImageResource(0);
        i1.setImageResource(R.drawable.splash_image);

        ArrayList<String> games = new ArrayList<String>();
        if(displayAllGames){
            for(Console c : dat.consoleList) {
                for (Game g : c.getGameList()) {
                    games.add(g.getTitle());
                }
            }
            t1.setText("Console: ");
            t7.setText("Games: " + dat.totalGameCount);
        }
        else if(displayUsers) {
            for (User u : dat.userList) {
                games.add(u.getUsername());
            }
        }
        else {
            conImage = curConsole.getName();
            conImage = conImage.replace(" ", "");
            conImage=conImage.toLowerCase();
            t7.setText("Games: " + curConsole.gameCount);
            t4.setText(("Title:"));
            t1.setText(("Release Date:" ));
            i7.setImageResource(getImageId(context,conImage));
            for (Game g : curConsole.getGameList()) {
                games.add(g.getTitle());
            }
        }
        ListView lv = (ListView)findViewById(R.id.listView);
        ArrayAdapter<String> myarrayAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, games);
        lv.setAdapter(myarrayAdapter);
        lv.setTextFilterEnabled(true);

    }

    public static int getImageId(Context context, String imageName) {
        return context.getResources().getIdentifier("drawable/" + imageName, null, context.getPackageName());
    }

    public static int getImageId2(Context context,String imageName) {
        return context.getResources().getIdentifier("drawable2/" + imageName, null, context.getPackageName());
    }

    public void populateConsoleList(){

        //spinner.setOnItemSelectedListener(this);
        ArrayList<String> al = new ArrayList<String>();
        al.add("Users");
        al.add("All Games");
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

    public void showPopup(String title, String message){
    AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(
            context);

    // set title
    alertDialogBuilder.setTitle(title);
    alertDialogBuilder
            .setMessage(message)
            .setCancelable(false)
    .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int id) {
                    dialog.cancel();
                }
            }
    );
    AlertDialog alertDialog = alertDialogBuilder.create();
    alertDialog.show();
}

    public void showConfirmPopup(String title, String message){
        AlertDialog.Builder builder = new AlertDialog.Builder(this);

        builder.setTitle(title);
        builder.setMessage(message);

        builder.setPositiveButton("Confirm", new DialogInterface.OnClickListener() {

            public void onClick(DialogInterface dialog, int which) {

                dialog.dismiss();
            }

        });

        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {

            @Override
            public void onClick(DialogInterface dialog, int which) {
                // Do nothing
                dialog.dismiss();
            }
        });

        AlertDialog alert = builder.create();
        alert.show();
    }


    public void searchGames(View v){
        if(c6.isChecked()){
            globalSearch = true;
        }
        else{

            globalSearch = false;
        }
        ArrayList<String> games = new ArrayList<String>();
        String txt = e2.getText().toString().toLowerCase();
        if(txt==null){
            if(searchInProgress) {
                updateGameList();
                return;
            }else{
                showPopup("Error", "Search cannot be empty");
            }

        }
        if((globalSearch)||(curConsole==null)) {
            for (Console c : dat.consoleList) {
                for (Game g : c.getGameList()) {
                    if (g.getTitle().toLowerCase().contains(txt)) {
                        games.add(g.getTitle());
                    }
                }
            }
        }
        else{
            for (Game g : curConsole.getGameList()) {
                if (g.getTitle().toLowerCase().contains(txt)) {
                    games.add(g.getTitle());
                }
            }
        }

        ListView lv = (ListView)findViewById(R.id.listView);
        ArrayAdapter<String> myarrayAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, games);
        lv.setAdapter(myarrayAdapter);
    }



    public void updateBasicGameInfo(String title) {
        if(displayAllGames){

            for(Console c : dat.consoleList) {
                for (Game g : c.getGameList()) {
                    if (g.getTitle().equals(title)) {
                        curGame = g;
                        t4.setText(("Title: " + g.getTitle()));
                        t1.setText(("Console " + g.getConsole()));
                    }
                }
            }

        }else{

        for (Game g : curConsole.getGameList()) {
            if (g.getTitle().equals(title)) {
                curGame = g;
                t4.setText(("Title: " + g.getTitle()));
                t1.setText(("Release Date" + g.getReleaseDate()));
            }
        }



                if (curGame.getEsrb().equals("Everyone"))
                {
                    i1.setImageResource(R.drawable.everyone);
                }
                else if (curGame.getEsrb().equals("Everyone 10+"))
                {
                    i1.setImageResource(R.drawable.everyone10);
                }
                else if (curGame.getEsrb().equals("Teen"))
                {
                    i1.setImageResource(R.drawable.teen);
                }
                else if (curGame.getEsrb().equals("Mature"))
                {
                    i1.setImageResource(R.drawable.mature);
                }
                else if (curGame.getEsrb().equals("Adults Only (AO)"))
                {
                    i1.setImageResource(R.drawable.ao);;
                }
                else{
                    i1.setImageResource(R.drawable.splash_image);
                }

            }
        }
    }




