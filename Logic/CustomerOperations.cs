using LabWork_Entity_Framework.Data;
using LabWork_Entity_Framework.Models;
using LabWork_Entity_Framework.Models.Internal;
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
        static Customer CreateNewCustomer(Customer customer)
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
        private static void EnterCustomerInfo(Customer customer)
        {
            bool goodInput = false;
            StringBuilder sb = new();
            //Ввод имени
            while (!goodInput)
            {
                Console.Write("*First Name: ");
                sb.Append(Console.ReadLine().Trim());
                sb[0] = char.ToUpper(sb[0]);
                if (sb.Length >= 1 && sb.Length < 100)
                {
                    goodInput = true;
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
                sb.Append(Console.ReadLine().Trim());
                sb[0] = char.ToUpper(sb[0]);
                if (sb.Length >= 1 && sb.Length < 100)
                {
                    goodInput = true;
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
                Console.Write("Enter you e-mail address: ");
                while (!goodInput)
                {
                    sb.Append(Console.ReadLine().Trim());
                    Regex regex = new(@"\w+\@\w+\.[A-z]+");
                    if (regex.IsMatch(sb.ToString()))
                    {
                        goodInput = true;
                        customer.Email = sb.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                    sb.Clear();
                }
            }

            //Ввод города
            goodInput = false;
            while (!goodInput)
            {
                Console.Write("*City: ");
                sb.Append(Console.ReadLine().Trim());
                if (sb.Length >= 2 && sb.Length < 100)
                {
                    goodInput = true;
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
                sb.Append(Console.ReadLine().Trim());
                if (sb.Length >= 5 && sb.Length < 100)
                {
                    goodInput = true;
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
                    if (sb.Length >= 2 && sb.Length < 4)
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
        public static UserInfo RegisterCustomer(Customer customer)
        {
            UserInfo info = new();
            info.CustomerId = customer.Id;
            info.Customer = customer;
            CreatePassword(info);
            return info;
        }
        private static void CreatePassword(UserInfo info)
        {
            bool matchingInput = false;
            while (!matchingInput)
            {
                Console.Write("Enter your password: ");
                string pass = Console.ReadLine().Trim();
                Console.Write("Enter your password again: ");
                if (Console.ReadLine().Trim() == pass)
                {
                    matchingInput = true;
                    info.Password = pass;
                }
                else
                {
                    Console.WriteLine("Passwords do not match. Try again.");
                }
            }
        }
        public static Customer LoginCustomer(ApplicationDbContext context, string email, string password)
        {
            var info = context.Customers.Where(c => c.Email == email).FirstOrDefault();
            if (info is null || info.UserInfo is null || info.UserInfo.Password != password)
            {
                return null;
            }
            else
            {
                return info.UserInfo.Customer;
            }
        }
    }
}
