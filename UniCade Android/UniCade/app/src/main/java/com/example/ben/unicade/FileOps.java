package com.example.ben.unicade;
import java.io.BufferedReader;
import java.io.File;
import java.io.*;
import java.util.Set;

import android.content.Context.*;
import android.os.Environment;
import android.view.View;
import android.widget.EditText;

/**
 * Created by Ben on 12/17/2015.
 */
public class FileOps {

    public static boolean loadDatabase(String name)

    {
        try {
            File sdCard = Environment.getExternalStorageDirectory();
            File dir = new File (sdCard.getAbsolutePath() + "/UniCade");
            dir.mkdirs();
            File f = new File(dir, name);


            if (!f.exists()) {
                System.err.println("Database file does not exist");
                return false;
            }

            BufferedReader file = new BufferedReader(new FileReader(f));
            String line;
            int conCount = 0;
            Console c = new Console();
            String[] r = null;
            clearLibrary();
            while ((line = file.readLine()) != null) {
                r = line.split("\\|");
                if (line.substring(0, 5).contains("***")) {
                    if (conCount > 0) {
                        MainActivity.dat.consoleList.add(c);
                    }
                    c = new Console(r[0].substring(3), r[1], r[2], r[3], r[4], safeParse(r[5]) , r[6], r[7], r[8]);
                    conCount++;
                } else {
                    c.getGameList().add(new Game(r[0], r[1], safeParse(r[2]), r[3], r[4], r[5], r[6], r[7], r[8], r[9], r[10], r[11], r[12], r[13], r[14], r[15], safeParse(r[16])));
                    //System.out.println(r[0]);
                }
            }
            file.close();
            MainActivity.obj.populateConsoleList();
            return true;
        }catch(IOException e){
            return false;

    }
    }



    public static void saveDatabase(String name)
    {


        try {
            File sdCard = Environment.getExternalStorageDirectory();
            File dir = new File (sdCard.getAbsolutePath() + "/UniCade");
            dir.mkdirs();
            File file = new File(dir, name);
            if (!file.exists()) {
                file.createNewFile();
            } else {
                file.delete();
                file.createNewFile();
            }

            FileWriter fw = new FileWriter(file);
            BufferedWriter sw = new BufferedWriter(fw);
            for(Console c : MainActivity.dat.consoleList)
            {

                sw.write("***" + c.getName() + "|" + c.getEmuPath() + "|" + c.getRomPath() + "|" + c.getPrefPath() + "|" + c.getRomExt() + "|" + c.gameCount + "|" + "Console Info" + "|" + c.getLaunchParam() + "|" + c.getReleaseDate() + "\n");
                for(Game g : c.getGameList())
                {
                    String fav = "0";
                    if(g.getFav()>0){
                         fav = "1";
                    }
                    sw.write(g.getFileName() +"|"+ g.getConsole() +"|"+ g.launchCount +"|"+ g.getReleaseDate() +"|"+ g.getPublisher()+"|"+ g.getDeveloper() +"|"+ g.getUserScore()+"|"+ g.getCriticScore()+"|"+ g.getPlayers()+"|"+ g.getTrivia() +"|"+ g.getEsrb()+"|"+ g.getEsrbDescriptor()+"|"+ g.getEsrbSummary()+"|"+ g.getDescription()+"|"+ g.getGenres() +"|"+g.getTags()+"|"+ fav+ "\n");

                }
            }
            sw.close();

        }catch (IOException e){
            return;
        }

    }

    public static boolean loadPreferences(String name)
    {




        try {
            File sdCard = Environment.getExternalStorageDirectory();
            File dir = new File (sdCard.getAbsolutePath() + "/UniCade");
            dir.mkdirs();
            File f = new File(dir, name);


            if (!f.exists()) {
                System.err.println("Database file does not exist");
                return false;
            }

            BufferedReader file = new BufferedReader(new FileReader(f));

            int conCount = 0;
            Console c = new Console();
            String[] tmp = { "tmp" };



            String line = file.readLine();
            String[] r = line.split("\\|");
            System.err.println("PASS: "+ r[1]);
            SettingsWindow.passProtect = safeParse(r[1]);



            line = file.readLine();
            r = line.split("\\|");
            if ((r[1].contains("1"))) {
                SettingsWindow.scanOnStartup = 1;
            } else {
                SettingsWindow.scanOnStartup = 0;
            }

            line = file.readLine();
            r = line.split("\\|");
            if ((r[1].contains("1"))) {
                SettingsWindow.autoLoadDatabase = 1;
            } else {
                SettingsWindow.autoLoadDatabase = 0;
            }

            line = file.readLine();
            r = line.split("\\|");
            if ((r[1].contains("1"))) {
                SettingsWindow.enforceExt = 1;
            } else {
                SettingsWindow.enforceExt = 0;
            }

            line = file.readLine();
            r = line.split("\\|");
            if ((r[1].contains("1"))) {
                SettingsWindow.displayConImage = 1;
            } else {
                SettingsWindow.displayConImage = 0;
            }

            line = file.readLine();
            r = line.split("\\|");
            if ((r[1].contains("1"))) {
                SettingsWindow.displayESRBLogo = 1;
            } else {
                SettingsWindow.displayESRBLogo = 0;
            }

            line = file.readLine();    //Parse License Key
            r = line.split("\\|");
            MainActivity.userLicenseName = r[1];
            MainActivity.userLicenseKey = r[2];


            file.readLine(); //Skip ***Users*** line

            //Parse user data
            while ((line = file.readLine()) != null) {


                r = line.split("\\|");

                User u = new User(r[0], r[1], safeParse(r[2]), r[3], safeParse(r[4]), r[5], r[6], r[7]);
                if (r[6].length() > 0) {
                    String[] st = r[6].split("#");

                    for(String s : st)
                    {
                        u.favorites.add(s);
                    }
                }
                MainActivity.dat.userList.add(u);
                if (r[0].equals("UniCade")) {
                    MainActivity.curUser = u;
                }
            }

            file.close();

            return true;
        }
        catch (IOException e){
            return false;
        }
    }

