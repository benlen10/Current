package com.example.ben.unicade;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.ArrayAdapter;

import java.util.ArrayList;


public class MainActivity extends AppCompatActivity {

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
