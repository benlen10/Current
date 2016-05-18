package com.example.ben.unicade;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.FragmentActivity;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import java.util.ArrayList;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.TextView;
import android.widget.ImageView;
import android.app.Fragment;

/**
 * Created by Ben on 12/18/2015.
 */
public class SettingsWindow extends Activity {
    Console curConsole2;
    Console curConsole;
    public Game curGame;
    User curUser;

    //Preference Data

    public static String defaultUser;
    public static int scanOnStartup=0;
    public static int autoLoadDatabase =1;
    public static int passProtect =0;
    public static int enforceExt=0;
    public static int displayConImage = 1;
    public static int displayESRBLogo=1;
    public static CheckBox c2;
    public static CheckBox c3;
    public static CheckBox c4;
    public static CheckBox c5;
    public static EditText e1;
    public static TextView t3;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.settings_activity);
        loadView();
    }

    public void closeSettings(View v){
        FileOps.savePreferences("Preferences.txt");
        super.onBackPressed();
    }

    public void launchWebSettings(View v){
        startActivity(new Intent(getApplicationContext(), WebSettings.class));
    }

    public void saveDatabase(View v){
        FileOps.saveDatabase("Database.txt");
    }

    public void loadDatabase(View v){
        FileOps.loadDatabase("Database.txt");
    }

    public void loadPrefs(View v){
        FileOps.loadPreferences("Preferences.txt");
    }

    public void savePreferences(View v){
        FileOps.savePreferences("Preferences.txt");
    }

    public void loadBackup(View v){
        FileOps.loadDatabase("Database_backup.txt");
    }

    public void saveBackup(View v){
        FileOps.loadDatabase("Database_backup.txt");
    }

    public void loadView() {
        c2 = (CheckBox) findViewById(R.id.checkBox2);
        c3 = (CheckBox) findViewById(R.id.checkBox3);
        c4 = (CheckBox) findViewById(R.id.checkBox4);
        c5 = (CheckBox) findViewById(R.id.checkBox5);
        e1 = (EditText) findViewById(R.id.editText);
        t3 = (TextView) findViewById(R.id.textView3);

        if (SettingsWindow.autoLoadDatabase > 0) {
            c2.setChecked(true);
        }
        if (SettingsWindow.scanOnStartup > 0) {
            c4.setChecked(true);
        }
        if (SettingsWindow.displayConImage > 0) {
            c5.setChecked(true);
        }
        if (SettingsWindow.displayESRBLogo > 0) {
            c3.setChecked(true);

        }
        if (passProtect > 0) {
            e1.setText(Integer.toString(passProtect));
        }
        if(MainActivity.validLicense) {
            t3.setText("License: Full");
        }
        else{
            t3.setText("License: None");
        }
    }
    }


