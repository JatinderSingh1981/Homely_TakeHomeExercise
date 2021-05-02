using Entities.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Common.API;
using System;

namespace DBContext.API
{
    public class DataContext : DbContext
    {
        public DbSet<PropertyListing> PropertyListing { get; set; }
       

        private readonly IOptions<AppSettings> _settings;

        public DataContext(DbContextOptions<DataContext> contextOptions,
            IOptions<AppSettings> settings) : base(contextOptions)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Log the queries to console
            optionsBuilder.LogTo(Console.WriteLine);

            optionsBuilder
               .EnableSensitiveDataLogging()
               .UseSqlServer(
               _settings.Value.ConnectionStrings.ApiDatabase, 
               options => options.EnableRetryOnFailure(3));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed data here if required

        }
    }
}
