package com.example.ben.unicade;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.media.Image;
import android.os.Bundle;
import android.text.InputType;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

/**
 * Created by Ben on 12/20/2015.
 */
public class DetailedInfo extends Activity{

    public static TextView t5;
    public static TextView t9;
    public static TextView t10;
    public static TextView t11;  //Title
    public static TextView t12;
    public static TextView t13;
    public static TextView t14;
    public static TextView t15;
    public static TextView t16;
    public static TextView t17;
    public static Button b4;
    public static ImageView i2;
    public static ImageView i3;
    public static ImageView i4;
    public static ImageView i5;
    public static ImageView i6;
    public static ImageView i8;
    public static CheckBox c1;
    private String resultText = "";
    private String origText = "";
    private String popupTitle = "";
    public Game curGame;
    final Context context = this;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.info_activity);
        findView();
        i6.setImageResource(MainActivity.getImageId(context, MainActivity.conImage));
        i8.setImageResource(R.drawable.splash_image);
        loadInfo(MainActivity.curGame);
    }

    public void closeInfoWindow(View v){
        super.onBackPressed();
    }

    public void refreshGameInfoButton(View v){
        loadInfo(curGame);
    }

    public void saveDatabase(View v){
        FileOps.saveDatabase();
    }

    public void loadInfo(Game g){
        if(g==null){
            return;
        }
        curGame = g;
        t5.setText(("Title: " + g.getTitle()));
        t9.setText(("Console: "+ g.getConsole()));
        t10.setText(("Release Date: " + g.getReleaseDate()));
        t11.setText(("Publisher: " + g.getPublisher()));
        t12.setText(("Critic Score: " + g.getCriticScore()));
        t13.setText(("Players: " + g.getPlayers()));
        t14.setText(("ESRB Rating: " + g.getEsrb()));
        t15.setText(("ESRB Descriptors: " + g.getEsrbDescriptor()));
        t15.setText(("Launch Count: " + g.launchCount));
        t15.setText(("Description: " + g.getDescription()));
        if(g.getFav()>0) {
            c1.setChecked(true);
        }


        if (g.getEsrb().equals("Everyone"))
        {
            i5.setImageResource(R.drawable.everyone);
        }
        else if (g.getEsrb().equals("Everyone 10+"))
        {
            i5.setImageResource(R.drawable.everyone10);
        }
        else if (g.getEsrb().equals("Teen"))
        {
            i5.setImageResource(R.drawable.teen);
        }
        else if (g.getEsrb().equals("Mature"))
        {
            i5.setImageResource(R.drawable.mature);
        }
        else if (g.getEsrb().equals("Adults Only (AO)"))
        {
            i5.setImageResource(R.drawable.ao);;
        }
        else{
            i5.setImageResource(0);
        }
    }

    public void findView(){
        t5 = (TextView) findViewById(R.id.textView5);
        t9 = (TextView) findViewById(R.id.textView9);
        t10 = (TextView) findViewById(R.id.textView10);
        t11 = (TextView) findViewById(R.id.textView11);
        t12 = (TextView) findViewById(R.id.textView12);
        t13 = (TextView) findViewById(R.id.textView13);
        t14 = (TextView) findViewById(R.id.textView14);
        t15 = (TextView) findViewById(R.id.textView15);
        t16 = (TextView) findViewById(R.id.textView16);
        t17 = (TextView) findViewById(R.id.textView17);
        i2 = (ImageView) findViewById(R.id.imageView2);
        i3 = (ImageView) findViewById(R.id.imageView3);
        i4 = (ImageView) findViewById(R.id.imageView4);
        i5 = (ImageView) findViewById(R.id.imageView5);
        i6 = (ImageView) findViewById(R.id.imageView6);
        i8 = (ImageView) findViewById(R.id.imageView8);
        b4 = (Button) findViewById(R.id.button4);
        c1 = (CheckBox) findViewById(R.id.checkBox);

    }

    public void showInputDialog(){
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(popupTitle);
        final EditText input = new EditText(this);
        input.setText(origText);
        builder.setView(input);


        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                resultText = input.getText().toString();
            }
        });
        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.cancel();
            }
        });

        builder.show();
    }



    public void editRelease(View v){
        origText = curGame.getReleaseDate();
        popupTitle = "Edit Release Date";
        showInputDialog();
        curGame.setReleaseDate(resultText);
        loadInfo(curGame);
    }

    public void editPublisher(View v){
        origText = curGame.getPublisher();
        popupTitle = "Edit Publisher";
        showInputDialog();
        curGame.setPublisher(resultText);
        loadInfo(curGame);
    }

    public void editScore(View v){
        origText = curGame.getCriticScore();
        popupTitle = "Edit Critic Score";
        showInputDialog();
        curGame.setCriticScore(resultText);
        loadInfo(curGame);
    }

    public void editPlayers(View v){
        origText = curGame.getPlayers();
        popupTitle = "Edit Playerse";
        showInputDialog();
        curGame.setPlayers(resultText);
        loadInfo(curGame);
    }

    public void editEsrb(View v){
        origText = curGame.getEsrb();
        popupTitle = "Edit ESRB Rating";
        showInputDialog();
        curGame.setEsrb(resultText);
        loadInfo(curGame);

    }

    public void editEsrbDescriptors(View v){
        origText = curGame.getEsrbDescriptor();
        popupTitle = "Edit ESRB Descriptors";
        showInputDialog();
        curGame.setEsrbDescriptors(resultText);
        loadInfo(curGame);
    }

    public void editLaunchCount(View v){
        origText = Integer.toString(curGame.launchCount);
        popupTitle = "Edit Launch Count";
        showInputDialog();
        curGame.launchCount = Integer.parseInt(resultText);
        loadInfo(curGame);
    }

    public void editDescription(View v){
        origText = curGame.getDescription();
        popupTitle = "Edit Desription";
        showInputDialog();
        curGame.setDescription(resultText);
        loadInfo(curGame);
    }

}
