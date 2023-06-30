
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

// Define a class for the restaurant
class Restaurant
{
    // Properties
    public string Name { get; set; }
    public List<MenuItem> Menu { get; set; }

    // Constructor
    public Restaurant(string name)
    {
        Name = name;
        Menu = new List<MenuItem>();
    }

    // Method to add a menu item
    public void AddMenuItem(string name, decimal price)
    {
        MenuItem item = new MenuItem(name, price);
        Menu.Add(item);
    }

    // Method to save menu items to the database
    public void SaveMenuToDatabase(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Clear existing menu items from the database
            SqlCommand clearCommand = new SqlCommand("DELETE FROM MenuItems", connection);
            clearCommand.ExecuteNonQuery();

            // Insert new menu items into the database
            foreach (MenuItem item in Menu)
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO MenuItems (Name, Price) VALUES (@Name, @Price)", connection);
                insertCommand.Parameters.AddWithValue("@Name", item.Name);
                insertCommand.Parameters.AddWithValue("@Price", item.Price);
                insertCommand.ExecuteNonQuery();
            }
        }
    }

    // Method to load menu items from the database
    public void LoadMenuFromDatabase(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Retrieve menu items from the database
            SqlCommand selectCommand = new SqlCommand("SELECT Name, Price FROM MenuItems", connection);
            SqlDataReader reader = selectCommand.ExecuteReader();

            // Clear existing menu items
            Menu.Clear();

            // Add retrieved menu items to the list
            while (reader.Read())
            {
                string name = reader.GetString(0);
                decimal price = reader.GetDecimal(1);
                MenuItem item = new MenuItem(name, price);
                Menu.Add(item);
            }

            reader.Close();
        }
    }

    // Method to display the menu
    public void DisplayMenu()
    {
        Console.WriteLine("Menu for {0}:", Name);
        foreach (MenuItem item in Menu)
        {
            Console.WriteLine("{0} - ${1}", item.Name, item.Price);
        }
    }
}

// Define a class for menu items
class MenuItem
{
    // Properties
    public string Name { get; set; }
    public decimal Price { get; set; }

    // Constructor
    public MenuItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

// Main program
class Program
{
    static void Main()
    {
        // Create a restaurant
        Restaurant restaurant = new Restaurant("My Restaurant");

        // Add menu items
        restaurant.AddMenuItem("Burger", 9.99m);
        restaurant.AddMenuItem("Pizza", 12.99m);
        restaurant.AddMenuItem("Salad", 7.99m);

        // Save menu items to the database
        string connectionString = "YourConnectionString";
        restaurant.SaveMenuToDatabase(connectionString);

        // Clear existing menu items
        restaurant.Menu.Clear();

        // Load menu items from the database
        restaurant.LoadMenuFromDatabase(connectionString);

        // Display the menu
        restaurant.DisplayMenu();

        // Wait for user input to exit
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}