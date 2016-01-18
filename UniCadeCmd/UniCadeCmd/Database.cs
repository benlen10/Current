using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UniCadeCmd
{
    class Database
    {
        public ArrayList consoleList;
        public ArrayList userList;
        public ArrayList reviewList;
        public static int totalGameCount;
        private static string hashKey = "JI3vgsD6Nc6VSMrNw0b4wvuJmDw6Lrld";

    public Database() {
            consoleList = new ArrayList();
            userList = new ArrayList();
            reviewList = new ArrayList();
        }

        public static string getHashKey()
        {
            return hashKey;
        }

    }
}
