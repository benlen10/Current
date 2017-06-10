#include <stdio.h>
#include "sqlite3.h"
//Custom
#include "load.h"
#include <iostream>
#include <fstream>
#include <cstring>

const int MAX_CHARS_PER_LINE = 5000;
sqlite3 *db;

int main(int argc, char* argv[])
{
  int conn;

  conn = sqlite3_open("sample.db", &db); //sqlite3 api

  if( conn ){
    fprintf(stderr, "Unable to open the database: %s\n", sqlite3_errmsg(db)); //sqlite3 api
    return(0);
  }else{
    fprintf(stderr, "database opened successfully\n");
  }
  
  //MY CODEE (Body)
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

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

  //Define the CREATE TABLE command for the FoodGroupDescriptions table
  command = "CREATE TABLE FoodGroupDescriptions(\
   FdGrp_Cd   CHAR (4)     NOT NULL,\
   FdGrp_Desc VARCHAR (60) NOT NULL,\
   PRIMARY KEY (FdGrp_Cd),\
   FOREIGN KEY (FdGrp_Cd) REFERENCES FoodDescriptions\
);";

  //Execute the SQL command and create the FoodGroupDescriptions table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the LangualFactor table
  command = "CREATE TABLE LangualFactor(\
   NDB_No   CHAR (5)     NOT NULL,\
   Factor_Code CHAR (5)  NOT NULL,\
   PRIMARY KEY (NDB_No),\
   PRIMARY KEY (Factor_Code),\
   FOREIGN KEY (NDB_No) REFERENCES FoodDescriptions,\
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactorsDescription\
);";

  //Execute the SQL command and create the LangualFactor table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the LangualFactorsDescription table
  command = "CREATE TABLE LangualFactorsDescription(\
   Factor_Code   CHAR (5)       NOT NULL,\
   Description   VARCHAR (140)  NOT NULL,\
   PRIMARY KEY (Factor_Code),\
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactor\
   );";

  //Execute the SQL command and create the LangualFactorsDescription table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

 //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the NutrientData table
  command = "CREATE TABLE NutrientData(\
   NDB_No   CHAR (5)       NOT NULL,\
   Nutr_No  CHAR (3)       NOT NULL,\
   Nutr_Val  DECIMAL(10,3)  NOT NULL,\
   Num_Data_Pts  DECIMAL(5,0)  NOT NULL,\
   Std_Error  DECIMAL(8,3),\
   Src_Cd  CHAR (2) NOT_NULL,\
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
   PRIMARY KEY (NDB_No),\
   PRIMARY KEY (Nutr_No),\
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

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the NutrientDefinitions table
  command = "CREATE TABLE NutrientDefinitions(\
   Nutr_No   CHAR (3)       NOT NULL,\
   Units     CHAR (7)       NOT NULL,\
   Tagname   CHAR(20),\
   NutrDesc  CHAR(60)       NOT NULL,\
   Num_Dec   CHAR(1)        NOT NULL,\
   SR_Order  INT (6)        NOT_NULL,\
   PRIMARY KEY (Nutr_No),\
   FOREIGN KEY (Nutr_No) REFERENCES NutrientData\
);";

  //Execute the SQL command and create the NutrientDefinitions table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   }  

   //Define the CREATE TABLE command for the SourceCode table
  command = "CREATE TABLE SourceCode(\
   Src_Cd     CHAR (2)        NOT NULL,\
   SrcCd_Desc CHAR (60)       NOT NULL,\
   PRIMARY KEY (Src_Cd),\
   FOREIGN KEY (Src_Cd)   REFERENCES NutrientData\
);";

  //Execute the SQL command and create the SourceCode table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the DataDerivation table
  command = "CREATE TABLE DataDerivation(\
   Deriv_Cd    CHAR (4)       NOT NULL,\
   Deriv_Desc  CHAR (120)     NOT NULL,\
   PRIMARY KEY (Deriv_Cd),\
   FOREIGN KEY (Deriv_Cd)   REFERENCES NutrientData\
);";

  //Execute the SQL command and create the DataDerivation table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the Weight table
  command = "CREATE TABLE Weight(\
   NDB_No       CHAR (5)       NOT NULL,\
   Seq          CHAR (2)       NOT NULL,\
   Amount       DECIMAL(5,3)   NOT NULL,\
   Msre_Desc    CHAR(84)       NOT NULL,\
   Gm_Wgt       DECIMAL(7,1)   NOT NULL,\
   Num_Data_Pts INT (3),\
   Std_Dev      DECIMAL(7,3),\
   PRIMARY KEY (NDB_No),\
   PRIMARY KEY (Seq),\
   FOREIGN KEY (NDB_No)   REFERENCES FoodDescriptions,\
   FOREIGN KEY (NDB_No)   REFERENCES NutrientData\
);";

  //Execute the SQL command and create the Weight table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

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

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

   //Define the CREATE TABLE command for the SourcesOfDataLink table
  command = "CREATE TABLE SourcesOfDataLink(\
   NDB_No       CHAR (5)       NOT NULL,\
   Nutr_No      CHAR (3)       NOT NULL,\
   DataSrc_ID   CHAR (6)       NOT NULL,\
   PRIMARY KEY (NDB_No),\
   PRIMARY KEY (Nutr_No),\
   PRIMARY KEY (DataSrc_ID),\
   FOREIGN KEY (NDB_No)       REFERENCES NutrientData,\
   FOREIGN KEY (Nutr_No)      REFERENCES NutrientData,\
   FOREIGN KEY (Nutr_No)      REFERENCES NutrientDefinitions,\
   FOREIGN KEY (DataSrc_ID)   REFERENCES SourcesOfData\
);";

  //Execute the SQL command and create the SourcesOfDataLink table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 

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
   FOREIGN KEY (NDB_No)       REFERENCES NutrientData\
);";

  //Execute the SQL command and create the SourcesOfData table 
  execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

  //DEBUG (Temp)
  if( execStatus != SQLITE_OK ){
      fprintf(stderr, "SQL error: %s\n", errorMsg);
      return;
   } 
}

