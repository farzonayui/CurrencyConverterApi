using Microsoft.EntityFrameworkCore;
using CurrencyConverterApi.Models;

namespace CurrencyConverterApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Currency> Currency { get; set; }
        public DbSet<ConvertionHistory> ConvertionHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Currency>()
                .Property(c => c.CurrencyCode)
                .HasConversion<string>();

            modelBuilder.Entity<ConvertionHistory>()
                .Property(c => c.FromCurrency)
                .HasConversion<string>();

            modelBuilder.Entity<ConvertionHistory>()
                .Property(c => c.ToCurrency)
                .HasConversion<string>();
        }
    }
}