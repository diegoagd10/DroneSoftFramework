using System;
using System.Data.Entity;
using System.Configuration;

namespace DroneSoftFramework.Database.Relational.Factory
{
    internal static class DbContextFactory
    {
        public static DbContext CreateDbContext<TDbContext>() where TDbContext : DbContext, new()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("DatabaseConnectionString");

            TDbContext context = new TDbContext();

            context.Database.Connection.ConnectionString = connectionString;
            return context;
        }
    }
}
