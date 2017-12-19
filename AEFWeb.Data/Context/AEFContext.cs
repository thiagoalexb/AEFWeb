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
            // get the configuration from the app settings
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("AEFConnection"));
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
