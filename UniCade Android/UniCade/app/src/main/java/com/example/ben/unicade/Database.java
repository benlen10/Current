package com.example.ben.unicade;
import java.util.ArrayList;

public class Database {

    //region Properties

    public ArrayList<Console> consoleList;
    public ArrayList<User> userList;
    public ArrayList reviewList;
    public static int totalGameCount;
    public static int conCount;
    private static String hashKey = "JI3vgsD6Nc6VSMrNw0b4wvuJmDw6Lrld";

    //endregion

    //region Constructors

    /*
    Summary: Public constructor for Database
     */
    public Database() {
        consoleList = new ArrayList();
        userList = new ArrayList();
        reviewList = new ArrayList();
    }

    //endregion

    //region Public Methods

    public static String getHashKey()
    {
        return hashKey;
    }

    //endregion
}
