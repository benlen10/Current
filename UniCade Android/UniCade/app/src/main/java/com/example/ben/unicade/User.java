package com.example.ben.unicade;
import java.util.ArrayList;

public class User {

    //region Properties

    private String userName;
    private String pass;
    public int loginCount;
    public int totalLaunchCount;
    private String userInfo;
    private String allowedEsrb;
    private String email;
    private String profPic;
    public ArrayList<String> favorites;

    //endregion

    //region Constructors

    /*
    Summary: Basic constructor for the User class
     */
    public User()
    {
        this.userName = "New User";
    }

    /*
    Summary: Extended constructor for the User class
     */
    public User(String userName, String pass, int loginCount, String email, int totalLaunchCount, String userInfo, String allowedEsrb, String profPic)
    {
        this.userName = userName;
        this.pass = pass;
        this.loginCount = loginCount;
        this.totalLaunchCount = totalLaunchCount;
        this.userInfo = userInfo;
        this.allowedEsrb = allowedEsrb;
        this.email = email;
        this.profPic = profPic;
        favorites = new ArrayList();
    }

    //endregion

    //region Getters

    public String getUsername()
    {
        return userName;
    }

    public String getPass()
    {
        return pass;
    }

    public String getEmail()
    {
        return email;
    }

    public String getProfPic()
    {
        return profPic;
    }

    public int getLoginCount()
    {
        return loginCount;
    }

    public int getLaunchCount()
    {
        return totalLaunchCount;
    }

    public String getUserInfo()
    {
        return userInfo;
    }

    public String getAllowedEsrb()
    {
        return allowedEsrb;
    }

    //endregion

    //region Setters

    public void setName(String s)
    {
        userName = s;
    }

    public void setpass(String s)
    {
        pass = s;
    }

    public void setEmail(String s)
    {
        email = s;
    }

    public void setProfPic(String s)
    {
        profPic = s;
    }

    public void setUserInfo(String s)
    {
        userInfo = s;
    }

    public void setAllowedEsrb(String s)
    {
        allowedEsrb = s;
    }

    //endregion
}
