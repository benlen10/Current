package com.example.ben.unicade;

/**
 * Created by Ben on 12/17/2015.
 */
public class Game {
    
        private String fileName;
        private String con;
        private String title;
        private String description;
        private String releaseDate;
        private String publisher;
        private String developer;
        private String genres;
        private String tags;
        private String userScore;
        private String criticScore;
        private String trivia;
        private String players;
        private String esrb;
        private String esrbDescriptor;
        private String esrbSummary;
        private int fav;
        public int launchCount;


        //Basic Constructor
        public Game(String fileName, String con)
        {
            this.fileName = fileName;
            this.con = con;
            title = fileName.substring(0, fileName.indexOf('.'));
            description = "";
            releaseDate = "";
            publisher = "";
            developer = "";
            userScore = "";
            launchCount= 0;
            criticScore = "";
            trivia = "";
            players = "";
            esrb = "";
            esrbDescriptor = "";
            esrbSummary = "";

        }

        //Extended Constuctor
        public Game(String fileName, String con, int launchCount, String releaseDate, String publisher, String developer, String userScore, String criticScore, String players, String trivia, String esrb, String esrbDescriptor, String esrbSummary, String description, String genres, String tags, int fav)
        {

            this.fileName = fileName;
            this.con = con;
            this.fav = fav;
            this.launchCount = launchCount;
            this.releaseDate = releaseDate;
            this.publisher = publisher;
            this.developer = developer;
            this.userScore = userScore;
            this.criticScore = criticScore;
            this.players = players;
            this.trivia = trivia;
            this.esrb = esrb;
            this.description = description;
            this.esrbDescriptor = esrbDescriptor;
            this.esrbSummary = esrbSummary;
            this.genres = genres;
            this.tags = tags;
            if (fileName.length() > 2)
            {
                title = fileName.substring(0, fileName.indexOf('.'));
            }
        }

        //Methods

    public String getFileName()
    {
        return fileName;
    }

    public String getConsole()
    {
        return con;
    }

    public String getReleaseDate()
    {
        return releaseDate;
    }

    public String getUserScore()
    {
        return userScore;
    }

    public String getDeveloper()
    {
        return developer;
    }

    public String getPublisher()
    {
        return publisher;
    }

    public String getCriticScore()
    {
        return criticScore;
    }

    public String getPlayers()
    {
        return players;
    }

    public int getFav()
    {
        return fav;
    }

    public void setFav(int n)
    {
        fav = n;
    }

    public String getTrivia()
    {
        return trivia;
    }

    public String getDescription()
    {
        return description;
    }

    public String getEsrb()
    {
        return esrb;
    }

    public String getEsrbDescriptor()
    {
        return esrbDescriptor;
    }

    public String getEsrbSummary()
    {
        return esrbSummary;
    }

    public String getTags()
    {
        return tags;
    }

    public String getTitle()
    {
        return title;
    }



    public String getGenres()
    {
        return genres;
    }

    public void setFileName(String s)
    {
        fileName = s;
    }

    public void setConsole(String s)
    {
        con = s;
    }

    public void setConsole(int s)
    {
        fav = s;
    }

    public void setReleaseDate(String s)
    {
        releaseDate = s;
    }

    public void setPublisher(String s)
    {
        publisher = s;
    }

    public void setDeveloper(String s)
    {
        developer = s;
    }

    public void setUserScore(String s)
    {
        userScore = s;
    }

    public void setDescription(String s)
    {
        description = s;
    }

    public void setCriticScore(String s)
    {
        criticScore = s;
    }

    public void setTrivia(String s)
    {
        trivia = s;
    }

    public void setPlayers(String s)
    {
        players = s;
    }

    public void setGenres(String s)
    {
        genres = s;
    }

    public void setTags(String s)
    {
        tags = s;
    }

    public void setEsrb(String s)
    {
        esrb = s;
    }

    public void setEsrbDescriptors(String s)
    {
        esrbDescriptor = s;
    }

    public void setEsrbSummary(String s)
    {
        esrbSummary = s;
    }
}
