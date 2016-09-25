package com.example.ben.unicade;

import android.app.Activity;
import android.media.Image;
import android.os.Bundle;
import android.view.View;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.Toast;

/**
 * Created by Ben on 12/31/2015.
 */
public class WebSettings extends Activity {

    private CheckBox c7;
    private CheckBox c8;
    private CheckBox c9;
    private CheckBox c10;
    private CheckBox c11;
    private CheckBox c12;
    private CheckBox c13;
    private CheckBox c14;
    private CheckBox c17;
    private CheckBox c18;
    private ImageView i9;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.web_settings);
        loadView();
    }




    public void rescrapeGlobal(View v){
        for(Console c : MainActivity.dat.consoleList){
            for(Game g : c.getGameList()){
                WebOps.scrapeInfo(g);
            }
        }
    }

    public void closeScrapingSettings(View v){
       saveWebSettings();
        super.onBackPressed();
    }

    public void applyWebSettings(View v){
        saveWebSettings();
    }

    public void saveWebSettings(){

        if(c7.isChecked()){
            WebOps.releaseDate = 1;
        }
        else{
            WebOps.releaseDate = 0;
        }

        if(c8.isChecked()){
            WebOps.critic = 1;
        }
        else{
            WebOps.critic = 0;
        }

        if(c9.isChecked()){
            WebOps.publisher = 1;
        }
        else{
            WebOps.publisher = 0;
        }

        if(c10.isChecked()){
            WebOps.developer = 1;
        }
        else{
            WebOps.developer = 0;
        }

        if(c11.isChecked()){
            WebOps.esrb = 1;
        }
        else{
            WebOps.esrb = 0;
        }

        if(c12.isChecked()){
            WebOps.esrbDescriptor = 1;
        }
        else{
            WebOps.esrbDescriptor = 0;
        }

        if(c13.isChecked()){
            WebOps.players = 1;
        }
        else{
            WebOps.players = 0;
        }

        if(c14.isChecked()){
            WebOps.description = 1;
        }
        else{
            WebOps.description = 0;
        }

        if(c17.isChecked()){
            WebOps.metac = 1;
        }
        else{
            WebOps.metac = 0;
        }

        if(c18.isChecked()){
            WebOps.mobyg = 1;
        }
        else{
            WebOps.mobyg = 0;
        }
        Toast.makeText(this, "Web settings saved successfully",
                Toast.LENGTH_LONG).show();
    }

    public void loadView(){
        c7 = (CheckBox) findViewById(R.id.checkBox7);
        c8 = (CheckBox) findViewById(R.id.checkBox8);
        c9 = (CheckBox) findViewById(R.id.checkBox9);
        c10 = (CheckBox) findViewById(R.id.checkBox10);
        c11 = (CheckBox) findViewById(R.id.checkBox11);
        c12 = (CheckBox) findViewById(R.id.checkBox12);
        c13 = (CheckBox) findViewById(R.id.checkBox13);
        c14 = (CheckBox) findViewById(R.id.checkBox14);
        c17 = (CheckBox) findViewById(R.id.checkBox17);
        c18 = (CheckBox) findViewById(R.id.checkBox18);
        i9 = (ImageView) findViewById(R.id.imageView9);
        i9.setImageResource(R.drawable.splash_image);
    }




}
