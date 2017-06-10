/**
 * @author See Contributors.txt for code contributors and overview of BadgerDB.
 *
 * @section LICENSE
 * Copyright (c) 2012 Database Group, Computer Sciences Department, University of Wisconsin-Madison.
 */

#include <stdio.h>
#include "sqlite3.h"
//Custom
#include "load.h"
#include <iostream>
#include <fstream>
#include <cstring>
#include <algorithm>


const int MAX_CHARS_PER_LINE = 5000;
sqlite3 *db;

int main(int argc, char* argv[])
{
  int conn;

  conn = sqlite3_open("sample4.db", &db); //sqlite3 api

  if( conn ){
    fprintf(stderr, "Unable to open the database: %s\n", sqlite3_errmsg(db)); //sqlite3 api
    return(0);
  }else{
    fprintf(stderr, "database opened successfully\n");
  }
  
  //MY CODE (Body)
  generateTable();

  populateTable();

  sqlite3_close(db);
}

static int callback(void *NotUsed, int argc, char **argv, char **azColName){
   int i;
   for(i=0; i<argc; i++){
      printf("%s = %s\n", azColName[i], argv[i] ? argv[i] : "NULL");
   }
   printf("\n");
   return 0;
}

void generateTable(){

  //Char pointer to store the CREATE TABLE commands
  std::string command;

  //Stores the retun status value after executing the SQL commands
  int execStatus;

  //Stores the error message string after executing the SQL commands
  char * errorMsg = 0;

  //Define the CREATE TABLE command for the FoodDescriptions table
  command = "CREATE TABLE FoodDescriptions(\
   NDB_No   CHAR (5)        NOT NULL,\
   FdGrp_Cd CHAR (4)        NOT NULL,\
   Long_Desc  VARCHAR (25)  NOT NULL,\
   Shrt_Desc  VARCHAR (25)  NOT NULL,\
   ComName  VARCHAR (100),\
   ManufacName  VARCHAR (65),\
   Survey  CHAR (1),\
   Ref_desc  VARCHAR (135),\
   Refuse  VARCHAR (2),\
   SciName  VARCHAR (65),\
   N_Factor  DECIMAL (4,2),\
   Pro_Factor  DECIMAL (4,2),\
   Fat_Factor  DECIMAL (4,2),\
   CHO_Factor  DECIMAL (4,2),\
   PRIMARY KEY (NDB_No),\
   FOREIGN KEY (FdGrp_Cd) REFERENCES FoodGroupDescriptions,\
   FOREIGN KEY (NDB_No) REFERENCES NutrientData,\
   FOREIGN KEY (NDB_No) REFERENCES Weight,\
   FOREIGN KEY (NDB_No) REFERENCES Footnote,\
   FOREIGN KEY (NDB_No) REFERENCES LangualFactor\
);";

  //Execute the SQL command and create the FoodDescriptions table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //Define the CREATE TABLE command for the FoodGroupDescriptions table
  command = "CREATE TABLE FoodGroupDescriptions(\
   FdGrp_Cd   CHAR (4)     NOT NULL,\
   FdGrp_Desc VARCHAR (60) NOT NULL,\
   PRIMARY KEY (FdGrp_Cd),\
   FOREIGN KEY (FdGrp_Cd) REFERENCES FoodDescriptions\
);";

  //Execute the SQL command and create the FoodGroupDescriptions table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the LangualFactor table
  command = "CREATE TABLE LangualFactor(\
   NDB_No   CHAR (5)     NOT NULL,\
   Factor_Code CHAR (5)  NOT NULL,\
   PRIMARY KEY (NDB_No, Factor_Code),\
   FOREIGN KEY (NDB_No) REFERENCES FoodDescriptions,\
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactorsDescription\
);";

  //Execute the SQL command and create the LangualFactor table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the LangualFactorsDescription table
  command = "CREATE TABLE LangualFactorsDescription(\
   Factor_Code   CHAR (5)       NOT NULL,\
   Description   VARCHAR (140)  NOT NULL,\
   PRIMARY KEY (Factor_Code),\
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactor\
   );";

  //Execute the SQL command and create the LangualFactorsDescription table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the NutrientData table
  command = "CREATE TABLE NutrientData(\
   NDB_No   CHAR (5)       NOT NULL,\
   Nutr_No  CHAR (3)       NOT NULL,\
   Nutr_Val  DECIMAL(10,3)  NOT NULL,\
   Num_Data_Pts  DECIMAL(5,0)  NOT NULL,\
   Std_Error  DECIMAL(8,3),\
   Src_Cd  CHAR (2) NOT NULL,\
   Deriv_Cd  CHAR (4),\
   Ref_NDB_No  CHAR (5),\
   Add_Nutr_Mark  CHAR (1),\
   Num_Studies  INT(2),\
   Min  DECIMAL (10,3),\
   Max  DECIMAL (10,3),\
   DF  INT (4),\
   Low_EB  DECIMAL (10,3),\
   Up_EB  DECIMAL (10,3),\
   Stat_cmt  CHAR (10),\
   AddMod_Date  CHAR (10),\
   CC  CHAR (1),\
   PRIMARY KEY (NDB_No, Nutr_No),\
   FOREIGN KEY (NDB_No) REFERENCES FoodDescriptions,\
   FOREIGN KEY (Ref_NDB_No) REFERENCES FoodDescriptions,\
   FOREIGN KEY (NDB_No) REFERENCES Weight,\
   FOREIGN KEY (NDB_No) REFERENCES Footnote,\
   FOREIGN KEY (NDB_No) REFERENCES SourcesOfDataLink,\
   FOREIGN KEY (Nutr_No) REFERENCES NutrientDefinitions,\
   FOREIGN KEY (Src_Cd) REFERENCES SourceCode,\
   FOREIGN KEY (Deriv_Cd) REFERENCES DataDerivation\
);";

  //Execute the SQL command and create the NutrientData table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the NutrientDefinitions table
  command = "CREATE TABLE NutrientDefinitions(\
   Nutr_No   CHAR (3)       NOT NULL,\
   Units     CHAR (7)       NOT NULL,\
   Tagname   CHAR(20),\
   NutrDesc  CHAR(60)       NOT NULL,\
   Num_Dec   CHAR(1)        NOT NULL,\
   SR_Order  INT (6)        NOT NULL,\
   PRIMARY KEY (Nutr_No),\
   FOREIGN KEY (Nutr_No) REFERENCES NutrientData\
);";

  //Execute the SQL command and create the NutrientDefinitions table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the SourceCode table
  command = "CREATE TABLE SourceCode(\
   Src_Cd     CHAR (2)        NOT NULL,\
   SrcCd_Desc CHAR (60)       NOT NULL,\
   PRIMARY KEY (Src_Cd),\
   FOREIGN KEY (Src_Cd)   REFERENCES NutrientData\
);";

  //Execute the SQL command and create the SourceCode table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the DataDerivation table
  command = "CREATE TABLE DataDerivation(\
   Deriv_Cd    CHAR (4)       NOT NULL,\
   Deriv_Desc  CHAR (120)     NOT NULL,\
   PRIMARY KEY (Deriv_Cd),\
   FOREIGN KEY (Deriv_Cd)   REFERENCES NutrientData\
);";

  //Execute the SQL command and create the DataDerivation table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the Weight table
  command = "CREATE TABLE Weight(\
   NDB_No       CHAR (5)       NOT NULL,\
   Seq          CHAR (2)       NOT NULL,\
   Amount       DECIMAL(5,3)   NOT NULL,\
   Msre_Desc    CHAR(84)       NOT NULL,\
   Gm_Wgt       DECIMAL(7,1)   NOT NULL,\
   Num_Data_Pts INT (3),\
   Std_Dev      DECIMAL(7,3),\
   PRIMARY KEY (NDB_No,Seq),\
   FOREIGN KEY (NDB_No)   REFERENCES FoodDescriptions,\
   FOREIGN KEY (NDB_No)   REFERENCES NutrientData\
);";

  //Execute the SQL command and create the Weight table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the Footnote table
   command = "CREATE TABLE Footnote(\
   NDB_No       CHAR (5)       NOT NULL,\
   Footnt_No    CHAR (4)       NOT NULL,\
   Footnt_Typ   CHAR(1)        NOT NULL,\
   Nutr_No      CHAR(3),\
   Footnt_Txt   CHAR(200)      NOT NULL,\
   FOREIGN KEY (NDB_No)   REFERENCES FoodDescriptions,\
   FOREIGN KEY (NDB_No)   REFERENCES NutrientData,\
   FOREIGN KEY (Nutr_No)   REFERENCES NutrientDefinitions\
);";

  //Execute the SQL command and create the Footnote table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the SourcesOfDataLink table
  command = "CREATE TABLE SourcesOfDataLink(\
   NDB_No       CHAR (5)       NOT NULL,\
   Nutr_No      CHAR (3)       NOT NULL,\
   DataSrc_ID   CHAR (6)       NOT NULL,\
   PRIMARY KEY (NDB_No, Nutr_No, DataSrc_ID),\
   FOREIGN KEY (NDB_No)       REFERENCES NutrientData,\
   FOREIGN KEY (Nutr_No)      REFERENCES NutrientData,\
   FOREIGN KEY (Nutr_No)      REFERENCES NutrientDefinitions,\
   FOREIGN KEY (DataSrc_ID)   REFERENCES SourcesOfData\
);";

  //Execute the SQL command and create the SourcesOfDataLink table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

   //Define the CREATE TABLE command for the SourcesOfData table
  command = "CREATE TABLE SourcesOfData(\
   DataSrc_ID  CHAR (6)       NOT NULL,\
   Authors     CHAR (255) ,\
   Title       CHAR (255)     NOT NULL,\
   Year        CHAR (4),\
   Journal     CHAR (135),\
   Vol_City    CHAR (16),\
   Issue_State CHAR (5),\
   Start_Page  CHAR (5),\
   End_Page    CHAR (65),\
   PRIMARY KEY (DataSrc_ID),\
   FOREIGN KEY (DataSrc_ID)       REFERENCES SourcesOfDataLink\
);";

  //Execute the SQL command and create the SourcesOfData table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);
}

