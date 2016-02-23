package com.example.ben.unicade;
import java.io.BufferedReader;
import java.io.File;
import java.io.*;
import android.content.Context.*;

/**
 * Created by Ben on 12/17/2015.
 */
public class FileOps {

    public static boolean loadDatabase(String path)

    {
        try {
            File f = new File(path);

            if (!f.exists()) {
                System.out.println("Database file does not exist");
                return false;
            }

            BufferedReader file = new BufferedReader(new FileReader(f));
            String line;
            int conCount = 0;
            Console c = new Console();
            String[] r = {" "};

            while ((line = file.readLine()) != null) {
                r = line.split("|");
                //System.out.println("Loop");
                if (line.substring(0, 5).contains("***")) {
                    if (conCount > 0) {
                        MainActivity.dat.consoleList.add(c);
                    }
                    c = new Console(r[0].substring(3), r[1], r[2], r[3], r[4], Integer.parseInt(r[5]), r[6], r[7], r[8]);
                    conCount++;
                } else {
                    c.getGameList().add(new Game(r[0], r[1], Integer.parseInt(r[2]), r[3], r[4], r[5], r[6], r[7], r[8], r[9], r[10], r[11], r[12], r[13], r[14], r[15], Integer.parseInt(r[16])));
                    //System.out.println(r[0]);
                }
            }
            file.close();
            return true;
        }catch(IOException e){
            return false;

    }
    }



    public  void saveDatabase()
    {


        try {
            File file = new File(MainActivity.obj.getFilesDir(), "Database.txt");

            if (!file.exists()) {
                file.createNewFile();
            } else {
                file.delete();
                file.createNewFile();
            }

            FileWriter fw = new FileWriter(file.getAbsoluteFile());
            BufferedWriter sw = new BufferedWriter(fw);
            for(Console c : MainActivity.dat.consoleList)
            {
                String txt = String.format("***{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|", c.getName(), c.getEmuPath(), c.getRomPath(), c.getPrefPath(), c.getRomExt(), c.gameCount, "Console Info", c.getLaunchParam(), c.getReleaseDate());
                sw.write(txt);
                for(Game g : c.getGameList())
                {
                    txt = String.format(g.getFileName() +"|"+ g.getConsole() +"|"+ g.launchCount +"|"+ g.getReleaseDate() +"|"+ g.getPublisher()+"|"+ g.getDeveloper() +"|"+ g.getUserScore()+"|"+ g.getCriticScore()+"|"+ g.getPlayers()+"|"+ g.getTrivia() +"|"+ g.getEsrb()+"|"+ g.getEsrbDescriptor()+"|"+ g.getEsrbSummary()+"|"+ g.getDescription()+"|"+ g.getGenres() +"|"+g.getTags()+"|"+ g.getFav());
                    sw.write(txt);

                }
            }
        }catch (IOException e){
            return;
        }

    }

