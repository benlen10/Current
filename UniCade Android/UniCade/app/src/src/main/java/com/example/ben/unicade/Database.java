package com.example.ben.unicade;
import java.util.ArrayList;

/**
 * Created by Ben on 12/17/2015.
 */
public class Database {
    public ArrayList<Console> consoleList;
    public ArrayList<User> userList;
    public ArrayList reviewList;
    public static int totalGameCount;
    private static String hashKey = "JI3vgsD6Nc6VSMrNw0b4wvuJmDw6Lrld";

    public Database() {
        consoleList = new ArrayList();
        userList = new ArrayList();
        reviewList = new ArrayList();
    }

    public static String getHashKey()
    {
        return hashKey;
    }
}
