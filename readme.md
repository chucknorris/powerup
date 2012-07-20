SqlBaseline
-----------
SqlBaseline is a tool to extract entities from SQL Server. It has been developed for use with 
 [RoundhousE](https://github.com/chucknorris/roundhouse), the database versioning tool.
 
Usage
-----
Run `sqlbaseline.exe` from the command prompt, passing in the SQL Severname, database name and
a path for the output file.

		.\sqlbaseline.exe -s:"(local)" -d:AdventureWorks -o:C:\Db\Adventure
		
Notes
-----
The goal of SqlBaseline extract all the entities from a database ready for use with
Roundhouse. These are the Anytime scripts and SqlBaseline will apply the second template
described [here](https://github.com/chucknorris/roundhouse/wiki/Anytimescripts)

For a walkthrough see this blog post http://keithbloom.blogspot.co.uk/2012/05/roundhouse-with-legacy-database.html