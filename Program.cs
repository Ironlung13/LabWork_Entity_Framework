using LabWork_Entity_Framework.Data;
using LabWork_Entity_Framework.Models;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LabWork_Entity_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***WORKING WITH DATABASE***\n");
            Console.WriteLine("1: Placeholder.");
            Console.WriteLine("2: Placeholder.");
            Console.WriteLine("DELETE: Delete and recreate database.");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Not yet implemented. Stay tuned.");
                    break;
                case "2":
                    Console.WriteLine("Not yet implemented. Stay tuned.");
                    break;
                case "DELETE":
                    DeleteOldDB();
                    CreateAndPopulateDB();
                    break;
            }
        }

        static void DeleteOldDB()
        {
            var context = new ApplicationDbContext();
            context.Database.EnsureDeleted();
            Console.WriteLine("DATABASE DELETED.");
        }

        static void CreateAndPopulateDB()
        {
            var context = new ApplicationDbContext();
            try
            {
                context.Database.ExecuteSqlRaw(@"SQL Scripts\Load Data.sql");
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
    }
}
