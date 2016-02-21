package com.example.ben.unicade;
import java.util.ArrayList;

/**
 * Created by Ben on 12/18/2015.
 */
public class User {

    private String userName;
    private String pass;
    public int loginCount;
    public int totalLaunchCount;
    private String userInfo;
    private String allowedEsrb;
    private String email;
    private String profPic;
    public ArrayList<String> favorites;


    //Methods
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

    public User()
    {
        this.userName = "New User";
    }

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
}
