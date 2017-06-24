/**
* @author Benjamin Lenington (lenington@wisc.edu)
*
* @section LICENSE
* Copyright (c) 2012 Database Group, Computer Sciences Department, University of Wisconsin-Madison.
*/

#include <stdio.h>
#include "sqlite3.h"
#include "load.h"
#include <iostream>
#include <fstream>
#include <cstring>
#include <algorithm>

//The max number of chars to parse per line
const int BUFFER_SIZE = 2000;

//Current sqlite database instance
sqlite3 *db;

int main(int argc, char* argv[])
{
	//Open new SQL connection
	int conn;
	conn = sqlite3_open("nutrients.db", &db);

	if (conn) {
		fprintf(stderr, "Unable to open the database: %s\n", sqlite3_errmsg(db)); //sqlite3 api
		return(0);
	}
	else {
		fprintf(stderr, "database opened successfully\n");
	}

	//Generate the database tables
	generateTable();

	//Populate the tables with data from USDA database text files 
	populateTable();

	//Close the database once operation is complete
	sqlite3_close(db);
}

/**
* Generate a new sqlite3 tables for the USDA Nutrition database
**/
void generateTable() {

	//Char pointer to store the CREATE TABLE commands
	std::string command;

	//Stores the retun status value after executing the SQL commands
	int execStatus;

	//Stores the error message string after executing the SQL commands
	char * errorMsg = 0;

	//Define the CREATE TABLE command for the FoodDescriptions table
	command = "DROP TABLE IF EXISTS FoodDescriptions;\
	CREATE TABLE FoodDescriptions(\
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
	command = "DROP TABLE IF EXISTS FoodGroupDescriptions;\
   CREATE TABLE FoodGroupDescriptions(\
   FdGrp_Cd   CHAR (4)     NOT NULL,\
   FdGrp_Desc VARCHAR (60) NOT NULL,\
   PRIMARY KEY (FdGrp_Cd),\
   FOREIGN KEY (FdGrp_Cd) REFERENCES FoodDescriptions\
);";

	//Execute the SQL command and create the FoodGroupDescriptions table 
	execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

	//Define the CREATE TABLE command for the LangualFactor table
	command = "DROP TABLE IF EXISTS LangualFactor;\
   CREATE TABLE LangualFactor(\
   NDB_No   CHAR (5)     NOT NULL,\
   Factor_Code CHAR (5)  NOT NULL,\
   PRIMARY KEY (NDB_No, Factor_Code),\
   FOREIGN KEY (NDB_No) REFERENCES FoodDescriptions,\
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactorsDescription\
);";

	//Execute the SQL command and create the LangualFactor table 
	execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

	//Define the CREATE TABLE command for the LangualFactorsDescription table
	command = "DROP TABLE IF EXISTS LangualFactorsDescription;\
   CREATE TABLE LangualFactorsDescription(\
   Factor_Code   CHAR (5)       NOT NULL,\
   Description   VARCHAR (140)  NOT NULL,\
   PRIMARY KEY (Factor_Code),\
   FOREIGN KEY (Factor_Code) REFERENCES LangualFactor\
   );";

	//Execute the SQL command and create the LangualFactorsDescription table 
	execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

	//Define the CREATE TABLE command for the NutrientData table
	command = "DROP TABLE IF EXISTS NutrientData;\
   CREATE TABLE NutrientData(\
   NDB_No   CHAR (5)       NOT NULL,\
   Nutr_No  CHAR (3)       NOT NULL,\
   Nutr_Val  DECIMAL(10,3)  NOT NULL,\
   Num_Data_Pts  DECIMAL(5,0)  NOT NULL,\
   Std_Error  DECIMAL(8,3),\
   Src_Cd  CHAR (2)               NOT NULL,\
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
	command = "DROP TABLE IF EXISTS NutrientDefinitions;\
   CREATE TABLE NutrientDefinitions(\
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
	command = "DROP TABLE IF EXISTS SourceCode;\
   CREATE TABLE SourceCode(\
   Src_Cd     CHAR (2)        NOT NULL,\
   SrcCd_Desc CHAR (60)       NOT NULL,\
   PRIMARY KEY (Src_Cd),\
   FOREIGN KEY (Src_Cd)   REFERENCES NutrientData\
);";

	//Execute the SQL command and create the SourceCode table 
	execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

	//Define the CREATE TABLE command for the DataDerivation table
	command = "DROP TABLE IF EXISTS DataDerivation;\
   CREATE TABLE DataDerivation(\
   Deriv_Cd    CHAR (4)       NOT NULL,\
   Deriv_Desc  CHAR (120)     NOT NULL,\
   PRIMARY KEY (Deriv_Cd),\
   FOREIGN KEY (Deriv_Cd)   REFERENCES NutrientData\
);";

	//Execute the SQL command and create the DataDerivation table 
	execStatus = sqlite3_exec(db, command.c_str(), callback, 0, &errorMsg);

	//Define the CREATE TABLE command for the Weight table
	command = "DROP TABLE IF EXISTS Weight;\
   CREATE TABLE Weight(\
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
	command = "DROP TABLE IF EXISTS Footnote;\
   CREATE TABLE Footnote(\
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
	command = "DROP TABLE IF EXISTS SourcesOfDataLink;\
   CREATE TABLE SourcesOfDataLink(\
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
	command = "DROP TABLE IF EXISTS SourcesOfData;\
   CREATE TABLE SourcesOfData(\
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

/**
* Parse each USDA database text file in the current working directory to populate the tables
**/
void populateTable() {

	//Char pointer to store the SQL commands
	char * command = (char *)malloc(2000);

	//Stores the retun status value after executing the SQL commands
	int execStatus;

	//Stores the error message string after executing the SQL commands
	char * errorMsg = 0;

	//Create a file input stream object
	std::ifstream inputStream;

	//Create buffer
	char strBuffer[BUFFER_SIZE];

	//Initialize an array to store the tokens
	const char * token[20] = {};

	//Open the file and exit if not found
	std::cout << "Parse: Food Descriptions (FOOD_DES.txt)\n\n" << std::endl;
	inputStream.open("FOOD_DES.txt");

	if (!inputStream.good()) {
		fprintf(stderr, "FOOD_DES.txt Not Found\n");
		return;
	}

	std::string commandStr = "INSERT INTO FoodDescriptions VALUES ";
	int loopCount = 0;

	// read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse NDB_No 
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
		if (CHO_Factor != "NULL") {
			CHO_Factor = CHO_Factor.substr(0, CHO_Factor.length() - 1);
		}

		//Generate Insert Statement
		sprintf(command, "(%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s),", NDB_No.c_str(), FdGrp_Cd.c_str(), Long_Desc.c_str(), Shrt_Desc.c_str(), ComName.c_str(), ManufacName.c_str(), Survey.c_str(), Ref_desc.c_str(), Refuse.c_str(), SciName.c_str(), N_Factor.c_str(), Pro_Factor.c_str(), Fat_Factor.c_str(), CHO_Factor.c_str());
		std::string append(command);
		if (NDB_No != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (NDB_No == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO FoodDescriptions VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (FoodDescriptions): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Open the file and exit if not found
	std::cout << "Parse: Food Group Descriptions (FD_GROUP.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("FD_GROUP.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "FD_GROUP.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO FoodGroupDescriptions VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse FdGrp_Cd 
		std::string FdGrp_Cd = parseString(token[0]);

		//Parse FdGrp_Desc 
		std::string FdGrp_Desc = parseString(token[1]);

		//Generate Insert Statement
		sprintf(command, "(%s,%s),", FdGrp_Cd.c_str(), FdGrp_Desc.c_str());
		std::string append(command);
		if (FdGrp_Cd != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (FdGrp_Cd == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO FoodGroupDescriptions VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (FoodGroupDescriptions): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Open the file and exit if not found
	std::cout << "Parse: Langual Factors (LANGUAL.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("LANGUAL.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "LANGUAL.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO LangualFactor VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse FdGrp_Cd 
		std::string NDB_No = parseString(token[0]);

		//Parse FdGrp_Desc 
		std::string Factor_Code = parseString(token[1]);

		//Generate Insert Statement
		sprintf(command, "(%s,%s),", NDB_No.c_str(), Factor_Code.c_str());
		std::string append(command);
		if (NDB_No != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (NDB_No == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO LangualFactor VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (LangualFactor): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Open the file and exit if file is not found
	std::cout << "Parse: Langual Factors Descriptions (LANGDESC.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("LANGDESC.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "LANGDESC.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO LangualFactorsDescription VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse Factor_Code 
		std::string Factor_Code = parseString(token[0]);

		//Parse Description 
		std::string Description = parseString(token[1]);

		//Generate Insert Statement
		sprintf(command, "(%s,%s),", Factor_Code.c_str(), Description.c_str());
		std::string append(command);
		if (Factor_Code != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (Factor_Code == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO LangualFactorsDescription VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (LangualFactorsDescription): %s\n", errorMsg);
			}
		}
		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Open the file and exit if file is not found
	std::cout << "Parse: Nutrient Data (NUT_DATA.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("NUT_DATA.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "NUT_DATA.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO NutrientData VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
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
		std::string Std_Error = parseStringAsDecimal(token[4]);

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
		std::string Low_EB = parseDecimal(token[13]);

		//Parse Up_EB
		std::string Up_EB = parseDecimal(token[14]);

		//Parse Stat_cmt
		std::string Stat_cmt = parseString(token[15]);

		//Parse AddMod_Date
		std::string AddMod_Date = parseStringAndAddQuotes(token[16]);

		//Parse CC (Not yet implemented in current USDA database version)
		std::string CC = "NULL";

		//Generate Insert Statement
		sprintf(command, "(%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s),", NDB_No.c_str(), Nutr_No.c_str(), Nutr_Val.c_str(), Num_Data_Pts.c_str(), Std_Error.c_str(), Src_Cd.c_str(), Deriv_Cd.c_str(), Ref_NDB_No.c_str(), Add_Nutr_Mark.c_str(), Num_Studies.c_str(), Min.c_str(), Max.c_str(), DF.c_str(), Low_EB.c_str(), Up_EB.c_str(), Stat_cmt.c_str(), AddMod_Date.c_str(), CC.c_str());
		std::string append(command);
		if (NDB_No != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (NDB_No == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO NutrientData VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (NutrientData): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Open the file and exit if not found
	std::cout << "Parse: Nutrient Definitions (NUTR_DEF.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("NUTR_DEF.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "NUTR_DEF.txt Not Found\n");
		return;
	}

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse Nutr_No 
		std::string Nutr_No = parseString(token[0]);

		//Parse Units 
		std::string Units = parseString(token[1]);

		//Parse Tagname 
		std::string Tagname = parseString(token[2]);

		//Parse NutrDesc 
		std::string NutrDesc = parseString(token[3]);

		//Parse Num_Dec 
		std::string Num_Dec = parseString(token[4]);

		//Parse SR_Order 
		std::string SR_Order = parseDecimal(token[5]);

		//Generate Insert Statement
		sprintf(command, "INSERT INTO NutrientDefinitions VALUES (%s,%s,%s,%s,%s,%s);", Nutr_No.c_str(), Units.c_str(), Tagname.c_str(), NutrDesc.c_str(), Num_Dec.c_str(), SR_Order.c_str());

		//Execute the SQL command
		if (Nutr_No != "NULL") {
			execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);
		}

		//Check for SQL error status
		if (execStatus != SQLITE_OK) {
			fprintf(stderr, "SQL error (NutrientDefinitions): %s\n", errorMsg);
			fprintf(stderr, "%s\n", command);
		}
	}

	//Open the file and exit if not found
	std::cout << "Parse: Source Code (SRC_CD.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("SRC_CD.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "SRC_CD.txt Not Found\n");
		return;
	}

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse Src_Cd 
		std::string Src_Cd = parseString(token[0]);

		//Parse SrcCd_Desc 
		std::string SrcCd_Desc = parseString(token[1]);

		//Generate Insert Statement
		sprintf(command, "INSERT INTO SourceCode VALUES (%s,%s);", Src_Cd.c_str(), SrcCd_Desc.c_str());

		//Execute the SQL command
		if ((Src_Cd != "NULL")) {
			execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);
		}

		//Check for SQL error status
		if (execStatus != SQLITE_OK) {
			fprintf(stderr, "SQL error (SourceCode): %s\n", errorMsg);
			fprintf(stderr, "%s\n", command);
		}
	}

	//Open the file and exit if not found
	std::cout << "Parse: Data Derivation (DERIV_CD.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("DERIV_CD.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "DERIV_CD.txt Not Found\n");
		return;
	}

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse Deriv_Cd 
		std::string Deriv_Cd = parseString(token[0]);

		//Parse Deriv_Desc 
		std::string Deriv_Desc = parseString(token[1]);

		//Generate Insert Statement
		sprintf(command, "INSERT INTO DataDerivation VALUES (%s,%s);", Deriv_Cd.c_str(), Deriv_Desc.c_str());

		//Execute the SQL command
		if (Deriv_Cd != "NULL") {
			execStatus = sqlite3_exec(db, command, callback, 0, &errorMsg);
		}

		//Check for SQL Errors
		if (execStatus != SQLITE_OK) {
			fprintf(stderr, "SQL error (DataDerivation): %s\n", errorMsg);
			fprintf(stderr, "%s\n", command);
		}
	}

	//Open the file and exit if not found
	std::cout << "Parse: Weight (WEIGHT.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("WEIGHT.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "WEIGHT.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO Weight VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse NDB_No 
		std::string NDB_No = parseString(token[0]);

		//Parse Seq 
		std::string Seq = parseDecimal(token[1]);

		//Parse Amount 
		std::string Amount = parseDecimal(token[2]);

		//Parse Msre_Desc 
		std::string Msre_Desc = parseString(token[3]);

		//Parse Gm_Wgt 
		std::string Gm_Wgt = parseDecimal(token[4]);

		//Parse Num_Data_Pts 
		std::string Num_Data_Pts = parseDecimal(token[5]);

		//Parse Std_Dev 
		std::string Std_Dev = parseDecimal(token[6]);
		Std_Dev = "NULL";

		//Generate Insert Statement
		sprintf(command, "(%s,%s,%s,%s,%s,%s,%s),", NDB_No.c_str(), Seq.c_str(), Amount.c_str(), Msre_Desc.c_str(), Gm_Wgt.c_str(), Num_Data_Pts.c_str(), Std_Dev.c_str());
		std::string append(command);
		if (NDB_No != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (NDB_No == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO WEight VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (WEight): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}


	//Open the file and exit if not found
	std::cout << "Parse: Footnote (FOOTNOTE.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("FOOTNOTE.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "FOOTNOTE.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO Footnote VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse NDB_No 
		std::string NDB_No = parseString(token[0]);

		//Parse Footnt_No 
		std::string Footnt_No = parseString(token[1]);

		//Parse Footnt_Typ 
		std::string Footnt_Typ = parseString(token[2]);

		//Parse Nutr_No 
		std::string Nutr_No = parseString(token[3]);

		//Parse Footnt_Txt 
		std::string Footnt_Txt = parseString(token[4]);

		//Generate Insert Statement
		sprintf(command, "(%s,%s,%s,%s,%s),", NDB_No.c_str(), Footnt_No.c_str(), Footnt_Typ.c_str(), Nutr_No.c_str(), Footnt_Txt.c_str());
		std::string append(command);
		if (NDB_No != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (NDB_No == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO Footnote VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (Footnote): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}


	//Open the file and exit if not found
	std::cout << "Parse: Sources of Data Link (DATSRCLN.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("DATSRCLN.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "DATSRCLN.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO SourcesOfDataLink VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse NDB_No 
		std::string NDB_No = parseString(token[0]);

		//Parse Nutr_No 
		std::string Nutr_No = parseString(token[1]);

		//Parse DataSrc_ID 
		std::string DataSrc_ID = parseString(token[2]);

		//Generate Insert Statement
		sprintf(command, "(%s,%s,%s),", NDB_No.c_str(), Nutr_No.c_str(), DataSrc_ID.c_str());
		std::string append(command);
		if (NDB_No != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (NDB_No == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO SourcesOfDataLink VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (SourcesOfDataLink): %s\n", errorMsg);
			}
		}
		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Open the file and exit if not found
	std::cout << "Parse: Sources of Data (DATA_SRC.txt)\n\n" << std::endl;
	inputStream.close();
	inputStream.open("DATA_SRC.txt");
	if (!inputStream.good()) {
		fprintf(stderr, "DATA_SRC.txt Not Found\n");
		return;
	}

	commandStr = "INSERT INTO SourcesOfData VALUES ";
	loopCount = 0;

	//Read each line of the file
	while (!inputStream.eof())
	{
		//Read a full line
		inputStream.getline(strBuffer, BUFFER_SIZE);

		//Parse all tokens from the line
		const char * const split = "^";
		token[0] = tokenize(strBuffer, split);
		for (int i = 1; i <= 20; i++)
		{
			token[i] = tokenize(NULL, split);
			if (!token[i]) {
				break;
			}
		}

		//Parse DataSrc_ID 
		std::string DataSrc_ID = parseString(token[0]);

		//Parse Authors 
		std::string Authors = parseString(token[1]);

		//Parse Title 
		std::string Title = parseString(token[2]);

		//Parse Year 
		std::string Year = parseString(token[3]);

		//Parse Journal 
		std::string Journal = parseString(token[4]);

		//Parse Vol_City 
		std::string Vol_City = parseString(token[5]);

		//Parse Issue_State 
		std::string Issue_State = parseString(token[6]);

		//Parse Start_Page 
		std::string Start_Page = parseString(token[7]);

		//Parse End_Page 
		std::string End_Page = parseString(token[8]);

		//Generate Insert Statement
		sprintf(command, "(%s,%s,%s,%s,%s,%s,%s,%s,%s),", DataSrc_ID.c_str(), Authors.c_str(), Title.c_str(), Year.c_str(), Journal.c_str(), Vol_City.c_str(), Issue_State.c_str(), Start_Page.c_str(), End_Page.c_str());
		std::string append(command);
		if (DataSrc_ID != "NULL") {
			commandStr += append;
		}

		//Execute the SQL command 
		if (DataSrc_ID == "NULL" || loopCount >= 1000) {
			commandStr.erase(commandStr.size() - 1);
			commandStr += ";";
			execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
			commandStr = "INSERT INTO SourcesOfDataLink VALUES ";
			loopCount = 0;
			if (execStatus != SQLITE_OK) {
				fprintf(stdout, "SQL error (SourcesOfDataLink): %s\n", errorMsg);
			}
		}

		loopCount++;
	}

	//Insert any remaining entries after the loop finishes 
	if (loopCount > 2) {
		commandStr.erase(commandStr.size() - 1);
		commandStr += ";";
		execStatus = sqlite3_exec(db, commandStr.c_str(), callback, 0, &errorMsg);
		loopCount = 0;
	}

	//Cleanup and free memory
	inputStream.close();
	free(command);

	std::cout << "All Tables Successfully Populated\n" << std::endl;
}

#pragma region Helper Functions

/**
* Parse a raw string and replace '~' chars with '\"'
* Set the string to "NULL" if the input is invalid or empty
* @param str			the string to parse
**/
std::string parseString(const char * str) {
	std::string result("NULL");
	if ((str != NULL)) {
		if (strlen(str) > 2) {
			result = str;
			std::replace(result.begin(), result.end(), '\r', ' ');
			std::replace(result.begin(), result.end(), '\"', ' ');
			std::replace(result.begin(), result.end(), '~', '\"');
		}
	}
	return result;
}

/**
* Parse a raw string and return the string representation of a decimal value
* Set the string to "NULL" if the input is invalid or empty
* @param str			the string to parse
**/
std::string parseDecimal(const char * str) {
	std::string result("NULL");
	if ((str != NULL)) {
		if ((strlen(str) >= 1) && (str[0] != '\r')) {
			result = str;
			std::replace(result.begin(), result.end(), '\r', ' ');
			std::replace(result.begin(), result.end(), '\"', ' ');
			std::replace(result.begin(), result.end(), '~', '\"');
		}
	}
	return result;
}

/**
* Parse string (with ~ symbols) as a decimal without quotes
* Set the string to "NULL" if the input is invalid or empty
* @param str			the string to parse
**/
std::string parseStringAsDecimal(const char * str) {
	std::string result("NULL");
	if ((str != NULL)) {
		if (strlen(str) > 2) {
			result = str;
			result = result.substr(1, result.length() - 2);
		}
	}
	return result;
}

/**
* Parse a raw string and add quotes to the string
* Set the string to "NULL" if the input is invalid or empty
* @param str			the string to parse
**/
std::string parseStringAndAddQuotes(const char * str) {
	std::string result("NULL");
	if ((str != NULL)) {
		if (strlen(str) > 2) {
			result = str;
			std::replace(result.begin(), result.end(), '\r', ' ');
			result = ("\"" + result + "\"");
		}
	}
	return result;
}

/**
* A placeholder callback function for the sqlite3_exec function
* @param NotUsed		dummey void pointer
* @param argc			current arg count
* @param argv			function arguments
* @param azColName	    col names for the current SQL call
**/
static int callback(void *NotUsed, int argc, char **argv, char **azColName) {
	return 0;
}

/**
* Modified version of the C strtok function
* If no chars exist between the current delims, return NULL
* @param str			the string to parse
* @param delims			the delim chars to split the string
**/
char * tokenize(char * str, char const * delims)
{
	char  *  tmp, *returnValue = 0;
	static char  * sourceValue = NULL;

	//Set the souceValue to the input string if the param is not NULL
	if (str != NULL) {
		sourceValue = str;
	}
	if (sourceValue == NULL) {
		return NULL;
	}

	//Split the string based upon the delims
	if ((tmp = strpbrk(sourceValue, delims)) != NULL) {
		*tmp = 0;
		returnValue = sourceValue;
		sourceValue = ++tmp;
	}
	//Return NULL if there are no chars between the delims
	else if (*sourceValue) {
		returnValue = sourceValue;
		sourceValue = NULL;
	}
	return returnValue;
}

#pragma endregion
