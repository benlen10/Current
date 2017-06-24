package com.example.ben.unicade;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import android.annotation.SuppressLint;
import android.os.StrictMode;
import android.util.Log;
import com.mysql.jdbc.Driver;


import javax.sql.DataSource;

/**
 * Created by Ben on 12/17/2015.
 */
public class SQLclass {

        String ip = "72.33.2.117/3306";
        String classs = "com.mysql.jdbc.Driver";
        String db = "unicade";
        String un = "root";
        String password = "Star6120";
        @SuppressLint("NewApi")
        public Connection CONN() {

            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
                    .permitAll().build();
            StrictMode.setThreadPolicy(policy);
            Connection conn = null;
            String ConnURL = null;
            try {
                Class.forName("com.mysql.jdbc.Driver").newInstance();
                ConnURL = "jdbc:jtds:sqlserver://" + ip + ";"
                        + "databaseName=" + db + ";user=" + un + ";password="
                        + password + ";";
                conn = DriverManager.getConnection(ConnURL);
            } catch (SQLException se) {
                Log.e("ERRO", se.getMessage());
            } catch (ClassNotFoundException e) {
                Log.e("ERRO", e.getMessage());
            } catch (Exception e) {
                Log.e("ERRO", e.getMessage());
            }
            return conn;
        }
        /*String url = "jdbc:mysql://72.33.2.117:3306/";
        String dbName = "unicade";
        String driver = "com.mysql.jdbc.Driver";
        String userName = "root";
        String password = "Star6120";
        try {
            Class.forName(driver).newInstance();
            Class.forName("com.mysql.jdbc.Driver");
            //DriverManager.getConnection(url + dbName, userName, password);
            Connection conn =  DriverManager.getConnection(url,userName,password);
            Statement st = conn.createStatement();
            ResultSet res = st.executeQuery("SELECT * FROM  ben_games");
            while (res.next()) {
                int id = res.getInt("id");
                System.out.println(id);
            }

            conn.close();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }*/
    }


