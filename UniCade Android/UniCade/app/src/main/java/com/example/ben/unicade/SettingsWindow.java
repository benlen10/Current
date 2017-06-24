package com.example.ben.unicade;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Environment;
import android.provider.ContactsContract;
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

import java.io.File;
import java.util.ArrayList;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.TextView;
import android.widget.ImageView;
import android.app.Fragment;
import android.widget.Toast;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import com.mysql.jdbc.Driver;

/**
 * Created by Ben on 12/18/2015.
 */
public class SettingsWindow extends Activity {
    Console curConsole2;
    Console curConsole;
    public Game curGame;
    public static String resultText;
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

    public void applyPrefs(View v){
        FileOps.savePreferences("Preferences.txt");
        Toast.makeText(this, "Preferences saved successfully",
                Toast.LENGTH_LONG).show();
    }

    public void launchWebSettings(View v){
        startActivity(new Intent(getApplicationContext(), WebSettings.class));
    }

    public void saveDatabase(View v){

        FileOps.saveDatabase("Database.txt");
        Toast.makeText(this, "Database saved successfully",
                Toast.LENGTH_LONG).show();
    }

    public void loadDatabase(View v){

        if(FileOps.loadDatabase("Database.txt")){
            Toast.makeText(this, "Databasae loaded successfully",
                    Toast.LENGTH_LONG).show();
        }
        else{
            Toast.makeText(this, "Database does not exist",
                    Toast.LENGTH_LONG).show();
        }
    }

    public void loadPrefs(View v){
        FileOps.loadPreferences("Preferences.txt");
    }

    public void savePreferences(View v){
        FileOps.savePreferences("Preferences.txt");
    }

    public void loadBackup(View v){
        if(FileOps.loadDatabase("Database_backup.txt")){
            Toast.makeText(this, "Backup databasae loaded successfully",
                    Toast.LENGTH_LONG).show();
        }
        else{
            Toast.makeText(this, "Backup database does not exist",
                    Toast.LENGTH_LONG).show();
        }


    }

    public void saveBackup(View v){
        FileOps.saveDatabase("Database_backup.txt");
        Toast.makeText(this, "Backup database saved successfully",
                Toast.LENGTH_LONG).show();
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
            t3.setText("License: Invalid");
        }
    }

    public void connectSQL(View v){
        if(connectSQL()){
            Toast.makeText(this, ("Connection successful"),
                    Toast.LENGTH_LONG).show();
        }
        else{
            Toast.makeText(this, ("Connection failed"),
                    Toast.LENGTH_LONG).show();
        }
    }

    public void enterLicense(View v){
        Toast.makeText(this, ("Current license key: " + MainActivity.userLicenseKey),
                Toast.LENGTH_LONG).show();
        PromptDialog dlg = new PromptDialog(SettingsWindow.this, R.string.Edit, R.string.LicenseKey) {
            @Override
            public boolean onOkClicked(String input) {
                MainActivity.userLicenseKey = input;
                if(MainActivity.validateSHA256(MainActivity.userLicenseName + Database.getHashKey(), MainActivity.userLicenseKey)){
                    MainActivity.validLicense = true;
                    t3.setText("License: Full");
                }
                else{
                    MainActivity.validLicense = true;
                    t3.setText("License: None");
                }
                return true;

            }
        };
        dlg.show();

    }
    public void deleteImagesButton(View v){
        deleteAllImages();
    }
    public void deleteAllImages(){
        File dir = null;
         dir = new File(Environment.getExternalStorageDirectory() + "/UniCade/Media/");
        for(File file: dir.listFiles()){
            file.delete();
        }
        Toast.makeText(this, "All local images deleted.",
                Toast.LENGTH_LONG).show();
    }

    public void showInputDialog(){
        PromptDialog dlg = new PromptDialog(SettingsWindow.this, R.string.ok, R.string.cancel) {
            @Override
            public boolean onOkClicked(String input) {
                resultText = input;
                return true;

            }
        };
        dlg.show();


    }

    public boolean connectSQL(){
        boolean isSuccess = false;
        String z ="";
        try {
            SQLclass sql;
            sql = new SQLclass();
            Connection con = sql.CONN();
            String userid = "root";
            String password = "Star6120";
            if (con == null) {
                z = "Error in connection with SQL server";
            } else {
                String query = "select * from Usertbl where UserId='" + userid + "' and password='" + password + "'";
                Statement stmt = con.createStatement();
                ResultSet rs = stmt.executeQuery(query);
                if(rs.next())
                {
                    z = "Login successfull";
                    isSuccess=true;
                }
                else
                {
                    z = "Invalid Credentials";
                    isSuccess = false;
                }
            }
            return isSuccess;
        }
        catch (Exception ex)
        {
            isSuccess = false;
            z = "Exceptions";
            return isSuccess;
        }
    }
    }




