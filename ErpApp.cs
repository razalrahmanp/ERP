
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

class Restaurant
{
    public string Name { get; set; }
    public List<MenuItem> Menu { get; set; }

    public Restaurant(string name)
    {
        Name = name;
        Menu = new List<MenuItem>();
    }

    public void AddMenuItem(string name, decimal price)
    {
        MenuItem item = new MenuItem(name, price);
        Menu.Add(item);
    }

    public void SaveMenuToDatabase(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand clearCommand = new SqlCommand("DELETE FROM MenuItems", connection);
            clearCommand.ExecuteNonQuery();

            foreach (MenuItem item in Menu)
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO MenuItems (Name, Price) VALUES (@Name, @Price)", connection);
                insertCommand.Parameters.AddWithValue("@Name", item.Name);
                insertCommand.Parameters.AddWithValue("@Price", item.Price);
                insertCommand.ExecuteNonQuery();
            }
        }
    }

    public void LoadMenuFromDatabase(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand selectCommand = new SqlCommand("SELECT Name, Price FROM MenuItems", connection);
            SqlDataReader reader = selectCommand.ExecuteReader();

            Menu.Clear();

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

    public void DisplayMenu()
    {
        Console.WriteLine("Menu for {0}:", Name);
        foreach (MenuItem item in Menu)
        {
            Console.WriteLine("{0} - ${1}", item.Name, item.Price);
        }
    }
}

class MenuItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public MenuItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

class Program
{
    static void Main()
    {
        Restaurant restaurant = new Restaurant("My Restaurant");

        restaurant.AddMenuItem("Burger", 9.99m);
        restaurant.AddMenuItem("Pizza", 12.99m);
        restaurant.AddMenuItem("Salad", 7.99m);

        string connectionString = "YourConnectionString";

        bool isAdmin = Login();

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

    static bool Login()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Perform authentication logic here (e.g., check against a database)

        // For simplicity, let's assume "admin" as the valid username and password
        if (username == "admin" && password == "admin")
        {
            return true;
        }

        return false;
    }
}