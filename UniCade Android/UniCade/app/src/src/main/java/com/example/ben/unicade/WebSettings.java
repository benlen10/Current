package com.example.ben.unicade;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;

/**
 * Created by Ben on 12/31/2015.
 */
public class WebSettings extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.settings_activity);
        loadView();
    }


    public void loadView(){
        return;
    }

    public void rescrapeGlobal(View v){
        for(Console c : MainActivity.dat.consoleList){
            for(Game g : c.getGameList()){
                WebOps.scrapeInfo(g);
            }
        }
    }
}