void populateTable(){

  //Char pointer to store the CREATE TABLE commands
  char * command = (char *) malloc(5000);

  //Stores the retun status value after executing the SQL commands
  int execStatus;

  //Stores the error message string after executing the SQL commands
  char * errorMsg = 0;

  //Create a file input stream object
  std::ifstream fin;

  //Create buffer
  char buf[MAX_CHARS_PER_LINE];


  //Open the FOOD_DES.txt file
  fin.open("FOOD_DES.txt"); 

  //Exit if file is not found
  if (!fin.good()){
  fprintf(stderr, "FOOD_DES.txt Not Found\n");
    return;
  }
  
  
  // read each line of the file
  while (!fin.eof())
  {
    //Read a full line
    fin.getline(buf, MAX_CHARS_PER_LINE);
    
    //Initialize an array to store the tokens
    const char * token[20] = {};
    //Parse all tokens from the line
    const char * const split = "^";
    token[0] = strtok(buf, split); 
    for (int i = 1; i <= 13; i++)
      {
        token[i] = strtok(NULL, split); 
        if (!token[i]){
           break; 
        }
      }

    std::string NDB_No = parseString(token[0]);

    //Parse FdGrp_Cd 
    std::string FdGrp_Cd = parseString(token[1]);

    //Parse Long_Desc
    std::string Long_Desc = parseString(token[2]);

    //Parse Shrt_Desc 
    std::string Shrt_Desc = parseString(token[3]);

    //Parse ComName (Check for NULL)
    std::string ComName = parseString(token[4]);

    //Parse ManufacName (Check for NULL)
    std::string ManufacName = parseString(token[5]);

    //Parse Survey (Check for NULL)
    std::string Survey = parseString(token[6]);

    //Parse Ref_desc (Check for NULL)
    std::string Ref_desc = parseString(token[7]);

    //Parse Refuse (Check for NULL)
    std::string Refuse = parseString(token[8]);

    //Parse SciName (Check for NULL)
    std::string SciName = parseString(token[9]);

    //Parse N_Factor (Check for NULL) (Decimal)
    std::string N_Factor = parseDecimal(token[10]);

    //Parse Pro_Factor (Check for NULL) (Decimal)
    std::string Pro_Factor = parseDecimal(token[11]);

    //Parse Fat_Factor (Check for NULL) (Decimal)
    std::string Fat_Factor = parseDecimal(token[12]);

    //Parse CHO_Factor (Check for NULL) (Decimal)
    std::string CHO_Factor = parseDecimal(token[13]);

    //Generate Insert Statement
    sprintf (command, "INSERT INTO FoodDescriptions VALUES (%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s);", NDB_No.c_str(), FdGrp_Cd.c_str(),FdGrp_Cd.c_str(),Shrt_Desc.c_str(),ComName.c_str(),ManufacName.c_str(),Survey.c_str(),Ref_desc.c_str(),Refuse.c_str(),SciName.c_str(), N_Factor.c_str(), Pro_Factor.c_str(), Fat_Factor.c_str(), CHO_Factor.c_str());
    std::cout << command << std::endl;

    //Execute the SQL command and create the SourcesOfData table 
  execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error (INSERT): %s\n", errorMsg);
   } 
   //End of loop
  }



  //Open the SECOND FILE (FD_GROUP.txt)
  fin.close();
  fin.open("FD_GROUP.txt"); 

  //Exit if file is not found
  if (!fin.good()){
  fprintf(stderr, "FD_GROUP.txt Not Found\n");
    return;
  }
  
  // read each line of the file
  while (!fin.eof())
  {
    //Read a full line
    fin.getline(buf, MAX_CHARS_PER_LINE);
    
    //Initialize an array to store the tokens
    const char * token[20] = {};
    //Parse all tokens from the line
    const char * const split = "^";
    token[0] = strtok(buf, split); 
    for (int i = 1; i <= 13; i++)
      {
        token[i] = strtok(NULL, split); 
        if (!token[i]){
           break; 
        }
      }

    //Parse FdGrp_Cd 
    std::string FdGrp_Cd = parseString(token[0]);

    //Parse FdGrp_Desc 
    std::string FdGrp_Desc = parseString(token[1]);

    //Break once end of file is reached
    if((FdGrp_Desc == "NULL") || (FdGrp_Cd == "NULL")){
      break;
    }

    //Generate Insert Statement
    sprintf (command, "INSERT INTO FoodGroupDescriptions VALUES (%s,%s);", FdGrp_Cd.c_str(), FdGrp_Desc.c_str());
    std::cout << command << std::endl;

    //Execute the SQL command and create the SourcesOfData table 
  execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error (INSERT2): %s\n", errorMsg);
   } 
   //End of loop
  }

  //Open the THIRD FILE (LANGUAL.txt)
  fin.close();
  fin.open("LANGUAL.txt"); 

  //Exit if file is not found
  if (!fin.good()){
  fprintf(stderr, "LANGUAL.txt Not Found\n");
    return;
  }
  
  // read each line of the file
  while (!fin.eof())
  {
    //Read a full line
    fin.getline(buf, MAX_CHARS_PER_LINE);
    
    //Initialize an array to store the tokens
    const char * token[20] = {};
    //Parse all tokens from the line
    const char * const split = "^";
    token[0] = strtok(buf, split); 
    for (int i = 1; i <= 13; i++)
      {
        token[i] = strtok(NULL, split); 
        if (!token[i]){
           break; 
        }
      }

    //Parse FdGrp_Cd 
    std::string NDB_No = parseString(token[0]);

    //Parse FdGrp_Desc 
    std::string Factor_Code = parseString(token[1]);

    //Break once end of file is reached (Neither can be null)
    if((NDB_No == "NULL") || (Factor_Code == "NULL")){
      break;
    }

    //Generate Insert Statement
    sprintf (command, "INSERT INTO LangualFactor VALUES (%s,%s);", NDB_No.c_str(), Factor_Code.c_str());
    std::cout << command << std::endl;

    //Execute the SQL command 
  execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error (INSERT3): %s\n", errorMsg);
   } 
   //End of loop
  }

  //Open the FOURTH FILE (LANGDESC.txt)
  fin.close();
  fin.open("LANGDESC.txt"); 

  //Exit if file is not found
  if (!fin.good()){
  fprintf(stderr, "LANGDESC.txt Not Found\n");
    return;
  }
  
  // read each line of the file
  while (!fin.eof())
  {
    //Read a full line
    fin.getline(buf, MAX_CHARS_PER_LINE);
    
    //Initialize an array to store the tokens
    const char * token[20] = {};
    //Parse all tokens from the line
    const char * const split = "^";
    token[0] = strtok(buf, split); 
    for (int i = 1; i <= 13; i++)
      {
        token[i] = strtok(NULL, split); 
        if (!token[i]){
           break; 
        }
      }

    //Parse Factor_Code 
    std::string Factor_Code = parseString(token[0]);

    //Parse Description 
    std::string Description = parseString(token[1]);

    //Break once end of file is reached (Neither can be null) (Optional break statement)
    if((Factor_Code == "NULL") || (Description == "NULL")){
      break;
    }

    //Generate Insert Statement
    sprintf (command, "INSERT INTO LangualFactorsDescription VALUES (%s,%s);", Factor_Code.c_str(), Description.c_str());
    std::cout << command << std::endl;

    //Execute the SQL command
  execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error (INSERT4): %s\n", errorMsg);
   } 
   //End of loop
  }

  //Open the FIFTH FILE (NUT_DATA.txt)
  fin.close();
  fin.open("NUT_DATA.txt"); 

  //Exit if file is not found
  if (!fin.good()){
  fprintf(stderr, "NUT_DATA.txt Not Found\n");
    return;
  }
  
  // read each line of the file
  while (!fin.eof())
  {
    //Read a full line
    fin.getline(buf, MAX_CHARS_PER_LINE);
    
    //Initialize an array to store the tokens
    const char * token[20] = {};
    //Parse all tokens from the line
    const char * const split = "^";
    token[0] = strtok(buf, split); 
    for (int i = 1; i <= 13; i++)
      {
        token[i] = strtok(NULL, split); 
        if (!token[i]){
           break; 
        }
      }

    //Parse NDB_No
    std::string NDB_No = parseString(token[0]);

    //Parse Nutr_No
    std::string Nutr_No = parseString(token[1]);

    //Parse Nutr_Val
    std::string Nutr_Val = parseDecimal(token[2]);

    //Parse Num_Data_Pts
    std::string Num_Data_Pts = parseDecimal(token[3]);

    //Parse Std_Error
    std::string Std_Error = parseDecimal(token[4]);

    //Parse Src_Cd
    std::string Src_Cd = parseString(token[5]);

    //Parse Deriv_Cd
    std::string Deriv_Cd = parseString(token[6]);

    //Parse Ref_NDB_No
    std::string Ref_NDB_No = parseString(token[7]);

    //Parse Add_Nutr_Mark
    std::string Add_Nutr_Mark = parseString(token[8]);

    //Parse Num_Studies
    std::string Num_Studies = parseDecimal(token[9]);

    //Parse Min
    std::string Min = parseDecimal(token[10]);

    //Parse Max
    std::string Max = parseDecimal(token[11]);

    //Parse DF
    std::string DF = parseDecimal(token[12]);

    //Parse Low_EB
    std::string Low_EB = parseString(token[13]);

    //Parse Up_EB
    std::string Up_EB = parseString(token[14]);

    //Parse Stat_cmt
    std::string Stat_cmt = parseString(token[15]);

    //Parse AddMod_Date
    std::string AddMod_Date = parseString(token[16]);

    //Parse CC
    std::string CC = parseString(token[17]);

    //Generate Insert Statement
    sprintf (command, "INSERT INTO NutrientData VALUES (%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s);", NDB_No.c_str(), Nutr_No.c_str(), Nutr_Val.c_str(), Num_Data_Pts.c_str(), Std_Error.c_str(), Src_Cd.c_str(), Deriv_Cd.c_str(), Ref_NDB_No.c_str(), Add_Nutr_Mark.c_str(), Num_Studies.c_str(), Min.c_str(), Max.c_str(), DF.c_str(), Low_EB.c_str(), Up_EB.c_str(), Stat_cmt.c_str(), AddMod_Date.c_str(), CC.c_str());
    std::cout << command << std::endl;

    //Execute the SQL command
  execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error (INSERT5): %s\n", errorMsg);
   } 
   //End of loop
  }


}


