using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCadeCmd
{
    class Game
    {
        private string fileName;
        private string con;
        private string title;
        private string description;
        private int releaseDate;
        private string publisher;
        private string developer;
        private string[] genres;
        private string[] tags;
        private int userScore;
        private int criticScore;
        private string trivia;
        private int players;
        private string esrb;
        private string esrbDescriptor;
        private string esrbSummary;
        public int launchCount;


        //Basic Constructor
        public Game(string fileName, string con, int launchCount)
        {
            this.fileName = fileName;
            this.con = con;
            title = fileName.Substring(0, fileName.IndexOf('.'));
        }

        //Extended Constuctor 
        public Game(string fileName, string con, int launchCount, string title, int releaseDate, string publisher, string developer, int userScore, int criticScore, int players, string trivia, string esrb, string esrbDescriptor, string esrbSummary, string description, string[] genres, string[] tags)
        {

            this.fileName = fileName;
            this.con = con;
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
            title = fileName.Substring(0, fileName.IndexOf('.'));
        }

        //Methods 

        public string getFileName()
        {
            return fileName;
        }

        public string getConsole()
        {
            return con;
        }

        public int getReleaseDate()
        {
            return releaseDate;
        }

        public int getUserScore()
        {
            return userScore;
        }

        public string getDeveloper()
        {
            return developer;
        }

        public string getPublisher()
        {
            return developer;
        }

        public int getCriticScore()
        {
            return criticScore;
        }

        public int getPlayers()
        {
            return players;
        }

        public string getTrivia()
        {
            return trivia;
        }

        public string getDescription()
        {
            return description;
        }

        public string getEsrb()
        {
            return esrb;
        }

        public string getEsrbDescriptor()
        {
            return esrbDescriptor;
        }

        public string getEsrbSummary()
        {
            return esrbSummary;
        }

        public string[] getTags()
        {
            return tags;
        }

        public string getTitle()
        {
            return title;
        }



        public string[] getGenres()
        {
            return genres;
        }

        public void setFileName(string s)
        {
            fileName = s;
        }

        public void setConsole(string s)
        {
            con = s;
        }

        public void setReleaseDate(int s)
        {
            releaseDate = s;
        }

        public void setPublisher(string s)
        {
            publisher = s;
        }

        public void setDeveloper(string s)
        {
            developer = s;
        }

        public void setUserScore(int s)
        {
            userScore = s;
        }

        public void setCriticScore(int s)
        {
            criticScore = s;
        }

        public void setTrivia(string s)
        {
            trivia = s;
        }

        public void setPlayers(int s)
        {
            players = s;
        }

        public void setGenres(string[] s)
        {
            genres = s;
        }

        public void setTags(string[] s)
        {
            tags = s;
        }

        public void setEsrb(string s)
        {
            esrb = s;
        }

        public void setEsrbDescriptors(string s)
        {
           esrbDescriptor = s;
        }

        public void setEsrbSummary(string s)
        {
            esrbSummary = s;
        }



    }

    
}
