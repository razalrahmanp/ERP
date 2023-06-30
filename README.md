# ERP

In this updated code, two new methods are added to the  Restaurant  class:  SaveMenuToDatabase  and  LoadMenuFromDatabase . The  SaveMenuToDatabase  method uses SQL queries to clear the existing menu items table and insert the current menu items into the database. The  LoadMenuFromDatabase  method retrieves menu items from the database and populates the  Menu  list. 
 
Please make sure to replace "YourConnectionString" with the actual connection string for your SQL Server database.


UI Implemented

In this updated code, a  Login  method is added to handle the authentication logic. The user is prompted to enter a username and password, and the method checks if the provided credentials match the expected admin credentials. If the login is successful, the admin section is displayed with options to save the menu to the database, load the menu from the database, display the menu, or exit. If the login fails, access to the admin section is denied. 
 
Please note that the authentication logic in the  Login  method is simplified for demonstration purposes. In a real-world scenario, you would typically use more secure and robust authentication mechanisms.
