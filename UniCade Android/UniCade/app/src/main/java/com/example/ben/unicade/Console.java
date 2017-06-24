package com.example.ben.unicade;
import java.util.ArrayList;


public class Console {

    //region Properties

    public String name;
    private String emuPath;
    private String romPath;
    private String prefPath;
    private String romExt;
    private String consoleInfo;
    private String releaseDate;
    private String launchParam;
    private ArrayList<Game> gameList;
    public int gameCount;

    //endregion

    //region Constructors

    public Console()
    {
        name = "null";
    }

    public Console(String name, String emuPath, String romPath, String prefPath, String romExt, int gameCount, String consoleInfo, String launchParam, String releaseDate)
    {
        this.name = name;
        this.emuPath = emuPath;
        this.romPath = romPath;
        this.prefPath = prefPath;
        this.romExt = romExt;
        this.gameCount = gameCount;
        this.consoleInfo = consoleInfo;
        this.launchParam = launchParam;
        this.releaseDate = releaseDate;
        gameList = new ArrayList<Game>();
    }

    //endregion

    //region Getters

    public String getName()
    {
        return name;
    }

    public String getReleaseDate()
    {
        return releaseDate;
    }

    public ArrayList<Game> getGameList()
    {
        return gameList;
    }

    public String getEmuPath()
    {
        return emuPath;
    }

    public String getPrefPath()
    {
        return prefPath;
    }

    public String getRomPath()
    {
        return romPath;
    }

    public String getRomExt()
    {
        return romExt;
    }

    public String getConsoleInfo()
    {
        return consoleInfo;
    }

    public String getLaunchParam()
    {
        return launchParam;
    }

    //endregion

    //region Setters

    public void setName(String s)
    {
        name = s;
    }

    public void setEmuPath(String s)
    {
        emuPath = s;
    }

    public void setReleaseDate(String s)
    {
        releaseDate = s;
    }

    public void setRomPath(String s)
    {
        romPath = s;
    }

    public void setRomExt(String s)
    {
        romExt  = s;
    }

    public void setConsoleInfo(String s)
    {
        consoleInfo = s;
    }

    public void setLaunchParam(String s)
    {
        launchParam = s;
    }

    //endregion

    //region Public Methods

    /**
     * Summary: Add a game to the current console object
     */
    public boolean addGame(Game game)
    {
        if (!game.getConsole().equals(name))
        {
            return false;
        }
        for (Game g: gameList) {

            if (g.getFileName().equals(game.getFileName())){
                return false;
            }
        }
        return true;
    }

    //endregion
}