void populateTable(){

  //Char pointer to store the CREATE TABLE commands
  char * command;

  //Stores the retun status value after executing the SQL commands
  int execStatus;

  //Stores the error message string after executing the SQL commands
  char * errorMsg = 0;

  //Create a file input stream object
  std::ifstream fin;

  //Open the first tile 
  fin.open("data.txt"); // open a file

  //Exit if file is not found
  if (!fin.good()){
  fprintf(stderr, "--- File Not Found\n");
    return;
  }
  
  // read each line of the file
  while (!fin.eof())
  {
    //Read a full line
    char buf[MAX_CHARS_PER_LINE];
    fin.getline(buf, MAX_CHARS_PER_LINE);
    
    //Initialize an array to store the tokens
    const char* token[20] = {}; // 

    //PARSE FOOD DESCRIPTIONS

    //Parse NDB_No
    std::string NDB_No(token[0]);
    NDB_No = NDB_No.substr(1, NDB_No.length());

    //Parse FdGrp_Cd 
    std::string FdGrp_Cd(token[1]);
    FdGrp_Cd = FdGrp_Cd.substr(1, FdGrp_Cd.length());

    //Parse Long_Desc
    std::string Long_Desc(token[2]);
    Long_Desc = Long_Desc.substr(1, Long_Desc.length());

    //Parse Shrt_Desc 
    std::string Shrt_Desc(token[3]);
    Shrt_Desc = Shrt_Desc.substr(1, Shrt_Desc.length());

    //Parse ComName (Check for NULL)
    std::string ComName(token[4]);
    if(strlen(token[4])<3){
      ComName = "";
    }
    ComName = ComName.substr(1, ComName.length());

    //Parse ManufacName (Check for NULL)
    std::string ManufacName(token[5]);
    if(strlen(token[5])<3){
      ManufacName = "";
    }
    ManufacName = ManufacName.substr(1, ManufacName.length());

    //Parse Survey (Check for NULL)
    std::string Survey(token[6]);
    if(strlen(token[6])<3){
      Survey = "";
    }
    Survey = Survey.substr(1, Survey.length());


    //Parse Ref_desc (Check for NULL)
    std::string Ref_desc(token[7]);
    if(strlen(token[7])<3){
      Ref_desc = "";
    }
    Ref_desc = Ref_desc.substr(1, Ref_desc.length());

    //Parse Refuse (Check for NULL)
    std::string Refuse(token[8]);
    if(strlen(token[8])<3){
      Refuse = "";
    }
    Refuse = Refuse.substr(1, Refuse.length());

    //Parse SciName (Check for NULL)
    std::string SciName(token[9]);
    if(strlen(token[9])<3){
      SciName = "";
    }
    SciName = SciName.substr(1, SciName.length());

    //Parse N_Factor (Check for NULL) (Decimal)
    std::string N_Factor(token[10]);
    if(strlen(token[10])<3){
      N_Factor = "";
    }

    //Parse Pro_Factor (Check for NULL) (Decimal)
    std::string Pro_Factor(token[11]);
    if(strlen(token[11])<3){
      Pro_Factor = "";
    }

    //Parse Fat_Factor (Check for NULL) (Decimal)
    std::string Fat_Factor(token[12]);
    if(strlen(token[12])<3){
      Fat_Factor = "";
    }

    //Parse CHO_Factor (Check for NULL) (Decimal)
    std::string CHO_Factor(token[13]);
    if(strlen(token[13])<3){
      CHO_Factor = "";
    }
    //Loop Until All Lines have been parsed
  }
}


      /*
      //ORIGINAL TOKEN LOOP
      int n = 0; 
      for (n = 0; n < 20; n++)
      {
        //Fetch the next token
        token[n] = strtok(0, '^'); 
        if (!token[n]){
          break; 
        } 
      }
      */




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
   Src_Cd  CHAR (2) NOT_NULL,
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
   SR_Order  INT (6)        NOT_NULL,
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
