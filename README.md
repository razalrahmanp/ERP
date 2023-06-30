# ERP

This C# code is a console application that allows a user to manage a restaurant's menu. The user can add menu items, save the menu to a database, load the menu from a database, and display the menu. The application also includes a basic login feature that checks if the user is an admin before allowing them to interact with the menu.
 Here is a step-by-step explanation of the code:
 1. The  `Restaurant`  class represents a restaurant. It has a name and a menu, which is a list of  `MenuItem`  objects. The  `MenuItem`  class represents a menu item, which has a name and a price.
 2. The  `Restaurant`  class has a method  `AddMenuItem`  that creates a new  `MenuItem`  and adds it to the restaurant's menu.
 3. The  `SaveMenuToDatabase`  method of the  `Restaurant`  class saves the restaurant's menu to a database. It opens a database connection, and for each menu item, it tries to update the item in the database. If the update affects no rows (meaning the item doesn't exist in the database), it inserts the item into the database.
 4. The  `LoadMenuFromDatabase`  method of the  `Restaurant`  class loads the restaurant's menu from a database. It opens a database connection, selects all menu items from the database, and replaces the restaurant's current menu with the items from the database.
 5. The  `DisplayMenu`  method of the  `Restaurant`  class displays the restaurant's menu in the console.
 6. The  `Program`  class contains the  `Main`  method, which is the entry point of the application. It creates a new  `Restaurant` , adds some menu items, and then enters a loop where it asks the user what they want to do (save the menu to the database, load the menu from the database, display the menu, or exit).
 7. The  `Login`  method of the  `Program`  class checks if the user is an admin by asking for a username and password and comparing them to hardcoded values. The password is hashed using SHA256 before comparison.
 8. The  `ReadPassword`  method of the  `Program`  class securely reads the password from the console by masking the input with asterisks.
 9. The  `ComputeSha256Hash`  method of the  `Program`  class computes the SHA256 hash of a string. It is used to hash the password before comparison.
