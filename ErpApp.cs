
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
 // Class representing a restaurant
class Restaurant
{
    // Properties of the restaurant
    public string Name { get; set; }
    public List<MenuItem> Menu { get; set; }
     // Constructor for the restaurant
    public Restaurant(string name)
    {
        Name = name;
        Menu = new List<MenuItem>();
    }
     // Method to add a menu item to the restaurant's menu
    public void AddMenuItem(string name, decimal price)
    {
        MenuItem item = new MenuItem(name, price);
        Menu.Add(item);
    }
     // Method to save the restaurant's menu to a database
    public void SaveMenuToDatabase(string connectionString)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
         // Update or insert each menu item to the database
        foreach (MenuItem item in Menu)
        {
            SqlCommand updateCommand = new SqlCommand("UPDATE MenuItems SET Price = @Price WHERE Name = @Name", connection);
            updateCommand.Parameters.AddWithValue("@Name", item.Name);
            updateCommand.Parameters.AddWithValue("@Price", item.Price);
            int rowsUpdated = updateCommand.ExecuteNonQuery();
             if (rowsUpdated == 0)
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO MenuItems (Name, Price) VALUES (@Name, @Price)", connection);
                insertCommand.Parameters.AddWithValue("@Name", item.Name);
                insertCommand.Parameters.AddWithValue("@Price", item.Price);
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}
     // Method to load the restaurant's menu from a database
    public void LoadMenuFromDatabase(string connectionString)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
         // Select all menu items from the database
        SqlCommand selectCommand = new SqlCommand("SELECT Name, Price FROM MenuItems", connection);
        SqlDataReader reader = selectCommand.ExecuteReader();
         // Create a new list to store the new menu items
        List<MenuItem> newMenu = new List<MenuItem>();
         // Add each menu item from the database to the new menu list
        while (reader.Read())
        {
            string name = reader.GetString(0);
            decimal price = reader.GetDecimal(1);
            MenuItem item = new MenuItem(name, price);
            newMenu.Add(item);
        }
         // Check if the menu items have changed
        if (!Menu.SequenceEqual(newMenu))
        {
            // Clear the existing menu items from the restaurant's menu
            Menu.Clear();
             // Add the new menu items to the restaurant's menu
            Menu.AddRange(newMenu);
        }
         reader.Close();
    }
}
     // Method to display the restaurant's menu
    public void DisplayMenu()
{
    StringBuilder menuBuilder = new StringBuilder();
     menuBuilder.AppendLine($"Menu for {Name}:");
    foreach (MenuItem item in Menu)
    {
        menuBuilder.AppendLine($"{item.Name} - ${item.Price}");
    }
     Console.WriteLine(menuBuilder.ToString());
}
}
 // Class representing a menu item
class MenuItem
{
    // Properties of the menu item
    public string Name { get; set; }
    public decimal Price { get; set; }
     // Constructor for the menu item
    public MenuItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}
 // Main program class
class Program
{
    static void Main()
    {
        // Create a new restaurant and add some menu items
        Restaurant restaurant = new Restaurant("My Restaurant");
        restaurant.AddMenuItem("Burger", 9.99m);
        restaurant.AddMenuItem("Pizza", 12.99m);
        restaurant.AddMenuItem("Salad", 7.99m);
         // Database connection string
        string connectionString = "YourConnectionString";
         // Check if the user is an admin
        bool isAdmin = Login();
         // If the user is an admin, allow them to interact with the restaurant's menu
        if (isAdmin)
        {
            Console.WriteLine("Logged in as admin.");
            Console.WriteLine("1. Save menu to database");
            Console.WriteLine("2. Load menu from database");
            Console.WriteLine("3. Display menu");
            Console.WriteLine("4. Exit");
             bool exit = false;
             while (!exit)
            {
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                 switch (choice)
                {
                    case "1":
                        restaurant.SaveMenuToDatabase(connectionString);
                        Console.WriteLine("Menu saved to database.");
                        break;
                    case "2":
                        restaurant.LoadMenuFromDatabase(connectionString);
                        Console.WriteLine("Menu loaded from database.");
                        break;
                    case "3":
                        restaurant.DisplayMenu();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Access denied. You do not have admin privileges.");
        }
         Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
     // Method to check if the user is an admin
    static bool Login()
{
    Console.Write("Enter username: ");
    
    string username = Console.ReadLine();
     
     Console.Write("Enter password: ");
    string password = ReadPassword();
     // For simplicity, let's assume "admin" as the valid username and a hashed version of password
    string hashedPassword = ComputeSha256Hash(password); // You need to implement this method to compute SHA256 hash
     // Ideally, the hashed password should be stored securely and retrieved for comparison
    string storedHashedPassword = "admin";
     if (username == "admin" && hashedPassword == storedHashedPassword)
    {
        return true;
    }
     return false;
}
 // Method to securely read password
static string ReadPassword()
{
    StringBuilder password = new StringBuilder();
    while (true)
    {
        ConsoleKeyInfo i = Console.ReadKey(true);
        if (i.Key == ConsoleKey.Enter)
        {
            break;
        }
        else if (i.Key == ConsoleKey.Backspace)
        {
            if (password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b");
            }
        }
        else
        {
            password.Append(i.KeyChar);
            Console.Write("*");
        }
    }
    return password.ToString();
}
/// <summary>
/// The function computes the SHA256 hash of a given input string and returns it as a hexadecimal
/// string.
/// </summary>
/// <param name="input">The input parameter is a string that represents the data for which you want to
/// compute the SHA256 hash.</param>
/// <returns>
/// The method returns a string representation of the SHA256 hash value of the input string.
/// </returns>
public static string ComputeSha256Hash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}