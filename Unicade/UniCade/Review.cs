using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCade
{
    class Review
    {
        private string console;
        private string gameTitle;
        private int score;
        private string reviewText;
        private int reccomend;

        public Review(string console, string gameTitle, int score, string reviewText, int reccomend)
        {
            this.console = console;
            this.gameTitle = gameTitle;
            this.score = score;
            this.reviewText = reviewText;
            this.reccomend = reccomend;
        }

        public string getConsole()
        {
            return console;
        }

        public string getTitle()
        {
            return gameTitle;
        }

        public int getScore()
        {
            return score;
        }

        public int getReccomend()
        {
            return reccomend;
        }

        public void setConsole(string s)
        {
            console = s;
        }

        public void setTitle(string s)
        {
            gameTitle = s;
        }

        public void setScore(int s)
        {
            score = s;
        }

        public void setReviewText(string s)
        {
            reviewText = s;
        }

        public void setReccomend(int s)
        {
            reccomend = s;
        }



    }

    
}