std:: string parseString(const char * str){
    std::string result("NULL");
    if(str!=NULL){
    if(strlen(str)>2){
      result = str;
      std::replace( result.begin(), result.end(), '~', '\"');
    }
    }
    return result;
}


std:: string parseDecimal(const char * str){
  std::string result("NULL");
    if(str != NULL){
      result = str;
    }
    return result;
}




//CREATE TABLE COMMANDS (ORIGINAL - DUPLICATE)
/*
CREATE TABLE FoodDescriptions(
   NDB_No   CHAR (5)        NOT NULL,
   FdGrp_Cd CHAR (4)        NOT NULL,
   Long_Desc  VARCHAR (25)  NOT NULL,
   Shrt_Desc  VARCHAR (25)  NOT NULL,
   ComName  VARCHAR (100),
   ManufacName  VARCHAR (65),
   Survey  CHAR (1),
   Ref_desc  VARCHAR (135),
   Refuse  VARCHAR (2),
   SciName  VARCHAR (65),
   N_Factor  DECIMAL (4,2),
   Pro_Factor  DECIMAL (4,2),
   Fat_Factor  DECIMAL (4,2),
   CHO_Factor  DECIMAL (4,2),
   PRIMARY KEY (NDB_No),
   FOREIGN KEY (FdGrp_Cd) REFERENCES FoodGroupDescriptions,
   FOREIGN KEY (NDB_No) REFERENCES NutrientData,
   FOREIGN KEY (NDB_No) REFERENCES Weight,
   FOREIGN KEY (NDB_No) REFERENCES Footnote,
   FOREIGN KEY (NDB_No) REFERENCES LangualFactor
);

CREATE TABLE FoodGroupDescriptions(
   FdGrp_Cd   CHAR (4)     NOT NULL,
   FdGrp_Desc VARCHAR (60) NOT NULL,
   PRIMARY KEY (FdGrp_Cd),
   FOREIGN KEY (FdGrp_Cd) REFERENCES FoodDescriptions
);

CREATE TABLE LangualFactor(
   NDB_No   CHAR (5)     NOT NULL,
   Factor_Code CHAR (5)  NOT NULL,
   PRIMARY KEY (NDB_No),
   PRIMARY KEY (Factor_Code),
   FOREIGN KEY (NDB_No) REFERENCES FoodDescriptions,
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactorsDescription
);

CREATE TABLE LangualFactorsDescription(
   Factor_Code   CHAR (5)       NOT NULL,
   Description   VARCHAR (140)  NOT NULL,
   PRIMARY KEY (Factor_Code),
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactor
);

CREATE TABLE NutrientData(
   NDB_No   CHAR (5)       NOT NULL,
   Nutr_No  CHAR (3)       NOT NULL,
   Nutr_Val  DECIMAL(10,3)  NOT NULL,
   Num_Data_Pts  DECIMAL(5,0)  NOT NULL,
   Std_Error  DECIMAL(8,3),
   Src_Cd  CHAR (2) NOT NULL,
   Deriv_Cd  CHAR (4),
   Ref_NDB_No  CHAR (5),
   Add_Nutr_Mark  CHAR (1),
   Num_Studies  INT(2),
   Min  DECIMAL (10,3),
   Max  DECIMAL (10,3),
   DF  INT (4),
   Low_EB  DECIMAL (10,3),
   Up_EB  DECIMAL (10,3),
   Stat_cmt  CHAR (10),
   AddMod_Date  CHAR (10),
   CC  CHAR (1),
   PRIMARY KEY (NDB_No),
   PRIMARY KEY (Nutr_No),
   FOREIGN KEY (NDB_No) REFERENCES FoodDescriptions
   FOREIGN KEY (Ref_NDB_No) REFERENCES FoodDescriptions
   FOREIGN KEY (NDB_No) REFERENCES Weight
   FOREIGN KEY (NDB_No) REFERENCES Footnote,
   FOREIGN KEY (NDB_No) REFERENCES SourcesOfDataLink
   FOREIGN KEY (Nutr_No) REFERENCES NutrientDefinitions,
   FOREIGN KEY (Src_Cd) REFERENCES SourceCode,
   FOREIGN KEY (Deriv_Cd) REFERENCES DataDerivation
);

CREATE TABLE NutrientDefinitions(
   Nutr_No   CHAR (3)       NOT NULL,
   Units     CHAR (7)       NOT NULL,
   Tagname   CHAR(20),
   NutrDesc  CHAR(60)       NOT NULL,
   Num_Dec   CHAR(1)        NOT NULL,
   SR_Order  INT (6)        NOT NULL,
   PRIMARY KEY (Nutr_No),
   FOREIGN KEY (Nutr_No) REFERENCES NutrientData
);

CREATE TABLE SourceCode(
   Src_Cd     CHAR (2)        NOT NULL,
   SrcCd_Desc CHAR (60)       NOT NULL,
   PRIMARY KEY (Src_Cd),
   FOREIGN KEY (Src_Cd)   REFERENCES NutrientData
);

CREATE TABLE DataDerivation(
   Deriv_Cd    CHAR (4)       NOT NULL,
   Deriv_Desc  CHAR (120)     NOT NULL,
   PRIMARY KEY (Deriv_Cd),
   FOREIGN KEY (Deriv_Cd)   REFERENCES NutrientData
);

CREATE TABLE Weight(
   NDB_No       CHAR (5)       NOT NULL,
   Seq          CHAR (2)       NOT NULL,
   Amount       DECIMAL(5,3)   NOT NULL,
   Msre_Desc    CHAR(84)       NOT NULL,
   Gm_Wgt       DECIMAL(7,1)   NOT NULL,
   Num_Data_Pts INT (3),
   Std_Dev      DECIMAL(7,3),
   PRIMARY KEY (NDB_No),
   PRIMARY KEY (Seq),
   FOREIGN KEY (NDB_No)   REFERENCES FoodDescriptions,
   FOREIGN KEY (NDB_No)   REFERENCES NutrientData
);

CREATE TABLE Footnote(
   NDB_No       CHAR (5)       NOT NULL,
   Footnt_No    CHAR (4)       NOT NULL,
   Footnt_Typ   CHAR(1)        NOT NULL,
   Nutr_No      CHAR(3),
   Footnt_Txt   CHAR(200)      NOT NULL,
   FOREIGN KEY (NDB_No)   REFERENCES FoodDescriptions,
   FOREIGN KEY (NDB_No)   REFERENCES NutrientData,
   FOREIGN KEY (Nutr_No)   REFERENCES NutrientDefinitions
);

CREATE TABLE SourcesOfDataLink(
   NDB_No       CHAR (5)       NOT NULL,
   Nutr_No      CHAR (3)       NOT NULL,
   DataSrc_ID   CHAR (6)       NOT NULL,
   PRIMARY KEY (NDB_No),
   PRIMARY KEY (Nutr_No),
   PRIMARY KEY (DataSrc_ID),
   FOREIGN KEY (NDB_No)       REFERENCES NutrientData,
   FOREIGN KEY (Nutr_No)      REFERENCES NutrientData,
   FOREIGN KEY (Nutr_No)      REFERENCES NutrientDefinitions,
   FOREIGN KEY (DataSrc_ID)   REFERENCES SourcesOfData
);

CREATE TABLE SourcesOfData(
   DataSrc_ID  CHAR (6)       NOT NULL,
   Authors     CHAR (255) ,
   Title       CHAR (255)     NOT NULL,
   Year        CHAR (4),
   Journal     CHAR (135),
   Vol_City    CHAR (16),
   Issue_State CHAR (5),
   Start_Page  CHAR (5),
   End_Page    CHAR (65),
   PRIMARY KEY (DataSrc_ID),
   FOREIGN KEY (NDB_No)       REFERENCES NutrientData
);
*/
