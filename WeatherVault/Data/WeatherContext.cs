using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using WeatherVault.Models;
using Microsoft.EntityFrameworkCore;

namespace WeatherVault.Models
{
    public class WeatherContext : DbContext
    {
        public DbSet<CachedWeather> CachedWeather { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=weather.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальный индекс на город (чтобы не было дублей в кеше)
            modelBuilder.Entity<CachedWeather>()
                .HasIndex(c => c.City)
                .IsUnique();
        }
    }
}
