package com.example.ben.unicade;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;


import javax.sql.DataSource;

/**
 * Created by Ben on 12/17/2015.
 */
public class SQLclass {

    Connection conn = null;

    public static void connectSql() {
        String url = "jdbc:mysql://72.33.70.240:3306/";
        String dbName = "unicade";
        String driver = "com.mysql.jdbc.Driver";
        String userName = "root";
        String password = "Star6120";
        try {
            Class.forName(driver).newInstance();
            Connection conn = DriverManager.getConnection(url + dbName, userName, password);
            Statement st = conn.createStatement();
            ResultSet res = st.executeQuery("SELECT * FROM  ben_games");
            while (res.next()) {
                int id = res.getInt("id");
                System.out.println(id);
            }

            conn.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
