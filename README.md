# Serratus Summary File Parser, API, and Database Generator


- This program has three main parts (intended for separation in the future): the parser (parser.cs), the web API (/controllers), and the Database generator (Domain/Model + ORM).

If you would like to run the parser, add the method "RunParser()" to the Main method in program.cs before the "CreateHostBuilder(args).Build().Run();" method. The parser will run and keep running until all files are parsed.

If you would like to run the API, ensure that only the "CreateHostBuilder(args).Build().Run();" method is present in the Main method in program.cs. This will start up the web API on a localhost server in your browser when ran on Debug/Release IIS Express in Visual Studio.

If you would like to generate a database, ensure all dependencies are up to date and run "Add-Migration $"{desired migration name}" and then "Update-Databse" in the nuget package manager console. This will create a new db migration and apply it to the PostgreSQL db you are connected to in the appsettings.json. 
 
# Anatomy


- Domain/Model and the bedrock of the Database

This program was created with domain driven design in mind. That means that the domain model defined is a direct represetation of the database generated (via EF CORE). The four parts of the domain model are as follows: Run, FamilySection, AccessionSection, and FastaSection. This represents each of the four parts of a summary file present in the lovelylake s3 bucket. To actualize the model, a new migration is created ("Add-Migration $"{desired migration name}"), where these four classes define the resulting database generated. 


- Connecting to a Database

The program connects to a database through use of a Helper.cs file that's only purpose is to generate a connection string in the specific format necessary for connection. The Helper file uses a configuration builder to get the defined credentials from the appsettings.json to format into the connection string. For any PostgresSQL database, use the following credentials:

> "RDS_USERNAME" = database username
> "RDS_PASSWORD" = database password
> "RDS_HOSTNAME" = host name, i.e. the dns location of your RDS database


- Parser 

The parser.cs file contains a parser that reads, serially, all summary files from the s3 bucket lovelywater. The parser connects to aws via your Serratus AWS credentials provided in the appsetings.json in the form of a nested JSON object "Tokens"- 
 
 {
    "Tokens": 
       {
           "AccessToken": "whateveryouraccesstokenis",
           "SecretToken": "donteversharethiswithanyone!!!"
       }
 }
 
 The parser then grabs files in groups of 1000 to parse and add to the database you are connected to in the appsettings.json. 
 
- API

The web API in this program is essentially just the out-of-the-box ASP.NET web API template with custom controllers generated from our domain model. There is one controller for each section of the database (Runs, FamilySections, AccessionSections, and FastaSections) meaning that there are four controllers in total. The controllers come with the basic CRUD commands needed for any rudimentary operations. However, adding custom routes is as easy as following the format present in the controllor: defining the type of request, the desired return object, and the necessary part of the database needed to perform said operation. 

To run the API, ensure that the only method called in the Main of the program file is "CreateHostBuilder(args).Build().Run();" This will generate an IIS Server on your localhost that is able to process requests based on the endpoints.


# Endpoint Documentation

- The endpoints for this api are present in the controller files for each part of the database's domain model. The basic CRUD commands are all present in each file, along with plenty of room for customization.

Example 1: Retrieving all runs

- The endpoint present in the run controller 'https://localhost:44363/api/runs/' will return a JSON file of all runs.

Example 2: Retrieving a nested JSON File with all summary file data

- The endpoint present in the run controller 'https://localhost:44363/api/runs/get-run/DRR000746' will return a nested JSON file representation of the summary file for the SRA 'DRR000746'.
