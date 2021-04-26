using LabWork_Entity_Framework.Data;
using Microsoft.EntityFrameworkCore;
using LabWork_Entity_Framework.Logic;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LabWork_Entity_Framework.Models;

namespace LabWork_Entity_Framework
{
    class Program
    {
        static void Main()
        {
            var context = new ApplicationDbContext();
            Console.WriteLine("***WORKING WITH DATABASE***\n");
            Console.WriteLine("1: Register new customer [Create].");
            Console.WriteLine("2: Generate yearly report [Read].");
            Console.WriteLine("3: Update a customer's address [Update].");
            Console.WriteLine("4: Delete a customer entry [Delete].");
            Console.WriteLine("\nDELETE: Delete and recreate database");
            Console.WriteLine();
        Choice:
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Create(context);
                    break;
                case "2":
                    Console.Clear();
                    Read(context);
                    break;
                case "3":
                    Console.Clear();
                    Update(context);
                    break;
                case "4":
                    Console.Clear();
                    Delete(context);
                    break;
                case "DELETE":
                    Console.Clear();
                    DeleteOldDB(context);
                    CreateAndPopulateDB(context);
                    break;
                default:
                    goto Choice;
            }
            Console.Clear();
            Main();
        }
        static void Create(ApplicationDbContext context)
        {
            Console.WriteLine("Hello, dearest new customer!");
            var newCustomer = CustomerOperations.CreateNewCustomer();
            try
            {
                context.Customers.Add(newCustomer);
                context.SaveChanges();
                Console.WriteLine($"New customer {newCustomer.FirstName} {newCustomer.LastName} successfully added to database.");
            }
            catch
            {
                Console.WriteLine("Something went wrong. Could not add new customer entry.");
            }
        }
        static void Read(ApplicationDbContext context)
        {
            Console.WriteLine("Good day, Manager!");
            Console.WriteLine("Here's your report on last year (2020):");
            Console.WriteLine(File.ReadAllText(GenerateYearlyReport(context, 2020)));
            Console.WriteLine("When you are done reading, press any key to quit.");
            Console.ReadLine();
        }
        static void Update(ApplicationDbContext context)
        {
            Console.WriteLine("Let's say, a customer has moved to a different city.");
            Console.WriteLine("We need to update his database entry.");

            Customer customer = null;
            bool userFound = false;
            while (!userFound)
            {
                StringBuilder sb = new();
                string email = string.Empty;
                bool goodInput = false;
                while (!goodInput)
                {
                    Console.Write("Enter e-mail address: ");

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

                customer = CustomerOperations.FindCustomerByEmail(context, email);
                if (customer is null)
                {
                    Console.WriteLine("User not found!");
                }
                else
                {
                    Console.WriteLine("User found!");
                    userFound = true;
                }
            }

            CustomerOperations.UpdateCustomerAddress(context, customer);
            Console.WriteLine("Updated customer info:");
            Console.WriteLine(customer);
        }
        static void Delete(ApplicationDbContext context)
        {
            Customer customer = null;
            string email = string.Empty;
            StringBuilder sb = new();
            bool goodInput = false;
            bool userFound = false;

            while (!userFound)
            {
                while (!goodInput)
                {
                    Console.Write("Enter customer's to delete email address: ");

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

                customer = CustomerOperations.FindCustomerByEmail(context, email);
                if(!(customer is null))
                {
                    userFound = true;
                }
                else
                {
                    Console.WriteLine("User not found!");
                    goodInput = false;
                }
            }
            Console.WriteLine("Customer Info:");
            Console.WriteLine(customer);

            try
            {
                Console.WriteLine($"Deleting {customer.FirstName} {customer.LastName}.");
                context.Customers.Remove(customer);
                context.SaveChanges();
                Console.WriteLine("Entry deleted successfully");
            }
            catch
            {
                Console.WriteLine("Something went wrong. Aborting.");
            }

            Console.WriteLine("\nEnter anything to quit.");
            Console.ReadLine();
        }
        static void DeleteOldDB(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("DATABASE DELETED.");
        }
        static void CreateAndPopulateDB(ApplicationDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                context.Database.ExecuteSqlRaw(File.ReadAllText(@"SQL Scripts\Load Data.sql"));
                context.SaveChanges();
                Console.WriteLine("DATABASE POPULATED SUCCESFULLY.");
            }
            catch (IOException)
            {
                Console.WriteLine("Unable to read SQL file.");
            }
            catch
            {
                Console.WriteLine("Database already populated. Aborting.");
            }
        }
        static string GenerateYearlyReport(ApplicationDbContext context, int year)
        {
            //Определим культуру, чтобы вывод валюты был в $
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            //Определим название файла, в который будет сохранен отчёт
            int i;
            if (!Directory.Exists("Reports"))
            {
                Directory.CreateDirectory("Reports");
            }
            for (i = 1; File.Exists($"Reports\\Report_{year}_{i}.txt"); i++);
            string filePath = $"Reports\\Report_{year}_{i}.txt";

            //Начинаем запись информации в файл
            using StreamWriter sr = File.CreateText(filePath);
            //Все заказы, сделанные в указанном году
            var orders = (from order in context.Orders 
                             where order.OrderDate.Year == year select order).ToList();
            //Из них, выполненные заказы
            var fulfilledOrders = (from order in orders 
                                    where order.OrderStatus == Enums.OrderStatusEnum.Completed 
                                    select order).ToList();
            //Заказы, от которых отказались
            var rejectedOrders = (from order in orders 
                                    where order.OrderStatus == Enums.OrderStatusEnum.Rejected 
                                    select order).ToList();
            //Остаточные заказы с предыдущего года
            var ordersLastYear = from order in context.Orders 
                                     where order.OrderDate.Year == year - 1 && order.OrderStatus != Enums.OrderStatusEnum.Rejected 
                                     select order;

            ///Начинаем запись в файл
            //Расчет заказов
            sr.WriteLine("***ORDERS***");
            sr.WriteLine($"Orders leftover from {year - 1}: {ordersLastYear.Count()}");
            sr.WriteLine($"Total orders from {year}: {orders.Count}");
            sr.WriteLine($"Orders fulfilled: {fulfilledOrders.Count}");
            sr.WriteLine($"Orders rejected: {rejectedOrders.Count}");
            sr.WriteLine($"Orders still in processing: {orders.Except(fulfilledOrders).Except(rejectedOrders).Count()}");

            //Расчет выручки
            sr.WriteLine("\n***REVENUE***");
            sr.WriteLine($"Total items sold: {fulfilledOrders.SelectMany(o=>o.OrderItems).Sum(c => c.Quantity)}");
            sr.WriteLine($"Total revenue: {fulfilledOrders.Sum(p=>p.TotalPrice):C2}");

            //Лучшие потребители
            var customers = context.Customers.ToList();
            sr.WriteLine("\n***BEST CUSTOMERS***");
            //Топ 3 потребителей
            //1. По потраченным средствам
            sr.WriteLine($"Top 3 spenders:");
            var biggestSpenders = customers.OrderByDescending(x => x.Orders.Where(x => x.OrderDate.Year == year).Where(x=>x.OrderStatus != Enums.OrderStatusEnum.Rejected).Select(x => x.TotalPrice).Sum()).Take(3).ToList();
            foreach (var customer in biggestSpenders)
            {
                sr.WriteLine($"\t{customer.FirstName} {customer.LastName} - {customer.Orders.Where(x => x.OrderDate.Year == year).Where(x => x.OrderStatus != Enums.OrderStatusEnum.Rejected).Sum(x => x.TotalPrice):C2}");
            }
            //2. По количеству купленных предметов
            var biggestHoarders = customers.OrderByDescending(x => x.Orders.Where(x=>x.OrderDate.Year == year).Where(x => x.OrderStatus != Enums.OrderStatusEnum.Rejected).SelectMany(x => x.OrderItems).Sum(x => x.Quantity)).Take(3).ToList();
            sr.WriteLine($"Top 3 hoarders:");
            foreach(var customer in biggestHoarders)
            {
                sr.WriteLine($"\t{customer.FirstName} {customer.LastName} - {customer.Orders.Where(x => x.OrderDate.Year == year).Where(x => x.OrderStatus != Enums.OrderStatusEnum.Rejected).SelectMany(x=>x.OrderItems).Sum(x=>x.Quantity)} items");
            }

            //Самые прибыльные магазины
            //Топ 3 
            sr.WriteLine("\n***STORES***");
            sr.WriteLine("All stores, sorted by revenue:");
            
            var topStores = context.Stores.ToList().OrderByDescending(s => s.Orders.Intersect(fulfilledOrders).Sum(o => o.TotalPrice)).ToList();
            foreach(var store in topStores)
            {
                sr.WriteLine($"\t{store.Name} - {store.Orders.Intersect(fulfilledOrders).Sum(s => s.TotalPrice):C2}");
            }

            //Возвращаем путь к готовому файлу отчёта
            return filePath;
        }
    }
}
