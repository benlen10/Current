using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCadeCmd
{
    public class Game
    {
        private string fileName;
        private string con;
        private string title;
        private string description;
        private string releaseDate;
        private string publisher;
        private string developer;
        private string genres;
        private string tags;
        private string userScore;
        private string criticScore;
        private string trivia;
        private string players;
        private string esrb;
        private string esrbDescriptor;
        private string esrbSummary;
        private int fav;
        public int launchCount;


        //Basic Constructor
        public Game(string fileName, string con, int launchCount)
        {
            this.fileName = fileName;
            this.con = con;
            title = fileName.Substring(0, fileName.IndexOf('.'));
            description = "";
            releaseDate = "";
            publisher = "";
            developer = "";
            userScore = "";
            criticScore = "";
            trivia = "";
            players = "";
            esrb = "";
            esrbDescriptor = "";
            esrbSummary = "";

        }

        //Extended Constuctor 
        public Game(string fileName, string con, int launchCount, string releaseDate, string publisher, string developer, string userScore, string criticScore, string players, string trivia, string esrb, string esrbDescriptor, string esrbSummary, string description, string genres, string tags, int fav)
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
            if (fileName.Length > 2)
            {
                title = fileName.Substring(0, fileName.IndexOf('.'));
            }
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

        public string getReleaseDate()
        {
            return releaseDate;
        }

        public string getUserScore()
        {
            return userScore;
        }

        public string getDeveloper()
        {
            return developer;
        }

        public string getPublisher()
        {
            return publisher;
        }

        public string getCriticScore()
        {
            return criticScore;
        }

        public string getPlayers()
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

        public string getTags()
        {
            return tags;
        }

        public string getTitle()
        {
            return title;
        }



        public string getGenres()
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

        public void setConsole(int s)
        {
            fav = s;
        }

        public void setReleaseDate(string s)
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

        public void setUserScore(string s)
        {
            userScore = s;
        }

        public void setDescription(string s)
        {
            description = s;
        }

        public void setCriticScore(string s)
        {
            criticScore = s;
        }

        public void setTrivia(string s)
        {
            trivia = s;
        }

        public void setPlayers(string s)
        {
            players = s;
        }

        public void setGenres(string s)
        {
            genres = s;
        }

        public void setTags(string s)
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