    public static void savePreferences(String name)
    {

        try {
            File sdCard = Environment.getExternalStorageDirectory();
            File dir = new File (sdCard.getAbsolutePath() + "/UniCade");
            dir.mkdirs();
            File file = new File(dir, name);
            if (!file.exists()) {
                file.createNewFile();
            } else {
                file.delete();
                file.createNewFile();
            }

            FileWriter fw = new FileWriter(file);
            BufferedWriter sw = new BufferedWriter(fw);


            //Save current settings
            if(SettingsWindow.e1.getText().toString()==null) {
                SettingsWindow.passProtect = 0;
            }
            else if(SettingsWindow.e1.getText().toString().length()<3){
                SettingsWindow.passProtect = 0;
            }
            else{
                SettingsWindow.passProtect = safeParse(SettingsWindow.e1.getText().toString());
            }

            if(SettingsWindow.c2.isChecked()) {
            SettingsWindow.autoLoadDatabase = 1;
            }
            else{
                SettingsWindow.autoLoadDatabase = 0;
            }

            if(SettingsWindow.c4.isChecked()) {
                SettingsWindow.scanOnStartup = 1;
            }
            else{
                SettingsWindow.scanOnStartup = 0;
            }

            if(SettingsWindow.c3.isChecked()) {
                SettingsWindow.displayESRBLogo = 1;
            }
            else{
                SettingsWindow.displayESRBLogo = 0;
            }

            if(SettingsWindow.c5.isChecked()) {
                SettingsWindow.displayConImage = 1;
            }
            else{
                SettingsWindow.displayConImage = 0;
            }

            sw.write("PassProtect|" + SettingsWindow.passProtect + "\n");
            sw.write("ScanOnStartup|" + SettingsWindow.scanOnStartup+ "\n");
            sw.write("AutoLoadDatabse|" + SettingsWindow.autoLoadDatabase+ "\n");
            sw.write("EnforceExt|" + SettingsWindow.enforceExt + "\n");
            sw.write("DisplayConImage|" + SettingsWindow.displayConImage + "\n");
            sw.write("DisplayESRBLogo|" + SettingsWindow.displayESRBLogo + "\n");
            sw.write("License Key|" + MainActivity.userLicenseName + "|" + MainActivity.userLicenseKey + "\n");


            sw.write("***UserData***");
            for (User u : MainActivity.dat.userList)
            {
                String favs = "";
                for (String s : u.favorites)
                {
                    favs += (s + "#");
                }
                sw.write( u.getUsername() +"|"+ u.getPass() +"|"+ u.getLoginCount() +"|"+ u.getEmail() +"|"+ u.getLaunchCount()+"|"+ u.getUserInfo()+"|"+ u.getAllowedEsrb()+"|"+ u.getProfPic()+ "\n");
        }
            sw.close();

    } catch (IOException e) {
        e.printStackTrace();
    }

    }

    










  

