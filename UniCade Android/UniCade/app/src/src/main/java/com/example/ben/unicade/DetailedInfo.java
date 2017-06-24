package com.example.ben.unicade;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.BitmapFactory;
import android.media.Image;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.text.InputType;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

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
    private boolean boxfrontFull = false;
    private boolean boxbackFull = false;
    private boolean screenshotFull = false;
    private boolean consoleFull = false;
    private android.view.ViewGroup.LayoutParams origParams;
    private String imgDecodableString;
    private static int RESULT_LOAD_IMG = 1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.info_activity);
        findView();
        if(SettingsWindow.displayConImage==1) {
            i6.setImageResource(MainActivity.getImageId(context, MainActivity.conImage));
        }
        i8.setImageResource(R.drawable.splash_image);
        loadInfo(MainActivity.curGame);
    }

    public void closeInfoWindow(View v){
        super.onBackPressed();
    }

    public void refreshGameInfoButton(View v){
        loadInfo(curGame);
    }

    public void rescrapeSingleGame(View v){
        WebOps.scrapeInfo(curGame);
        showPopup("Notice", "Operation Complete");
    }

    public void rescrapeConsole(View v){
        for(Game g : MainActivity.curConsole.getGameList()) {
            WebOps.scrapeInfo(g);
        }
        showPopup("Notice", "Operation Complete");
    }

    public void saveDatabase(View v){
        FileOps.saveDatabase("Database.txt");
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

            i2.setImageResource(MainActivity.getImageId(MainActivity.obj.context, g.getConsole().toLowerCase().replace(" ", "") + "_" + g.getTitle().toLowerCase().replace(" ", "") + "_boxfront"));
            i3.setImageResource(MainActivity.getImageId(MainActivity.obj.context, g.getConsole().toLowerCase().replace(" ", "") + "_" + g.getTitle().toLowerCase().replace(" ", "") + "_boxback"));
            i4.setImageResource(MainActivity.getImageId(MainActivity.obj.context, g.getConsole().toLowerCase().replace(" ", "") + "_" + g.getTitle().toLowerCase().replace(" ", "") + "_screenshot"));


        if (SettingsWindow.displayESRBLogo==1) {
            if (g.getEsrb().equals("Everyone")) {
                i5.setImageResource(R.drawable.everyone);
            } else if (g.getEsrb().equals("Everyone 10+")) {
                i5.setImageResource(R.drawable.everyone10);
            } else if (g.getEsrb().equals("Teen")) {
                i5.setImageResource(R.drawable.teen);
            } else if (g.getEsrb().equals("Mature")) {
                i5.setImageResource(R.drawable.mature);
            } else if (g.getEsrb().equals("Adults Only (AO)")) {
                i5.setImageResource(R.drawable.ao);
                ;
            } else {
                i5.setImageResource(0);
            }
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

    public void showInputDialog(String title){
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
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

    public void boxfrontFullscreen(View v){
        if(boxfrontFull) {
            android.view.ViewGroup.LayoutParams params = i2.getLayoutParams();
            params.width = 500;
            params.height = 500;
            i2.setLayoutParams(params);
            boxfrontFull = false;
        }
        else{

            android.view.ViewGroup.LayoutParams params = i2.getLayoutParams();
            params.width = 1500;
            params.height = 1500;
            i2.setLayoutParams(params);
            boxfrontFull = true;
        }
    }

    public void boxbackFullscreen(View v){
        if(boxbackFull) {
            android.view.ViewGroup.LayoutParams params = i3.getLayoutParams();
            params.width = 500;
            params.height = 500;
            i3.setLayoutParams(params);
            boxbackFull = false;
        }
        else{

            android.view.ViewGroup.LayoutParams params = i3.getLayoutParams();
            params.width = 1500;
            params.height = 1500;
            i3.setLayoutParams(params);
            boxbackFull = true;
        }
    }

    public void screenshotFullscreen(View v){
        if(screenshotFull) {
            android.view.ViewGroup.LayoutParams params = i4.getLayoutParams();
            params.width = 500;
            params.height = 500;
            i4.setLayoutParams(params);
            screenshotFull = false;
        }
        else{

            android.view.ViewGroup.LayoutParams params = i4.getLayoutParams();
            params.width = 1500;
            params.height = 1500;
            i4.setLayoutParams(params);
            screenshotFull = true;
        }
    }

    public void consoleFullscreen(View v){
        if(consoleFull) {
            android.view.ViewGroup.LayoutParams params = i4.getLayoutParams();
            params.width = 500;
            params.height = 500;
            i6.setLayoutParams(params);
            consoleFull = false;
        }
        else{

            android.view.ViewGroup.LayoutParams params = i4.getLayoutParams();
            params.width = 1500;
            params.height = 1500;
            i6.setLayoutParams(params);
            consoleFull = true;
        }
    }

    public void toggleFav(View v){
        if(c1.isChecked()){
            curGame.setFav(1);
            System.err.println("IS FAV");
        }
        else
        {
            curGame.setFav(0);
            System.err.println("IS NOT FAV");
        }
    }



    public void editRelease(View v){
        origText = curGame.getReleaseDate();
        showInputDialog("Edit Release Date");
        curGame.setReleaseDate(resultText);
        loadInfo(curGame);
    }

    public void editPublisher(View v){
        origText = curGame.getPublisher();
        showInputDialog("Edit Publisher");
        curGame.setPublisher(resultText);
        loadInfo(curGame);
    }

    public void editScore(View v){
        origText = curGame.getCriticScore();
        showInputDialog("Edit Critic Score");
        curGame.setCriticScore(resultText);
        loadInfo(curGame);
    }

    public void editPlayers(View v){
        origText = curGame.getPlayers();
        showInputDialog("Edit Players");
        curGame.setPlayers(resultText);
        loadInfo(curGame);
    }

    public void editEsrb(View v){
        origText = curGame.getEsrb();
        showInputDialog("Edit ESRB Rating");
        curGame.setEsrb(resultText);
        loadInfo(curGame);
    }

    public void editEsrbDescriptors(View v){
        origText = curGame.getEsrbDescriptor();
        showInputDialog("Edit ESRB Descriptors");
        curGame.setEsrbDescriptors(resultText);
        loadInfo(curGame);
    }

    public void editLaunchCount(View v){
        origText = Integer.toString(curGame.launchCount);
        showInputDialog("Edit Launch Count");
        curGame.launchCount = Integer.parseInt(resultText);
        loadInfo(curGame);
    }

    public void editDescription(View v){
        origText = curGame.getDescription();
        showInputDialog("Edit Desription");
        curGame.setDescription(resultText);
        loadInfo(curGame);
    }

    public void loadImagefromGallery(View view) {
        // Create intent to Open Image applications like Gallery, Google Photos
        Intent galleryIntent = new Intent(Intent.ACTION_PICK,
                android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
        // Start the Intent
        startActivityForResult(galleryIntent, RESULT_LOAD_IMG);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        try {
            // When an Image is picked
            if (requestCode == RESULT_LOAD_IMG && resultCode == RESULT_OK
                    && null != data) {
                // Get the Image from data

                Uri selectedImage = data.getData();
                String[] filePathColumn = { MediaStore.Images.Media.DATA };

                // Get the cursor
                Cursor cursor = getContentResolver().query(selectedImage,
                        filePathColumn, null, null, null);
                // Move to first row
                cursor.moveToFirst();

                int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                imgDecodableString = cursor.getString(columnIndex);
                cursor.close();
                ImageView imgView = (ImageView) findViewById(R.id.imageView6);
                // Set the Image in ImageView after decoding the String
                imgView.setImageBitmap(BitmapFactory
                        .decodeFile(imgDecodableString));

            } else {
                Toast.makeText(this, "You haven't picked Image",
                        Toast.LENGTH_LONG).show();
            }
        } catch (Exception e) {
            Toast.makeText(this, "Something went wrong", Toast.LENGTH_LONG)
                    .show();
        }

    }



}
