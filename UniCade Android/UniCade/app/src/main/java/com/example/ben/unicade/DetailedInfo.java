package com.example.ben.unicade;

import android.app.Activity;
import android.app.AlertDialog;
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
    public static CheckBox c1;
    private String m_Text = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.info_activity);
        findView();
        loadInfo(MainActivity.curGame);
    }

    public void closeInfoWindow(View v){
        super.onBackPressed();
    }

    public void loadInfo(Game g){
        if(g==null){
            return;
        }
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
        b4 = (Button) findViewById(R.id.button4);
        c1 = (CheckBox) findViewById(R.id.checkBox);

    }

    public void showInputDialog(View v){
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("Title");

        final EditText input = new EditText(this);

        input.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
        builder.setView(input);


        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                m_Text = input.getText().toString();
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

}
