package com.example.ben.unicade;

import android.os.NetworkOnMainThreadException;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

/**
 * Created by Ben on 12/17/2015.
 */
public class WebOps {
    //Scraper Settings

    public static int metac = 1;
    public static int mobyg = 1;
    public static int year = 1;
    public static int publisher = 1;
    public static int critic = 1;
    public static int developer = 1;
    public static int description = 1;
    public static int esrb = 1;
    public static int esrbDescriptor = 1;
    public static int players = 1;
    public static int releaseDate = 1;
    public static int boxFront = 1;
    public static int boxBack = 1;
    public static int screenshot = 1;
    public static String gameName;

    public static int maxDescriptionLength = 5000;


    public static void scrapeInfo(Game g)
    {
        gameName = g.getTitle().replace(" - ", " ");
        gameName = gameName.replace(" ", "-");
        gameName = gameName.replace("'", "");
        if (mobyg > 0)
        {
            scrapeMobyGames(g);
        }
        if (metac > 0)
        {
            scrapeMetacritic(g);
        }

    }



    public static void scrapeMobyGames(Game g) {
        if (g == null) {
            return;
        }
        String url = ("http://www.mobygames.com/game/" + g.getConsole() + "/" + gameName);
        System.err.println("URLORIG: " + url);
        url = url.toLowerCase();
        String html = "";
        try {
            URL iurl = new URL(url);

            InputStream is = iurl.openStream();  // throws an IOException
            BufferedReader br = new BufferedReader(new InputStreamReader(is));
            String line;

            while ((line = br.readLine()) != null) {
                html = html + line;
            }
            System.err.println("URLSTRING: " + html);

        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        catch (NetworkOnMainThreadException e){
            e.printStackTrace();
        }
        try{
        //Parse ESRB
        if (esrb > 0) {
            int indexA = html.indexOf("ESRB");
            if (indexA < 0) {
                indexA = 0;
            }
            String s = html; //html.substring(indexA, indexA + 50);
            if (s.contains("Everyone")) {
                g.setEsrb("Everyone");
            } else if (s.contains("Kids to Adults")) {
                g.setEsrb("Everyone");
            } else if (s.contains("Everyone 10+")) {
                g.setEsrb("Everyone 10+");
            } else if (s.contains("Teen")) {
                g.setEsrb("Teen");
            } else if (s.contains("Mature")) {
                g.setEsrb("Mature");
            } else if (s.contains("Mature")) {
                g.setEsrb("Mature");
            } else if (s.contains("Adults Only")) {
                g.setEsrb("AO (Adults Only)");
            }

            if (g.getEsrb().length() > 2) {
                System.out.println(g.getEsrb());
            }

        }

        //Parse Release Date
        if (releaseDate > 0) {
            int tmp = html.indexOf("release-info");

            if (tmp > 0) {
                int indexB = html.indexOf("release-info", (tmp + 20));

                    String releaseDate = html.substring((indexB + 14), 4);
                    g.setReleaseDate(releaseDate);
                    System.out.println(g.getReleaseDate());


            }

            //Parse Critic Scores
            tmp = 0;
            tmp = html.indexOf("scoreHi");

            if (tmp > 0) {
                String criticScore = html.substring((tmp + 9), 2);
                String s2 = html.substring((tmp + 9));
                g.setCriticScore(criticScore);

            }
        }


        //Parse Publisher
        if (publisher > 0) {
            int tmp = 0;
            tmp = html.indexOf("/company/");
            if (tmp > 0) {
                int tmp2 = html.indexOf("-", tmp + 10);
                //System.out.println(tmp);


                String publisher = html.substring((tmp + 9), tmp2 - (tmp + 9));
                g.setPublisher(publisher);
                System.out.println(publisher);

            }
        }

        if (description > 0) {
            //Parse Description
            int tmp = 0;
            tmp = html.indexOf("Description<");
            if (tmp > 0) {
                int tmp2 = html.indexOf("<div class", tmp + 15);
                System.out.println(tmp);

                System.out.println(tmp2);
                if (tmp2 > 0) {
                    String description = html.substring((tmp + 16), tmp2 - (tmp + 16));
                    description = description.replace("\"", "");
                    description = description.replace("\t", "");
                    description = description.replace("\n", "");
                    description = description.replace("\r", "");

                    if (description.length() > maxDescriptionLength) {
                        description = description.substring(0, maxDescriptionLength);
                    }
                    g.setDescription(description);
                }
            }
        }
    }
        catch(IndexOutOfBoundsException e){

    }
        DetailedInfo.curGame = g;
    }


    public static void scrapeMetacritic(Game g)
    {
        try{
        //Metacritic Scraper

        String metaCon = "";
        if (g.getConsole().equals("PS1"))
        {
            metaCon = "playstation";
        }
        else if (g.getConsole().equals("N64"))
        {
            metaCon = "nintendo-64";
        }
        else if (g.getConsole().equals("GBA"))
        {
            metaCon = "game-boy-advance";
        }
        else if (g.getConsole().equals("PSP"))
        {
            metaCon = "psp";
        }
        else if (g.getConsole().equals("Gamecube"))
        {
            metaCon = "gamecube";
        }
        else if (g.getConsole().equals("Wii"))
        {
            metaCon = "wii";
        }
        else if (g.getConsole().equals("NDS"))
        {
            metaCon = "ds";
        }
        else if (g.getConsole().equals("Dreamcast"))
        {
            metaCon = "dreamcast";
        }


        String html;

        if (metaCon.length() < 1)
        {
            return;
        }

        String url = ("http://www.metacritic.com/game/" + metaCon + "/" + gameName + "/details");
        url = url.toLowerCase();

        html = "";
        try {
            URL iurl = new URL(url);

            InputStream is = iurl.openStream();  // throws an IOException
            BufferedReader br = new BufferedReader(new InputStreamReader(is));
            String line;

            while ((line = br.readLine()) != null) {
                html = html + line;
            }


        }
        catch(MalformedURLException e){
            e.printStackTrace();
        }
        catch(IOException e){
            e.printStackTrace();
        }


        //Parse Esrb descriptors
        if (esrbDescriptor > 0) {
            int tmp = 0;
            tmp = html.indexOf("ESRB Descriptors:");
            if (tmp > 0) {
                int tmp2 = html.indexOf("</td>", tmp + 26);

                if (tmp2 > 0) {
                    String descriptors = html.substring((tmp + 26), tmp2 - (tmp + 26));
                    g.setEsrbDescriptors(descriptors);
                }
            }
        }

        if (players > 0)
        {
            //Parse Players (Metacritic)
            int tmp = 0;
            tmp = html.indexOf("Players");
            if (tmp > 0)
            {
                int tmp2 = html.indexOf("<", tmp + 17);

                if (tmp2 > 0) {
                    String players = html.substring((tmp + 17), tmp2 - (tmp + 17));
                    g.setPlayers(players);
                }
            }
        }
            DetailedInfo.curGame = g;


        }
        catch(IndexOutOfBoundsException e){

        }



       

    }
}
