package com.example.ben.unicade;

import android.app.Activity;
import android.os.Bundle;
import android.support.v4.app.FragmentActivity;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
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
    public static int showSplash;
    public static int scanOnStartup;
    public static int restrictESRB;
    public static int requireLogin;
    public static int cmdOrGui;
    public static int showLoading;
    public static int payPerPlay;
    public static int coins;
    public static int playtime;
    public static int perLaunch;
    public static int viewEsrb;
    public static int passProtect;
    public static int enforceExt;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.settings_activity);
    }

    public void closeSettings(View v){
        super.onBackPressed();
    }

    public void saveDatabase(View v){
        FileOps.saveDatabase();
    }

    public void loadDatabase(View v){
        FileOps.loadDatabase();
    }



    }
