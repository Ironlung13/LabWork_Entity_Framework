using LabWork_Entity_Framework.Data;
using LabWork_Entity_Framework.Models;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LabWork_Entity_Framework.Logic
{
    public static class CustomerOperations
    {
        public static Customer CreateNewCustomer()
        {
            Customer customer = new();
            return CreateNewCustomer(customer);
        }
        private static Customer CreateNewCustomer(Customer customer)
        {
            Console.WriteLine("Welcome, new customer!");
            Console.WriteLine("Please, enter your reference data to our database:");
            Console.WriteLine("NOTE: fields marked with a * are required.");

            EnterCustomerInfo(customer);

            Console.WriteLine("\nHere is your info:");
            Console.WriteLine(customer);
            Console.WriteLine("Is everything correct?");
            Console.WriteLine("If it is, enter Y");
            switch (Console.ReadLine())
            {
                case "Y":
                case "y":

                    return customer;
                default:
                    Console.Clear();
                    return CreateNewCustomer(customer);
            }
        }
        public static string GetOrdersInfo(Customer customer)
        {
            //Add exception handling
            StringBuilder sb = new();
            int orderCount = 1;
            foreach (var order in customer.Orders)
            {
                sb.AppendLine($"ORDER #{orderCount} [Bought from {order.Store.Name}]:");
                foreach (var orderItem in order.OrderItems)
                {
                    sb.AppendLine($"{orderItem.Product.Name} - {orderItem.Quantity}x. Total cost - {orderItem.Price * orderItem.Quantity * (1m - orderItem.Discount):C2}");
                }
                orderCount++;
            }

            return sb.ToString();
        }
        public static Customer FindCustomer(ApplicationDbContext context)
        {
            Customer customer = null;
            bool userFound = false;
            while (!userFound)
            {
                StringBuilder sb = new();
                string email = string.Empty;
                bool goodInput = false;
                while (!goodInput)
                {
                    Console.Write("Enter you e-mail address: ");
                    while (!goodInput)
                    {
                        sb.Append(Console.ReadLine().Trim().ToLower());
                        Regex regex = new(@"\w+\@\w+\.[A-z]+");
                        if (regex.IsMatch(sb.ToString()))
                        {
                            goodInput = true;
                            email = sb.ToString();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Try again.");
                        }
                        sb.Clear();
                    }
                }

                var customers = context.Customers.Where(x => x.Email == email).ToList();
                if (customers is null)
                {
                    Console.WriteLine("User not found!");
                }
                else if (customers.Count() != 1)
                {
                    Console.WriteLine("Multiple users are using this email.");
                }
                else
                {
                    Console.WriteLine("User found! Is this you?");
                    customer = customers.First();
                    userFound = true;
                }
            }
            return customer;
        }
        public static Customer FindCustomerByEmail(ApplicationDbContext context, string email)
        {
            var customers = context.Customers.Where(x => x.Email == email).ToList();
            if (customers is null || customers.Count == 0)
            {
                return null;
            }
            else if (customers.Count() > 1)
            {
                throw new ArgumentException("Multiple users with matching email.");
            }
            else
            {
                return customers.First();
            }
        }
        public static void UpdateCustomerAddress(ApplicationDbContext context, Customer customer)
        {
            Console.WriteLine($"Current address of {customer.FirstName} {customer.LastName}:");
            Console.Write($"City: {customer.City}, ");
            if (!(customer.State is null))
            {
                Console.Write($"State: {customer.State}, ");
            }
            Console.WriteLine($"Address: {customer.Address}");

            bool goodInput = false;
            StringBuilder sb = new();
            while (!goodInput)
            {
                Console.Write($"Enter new City: ");
                sb.Append(Console.ReadLine().Trim().ToLower());
                if (sb.Length >= 2 && sb.Length < 100)
                {
                    goodInput = true;
                    sb[0] = char.ToUpper(sb[0]);
                    customer.City = sb.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            goodInput = false;
            while (!goodInput)
            {
                Console.Write("Enter new address: ");
                sb.Append(Console.ReadLine().Trim().ToLower());
                if (sb.Length >= 5 && sb.Length < 100)
                {
                    goodInput = true;
                    sb[0] = char.ToUpper(sb[0]);
                    customer.Address = sb.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            //Ввод штата
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("Enter new State(not neccessary): ");
                sb.Append(Console.ReadLine().Trim().ToUpper());
                if (sb.Length > 0)
                {
                    if (sb.Length == 2)
                    {
                        goodInput = true;
                        customer.State = sb.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                }
                else
                {
                    customer.State = null;
                    goodInput = true;
                }
                sb.Clear();
            }

            Console.WriteLine("Saving new info.");
            context.SaveChanges();
            Console.WriteLine("Customer info after change:");
            Console.WriteLine(customer);
            Console.WriteLine("\nEnter anything to quit.");
            Console.ReadLine();
        }
        private static void EnterCustomerInfo(Customer customer)
        {
            bool goodInput = false;
            StringBuilder sb = new();
            //Ввод имени
            while (!goodInput)
            {
                Console.Write("*First Name: ");
                sb.Append(Console.ReadLine().Trim().ToLower());
                if (sb.Length >= 1 && sb.Length < 100)
                {
                    goodInput = true;
                    sb[0] = char.ToUpper(sb[0]);
                    customer.FirstName = sb.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            //Ввод фамилии
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("*Last Name: ");
                sb.Append(Console.ReadLine().Trim().ToLower());
                if (sb.Length >= 1 && sb.Length < 100)
                {
                    goodInput = true;
                    sb[0] = char.ToUpper(sb[0]);
                    customer.LastName = sb.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            //Ввод номера телефона
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("Phone Number [(XXX)XXX-XXXX format]: ");
                sb.Append(Console.ReadLine().Trim());
                if (sb.Length > 0)
                {
                    Regex regex = new(@"\([0-9]{3}\)[0-9]{3}-[0-9]{4}");
                    if (regex.IsMatch(sb.ToString()))
                    {
                        goodInput = true;
                        customer.PhoneNumber = sb.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                }
                else
                {
                    customer.PhoneNumber = null;
                    goodInput = true;
                }
                sb.Clear();
            }

            //Ввод email
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("*E-mail: ");

                sb.Append(Console.ReadLine().Trim().ToLower());
                Regex regex = new(@"\w+\@\w+\.[A-z]+");
                if (regex.IsMatch(sb.ToString()))
                {
                    if (CheckRepeatingEmail(new ApplicationDbContext(), sb.ToString()))
                    {
                        Console.WriteLine("This email address is already registered in the database.");
                    }
                    else
                    {
                        goodInput = true;
                        customer.Email = sb.ToString();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            //Ввод города
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("*City: ");
                sb.Append(Console.ReadLine().Trim().ToLower());
                if (sb.Length >= 2 && sb.Length < 100)
                {
                    goodInput = true;
                    sb[0] = char.ToUpper(sb[0]);
                    customer.City = sb.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            //Ввод адреса
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("*Address: ");
                sb.Append(Console.ReadLine().Trim().ToLower());
                if (sb.Length >= 5 && sb.Length < 100)
                {
                    goodInput = true;
                    sb[0] = char.ToUpper(sb[0]);
                    customer.Address = sb.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                sb.Clear();
            }

            //Ввод штата
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("State: ");
                sb.Append(Console.ReadLine().Trim().ToUpper());
                if (sb.Length > 0)
                {
                    if (sb.Length == 2)
                    {
                        goodInput = true;
                        customer.State = sb.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                }
                else
                {
                    customer.State = null;
                    goodInput = true;
                }
                sb.Clear();
            }

            //Ввод индекса
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("*Zip Code: ");
                if (int.TryParse(Console.ReadLine().Trim(), out int zip))
                {
                    goodInput = true;
                    customer.ZipCode = zip;
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }
        
        private static bool CheckRepeatingEmail(ApplicationDbContext context, string email)
        {
            var sameEmail = context.Customers.Where(x => x.Email == email).ToList();
            if (sameEmail.Count() != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
