using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TambayPortal.Data.Models;

namespace TambayPortal.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<SaleTransaction> SaleTransactions { get; set; }
        public DbSet<NetworkUsage> NetworkUsages { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<RateClass> RateClasses { get; set; }
        public DbSet<Log> Logs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tambay_portal.db");
            this.Database.Migrate();
        }
    }
}
