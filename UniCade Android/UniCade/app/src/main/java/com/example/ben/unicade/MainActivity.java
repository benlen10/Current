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
    }

    public void generateGameList(){
    }

    @Override
    public void populateList(){
        ListView lv = (ListView)findViewById(R.id.listView);
        ArrayAdapter<String> myarrayAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, myList);
        lv.setAdapter(myarrayAdapter);
        lv.setTextFilterEnabled(true);
    }
}