    public static void loadDefaultConsoles()
    {
        MainActivity.dat.consoleList.add(new Console("Sega Genisis", "C:\\UniCade\\Emulators\\Fusion\\Fusion.exe", "C:\\UniCade\\ROMS\\Sega Genisis\\", "prefPath", ".bin*.iso*.gen*.32x", 0, "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
        MainActivity.dat.consoleList.add(new Console("Wii", "C:\\UniCade\\Emulators\\Dolphin\\dolphin.exe", "C:\\UniCade\\ROMS\\Wii\\", "prefPath", ".gcz*.iso", 0, "consoleInfo", "/b /e %file", "2006"));
        MainActivity.dat.consoleList.add(new Console("NDS", "C:\\UniCade\\Emulators\\NDS\\DeSmuME.exe", "C:\\UniCade\\ROMS\\NDS\\", "prefPath", ".nds", 0, "consoleInfo", "%file", "2005"));
        MainActivity.dat.consoleList.add(new Console("GBC", "C:\\UniCade\\Emulators\\GBA\\VisualBoyAdvance.exe", "C:\\UniCade\\ROMS\\GBC\\", "prefPath", ".gbc", 0, "consoleInfo", "%file", "1998"));
        MainActivity.dat.consoleList.add(new Console("MAME", "C:\\UniCade\\Emulators\\MAME\\mame.bat", "C:\\UniCade\\Emulators\\MAME\\roms\\", "prefPath", ".zip", 0, "consoleInfo", "", "1980")); //%file -skip_gameinfo -nowindow
        MainActivity.dat.consoleList.add(new Console("PC", "C:\\Windows\\explorer.exe", "C:\\UniCade\\ROMS\\PC\\", "prefPath", ".lnk*.url", 0, "consoleInfo", "%file", "1980"));
        Console c1 = new Console("GBA", "C:\\UniCade\\Emulators\\GBA\\VisualBoyAdvance.exe", "C:\\UniCade\\ROMS\\GBA\\", "prefPath", ".gba", 0, "consoleInfo", "%file", "2001");
        c1.getGameList().add(new Game("Mario.zip", "GBA"));
        c1.getGameList().add(new Game("Mario2.zip", "GBA"));
        MainActivity.dat.consoleList.add(c1);
        MainActivity.dat.consoleList.add(new Console("Gamecube", "C:\\UniCade\\Emulators\\Dolphin\\dolphin.exe", "C:\\UniCade\\ROMS\\Gamecube\\", "prefPath", ".iso*.gcz", 0, "consoleInfo", "/b /e %file", "2001"));
        MainActivity.dat.consoleList.add(new Console("NES", "C:\\UniCade\\Emulators\\NES\\Jnes.exe", "C:\\UniCade\\ROMS\\NES\\", "prefPath", ".nes", 0, "consoleInfo", "%file", "1983"));
        MainActivity.dat.consoleList.add(new Console("SNES", "C:\\UniCade\\Emulators\\ZSNES\\zsnesw.exe", "C:\\UniCade\\ROMS\\SNES\\", "prefPath", ".smc", 0, "consoleInfo", "%file", "1990"));
        MainActivity.dat.consoleList.add(new Console("N64", "C:\\UniCade\\Emulators\\Project64\\Project64.exe", "C:\\UniCade\\ROMS\\N64\\", "prefPath", ".n64*.z64", 0, "consoleInfo", "%file", "1996"));
        MainActivity.dat.consoleList.add(new Console("PS1", "C:\\UniCade\\Emulators\\ePSXe\\ePSXe.exe", "C:\\UniCade\\ROMS\\PS1\\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-nogui -loadbin %file", "1994"));
        MainActivity.dat.consoleList.add(new Console("PS2", "C:\\UniCade\\Emulators\\PCSX2\\pcsx2.exe", "C:\\UniCade\\ROMS\\PS2\\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2000"));
        MainActivity.dat.consoleList.add(new Console("Atari 2600", "C:\\UniCade\\Emulators\\Stella\\Stella.exe", "C:\\UniCade\\ROMS\\Atari 2600\\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "1977"));
        MainActivity.dat.consoleList.add(new Console("Dreamcast", "C:\\UniCade\\Emulators\\NullDC\\nullDC_Win32_Release-NoTrace.exe", "C:\\UniCade\\ROMS\\Dreamcast\\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
        MainActivity.dat.consoleList.add(new Console("PSP", "C:\\UniCade\\Emulators\\PPSSPP\\PPSSPPWindows64.exe", "C:\\UniCade\\ROMS\\PSP\\", "prefPath", ".iso*.cso", 0, "consoleInfo", "%file", "2005"));

    }

    public static void refreshGameCount()
    {

        for (Console c : MainActivity.dat.consoleList)
        {

            for (Game g : c.getGameList())
            {

                Database.totalGameCount++;
            }
        }

    }

    public static void clearLibrary(){
        MainActivity.dat.consoleList.clear();
    }

    public static void defaultPreferences()
    {

        SettingsWindow.defaultUser = "UniCade";
        SettingsWindow.scanOnStartup = 0;
        SettingsWindow.autoLoadDatabase = 0;
        SettingsWindow.passProtect = 0;
        SettingsWindow.enforceExt = 0;
        SettingsWindow.displayESRBLogo = 0;
        SettingsWindow.displayConImage = 0;

    }

    public static int safeParse(String text) {
        try {
            return Integer.parseInt(text);
        } catch (NumberFormatException e) {
            return 0;
        }
    }
}