    public static boolean loadPreferences(String path)
    {



        String[] tmp = { "tmp" };
        String[] r = { " " };
        try {
            File f = new File(path);

            if (!f.exists())
            {
                System.out.println("Database file does not exist");
                return false;
            }

            BufferedReader file = new BufferedReader(new FileReader(f));

            String line = file.readLine();


            r = line.split("|");
            for(User u :MainActivity.dat.userList)
            {
                if (u.getUsername().equals(r[1]))   //Set curUser to default user
                {
                    MainActivity.curUser = u;
                    System.out.println("Current user change to " + u.getUsername());
                }
            }
            line = file.readLine();
            r = line.split("|");
            MainActivity.databasePath = r[1];

            line = file.readLine();
            r = line.split("|");
            MainActivity.emuPath = r[1];

            line = file.readLine();
            r = line.split("|");
            MainActivity.mediaPath = r[1];

            line = file.readLine();
            r = line.split("|");
            if (r[1].contains("1")) {
                SettingsWindow.showSplash = 1;
            } else {
                SettingsWindow.showSplash = 0;
            }

            line = file.readLine();
            r = line.split("|");
            if ((r[1].contains("1"))) {
                SettingsWindow.scanOnStartup = 1;
            } else {
                SettingsWindow.scanOnStartup = 0;
            }

            line = file.readLine();
            r = line.split("|");
            SettingsWindow.restrictESRB = Integer.parseInt(r[1]);

            file.readLine();
            r = line.split("|");
            if (r[1].contains("1")) {
                SettingsWindow.requireLogin = 1;
            } else {
                SettingsWindow.requireLogin = 0;
            }

            line = file.readLine();
            r = line.split("|");
            if (r[1].contains("1")) {
                SettingsWindow.cmdOrGui = 1;
            } else {
                SettingsWindow.cmdOrGui = 0;
            }

            line = file.readLine();
            r = line.split("|");
            if (r[1].contains("1")) {
                SettingsWindow.showLoading = 1;
            } else {
                SettingsWindow.showLoading = 0;
            }

            line = file.readLine();
            r = line.split("|");
            if (r[1].contains("1")) {
                SettingsWindow.payPerPlay = 1;
            } else {
                SettingsWindow.payPerPlay = 0;
            }

            if (r[2].contains("1")) {
                SettingsWindow.perLaunch = 1;
            } else {
                SettingsWindow.perLaunch = 0;
            }
            SettingsWindow.coins = Integer.parseInt(r[3]);
            SettingsWindow.playtime = Integer.parseInt(r[4]);

            line = file.readLine();    //Parse License Key
            r = line.split("|");
            MainActivity.userLicenseName = r[1];
            MainActivity.userLicenseKey = r[2];


            file.readLine(); //Skip ***Users*** line

            //Parse user data
            while ((line = file.readLine()) != null) {


                r = line.split("|");

                User u = new User(r[0], r[1], Integer.parseInt(r[2]), r[3], Integer.parseInt(r[4]), r[5], r[6], r[7]);
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

    public static void savePreferences(String path)
    {

        try{
        File file = new File(path);

        if (!file.exists()) {
            file.createNewFile();
        }else{
            file.delete();
            file.createNewFile();
        }

        FileWriter fw = new FileWriter(file.getAbsoluteFile());
        BufferedWriter sw = new BufferedWriter(fw);

            sw.write("DefaultUser|" + SettingsWindow.defaultUser);
            sw.write("DatabasePath|" + MainActivity.databasePath);
            sw.write("EmulatorFolderPath|" + MainActivity.emuPath);
            sw.write("MediaFolderPath|" + MainActivity.mediaPath);
            sw.write("ShowSplash|" + SettingsWindow.showSplash);
            sw.write("ScanOnStartup|" + SettingsWindow.scanOnStartup);
            sw.write("RestrictESRB|" + SettingsWindow.restrictESRB);
            sw.write("RequireLogin|" + SettingsWindow.requireLogin);
            sw.write("CmdOrGui|" + SettingsWindow.cmdOrGui);
            //sw.write("KeyBindings|" + SettingsWindow.defaultUser);
            sw.write("LoadingScreen|" + SettingsWindow.showLoading);
            sw.write("PaySettings|" + SettingsWindow.payPerPlay + "|" + SettingsWindow.perLaunch + "|" + SettingsWindow.coins + "|" + SettingsWindow.playtime);
            sw.write("License Key|" + MainActivity.userLicenseName + "|" + MainActivity.userLicenseKey);
            sw.write("***UserData***");
            for (User u : MainActivity.dat.userList)
            {
                String favs = "";
                for (String s : u.favorites)
                {
                    favs += (s + "#");
                }
                sw.write( u.getUsername() +"|"+ u.getPass() +"|"+ u.getLoginCount() +"|"+ u.getEmail() +"|"+ u.getLaunchCount()+"|"+ u.getUserInfo()+"|"+ u.getAllowedEsrb()+"|"+ u.getProfPic());
        }

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

    public static void defaultPreferences()
    {

        SettingsWindow.defaultUser = "UniCade";
        SettingsWindow.showSplash = 0;
        SettingsWindow.scanOnStartup = 0;
        SettingsWindow.restrictESRB = 0;
        SettingsWindow.requireLogin = 0;
        SettingsWindow.cmdOrGui = 0;
        SettingsWindow.showLoading = 0;
        SettingsWindow.payPerPlay = 0;
        SettingsWindow.coins = 1;
        SettingsWindow.playtime = 15;
        SettingsWindow.perLaunch = 0;
    }
}
