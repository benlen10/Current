#include <stdio.h>
#include "sqlite3.h"

int main(int argc, char* argv[])
{
  sqlite3 *db;
  int conn;

  conn = sqlite3_open("sample.db", &db); //sqlite3 api

  if( conn ){
    fprintf(stderr, "Unable to open the database: %s\n", sqlite3_errmsg(db)); //sqlite3 api
    return(0);
  }else{
    fprintf(stderr, "database opened successfully\n");
  }
  sqlite3_close(db); //sqlite3 api
}

//CREATE TABLE COMMANDS
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
