using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AEFWeb.Data.Context
{
    public class AEFContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<EventLog> EventLog { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<PostTag> PostTag { get; set; }
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }

        // public AEFContext(IHostingEnvironment env) : base()
        // {
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Get all classes that implements IMapping
            var typesToMapping = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                                                                && typeof(IConfiguring).IsAssignableFrom(x)).ToList();

            //Set config
            foreach (var mapping in typesToMapping)
            {
                dynamic mappingClass = Activator.CreateInstance(mapping);
                modelBuilder.ApplyConfiguration(mappingClass);
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (String.IsNullOrEmpty(envName)) envName = "Development";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
#if DEBUG
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
#else
                .AddJsonFile($"appsettings.Production.json", optional: true, reloadOnChange: true)
#endif
                .Build();

            

            // define the database to use
            #if DEBUG
            var connectionString = config.GetConnectionString("AEFConnection");
            //replace env. variables
            var r = new System.Text.RegularExpressions.Regex("\\${.*}\\;");
            var matched = r.Match(connectionString);
            while (matched.Success)
            {
                var varName = matched.Value.Substring(2, (matched.Value.Length - 4));
                var varValue = Environment.GetEnvironmentVariable(varName);
                connectionString = connectionString.Replace(
                    matched.Value,
                    String.Concat(varValue, ";")
                );

                matched = matched.NextMatch();
            }
            optionsBuilder.UseSqlServer(connectionString);
            System.Console.WriteLine("Connection string: {0}", connectionString);
            #else
            optionsBuilder.UseSqlServer(config.GetConnectionString("AEFConnection"));
	    System.Console.WriteLine("Connection string: {0}", config.GetConnectionString("AEFConnection"));
            #endif
        }
    }
}
