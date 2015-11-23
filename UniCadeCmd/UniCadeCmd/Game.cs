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
        public int launchCount;
        public string title;
        public string description;
        public int releaseDate;
        public string publisher;
        public string developer;
        public string[] geners;
        public string[] tags;
        public int userScore;
        public int criticScore;
        public string trivia;
        public int players;
        public string esrb;
        public string esrbDescriptor;
        public string esrbSummary;

        //Methods 


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
            this.geners = geners;
            this.tags = tags;
            title = fileName.Substring(0, fileName.IndexOf('.'));
        }

        public string getFileName()
        {
            return fileName;
        }

        public string getConsole()
        {
            return con;
        }

        public void setFileName(string s)
        {
            fileName = s;
        }

        public void setConsole(string s)
        {
            con = s;
        }




    }

    
}
