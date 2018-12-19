This project was created with some pre-existing data from Yelp's database
publically given in prior years (this project does NOT include this data).

Some notes about the files in this project:
- The project used PostgreSQL for the database.
- DDL.sql is creating the tables for the database,
- Trigger.sql contains the trigger statements that deals with automatic updates.
- Update.sql is filled with queries that are ran after the database is filled
    to properly change certain attributes of tables to be their respective
    amounts.
- Indexes.sql contains queries to create indexes on the tables required for
    the scope of the project.
- ParseAndInsertTables is a python file that connects to the database, reads
    the .json data files, and then inserts the tuples into the database.