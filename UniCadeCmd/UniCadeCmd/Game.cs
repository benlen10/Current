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
        public int releaseDate;
        public string publisher;
        public string developer;
        public string[] geners;
        public string[] tags;
        public int userScore;
        public int CriticScore;
        public string trivia;
        public int players;
        public string esrb;
        public string esrbDescriptor;

        //Methods 

        public Game(string fileName, string con, int launchCount)
        {
            this.fileName = fileName;
            this.con = con;
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
