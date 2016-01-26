﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        public static void Start(List<User> users, List<Product> products)
        {
            // Show app info
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
            Console.WriteLine();

            // Login
            bool loggedIn = false;
            Console.WriteLine("Enter Username:");
            string name = Console.ReadLine();

            // Validate Username
            bool validUser = false;
            if (name.Length > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    User user = users[i];
                    if (user.Username == name)
                    {
                        validUser = true;
                    }
                }

                if (validUser)
                {
                    Console.WriteLine("Enter Password:");
                    string password = Console.ReadLine();

                    // Validate Password
                    bool validPassword = false;
                    for (int i = 0; i < users.Count; i++)
                    {
                        User user = users[i];
                        if (user.Username == name && user.Password == password)
                        {
                            validPassword = true;
                        }
                    }

                    if (validPassword == true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Login successful!");
                        loggedIn = true;

                        // Write Greeting
                        Console.WriteLine();
                        Console.WriteLine("Welcome " + name + "!");

                        // Show remaining balance
                        double balance = 0;
                        for (int i = 0; i < users.Count; i++)
                        {
                            User user = users[i];
                            if (user.Username == name && user.Password == password)
                            {
                                balance = user.Balance;
                                Console.WriteLine();
                                Console.WriteLine("Your balance is " + user.Balance.ToString("C"));
                            }
                        }

                        // Show product list
                        while (true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("What would you like to buy?");
                            for (int i = 0; i < products.Count; i++)
                            {
                                Product product = products[i];
                                Console.WriteLine(i + 1 + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
                            }
                            Console.WriteLine(products.Count + 1 + ": Exit");

                            Console.WriteLine("Enter a number:");
                            string answer = Console.ReadLine();
                            int number = Convert.ToInt32(answer);
                            number = number - 1;

                            if (number == products.Count)
                            {
                                // User selected Exit

                                // Write out new balance
                                foreach (var user in users)
                                {
                                    if (user.Username == name && user.Password == password)
                                    {
                                        user.Balance = balance;
                                    }
                                }
                                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                                File.WriteAllText(@"Data\Users.json", json);

                                // Write out new quantities
                                string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
                                File.WriteAllText(@"Data\Products.json", json2);


                                // Prevent console from closing
                                Console.WriteLine();
                                Console.WriteLine("Press Enter key to exit");
                                Console.ReadLine();
                                return;
                            }
                            else
                            {
                                Console.WriteLine("You want to buy: " + products[number].Name);
                                Console.WriteLine("Your balance is " + balance.ToString("C"));

                                // Prompt for quantity
                                Console.WriteLine("Enter amount to purchase:");
                                answer = Console.ReadLine();
                                int quantity = Convert.ToInt32(answer);

                                // Check if user has enough balance
                                if (balance - products[number].Price * quantity < 0)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("You do not have enough money to buy that.");
                                    Console.ResetColor();
                                    continue;
                                }

                                // Check if product has any remaining quantity
                                if (products[number].Quantity <= quantity)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("Sorry, " + products[number].Name + " is out of stock");
                                    Console.ResetColor();
                                    continue;
                                }

                                if (quantity > 0)
                                {
                                    balance = balance - products[number].Price * quantity;
                                    products[number].Quantity = products[number].Quantity - quantity;

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("You bought " + quantity + " " + products[number].Name);
                                    Console.WriteLine("Your new balance is " + balance.ToString("C"));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine();
                                    Console.WriteLine("Purchase cancelled");
                                    Console.ResetColor();
                                }
                            }
                        }
                    }
                    else
                    {
                        // TODO: fix so we don't have to restart app on failed login
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("You entered an invalid password. Please restart TUSC to try again.");
                        Console.ResetColor();

                        // Prevent console from closing
                        Console.WriteLine();
                        Console.WriteLine("Press Enter key to exit");
                        Console.ReadLine();
                        return;
                    }
                }
                else
                {
                    // TODO: fix so we don't have to restart app on failed login
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("You entered an unknown user. Please restart TUSC to try again.");
                    Console.ResetColor();

                    // Prevent console from closing
                    Console.WriteLine();
                    Console.WriteLine("Press Enter key to exit");
                    Console.ReadLine();
                    return;
                }
            }

            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }
    }
}