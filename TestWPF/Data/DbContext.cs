using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestWPF.Data
{
   public class AppDbContext : DbContext
    {
        public DbSet<Models.Товар> Товары => Set<Models.Товар>();
        public DbSet<Models.Поставщик> Поставщики => Set<Models.Поставщик>();
        public DbSet<Models.Category> Категории => Set<Models.Category>();
        public DbSet<Models.Manufacturer> Производители => Set<Models.Manufacturer>();
        public DbSet<Models.Role> Роли => Set<Models.Role>();
        public DbSet<Models.User> Пользователи => Set<Models.User>();
        public DbSet<Models.Unit> ЕдиницыИзмерения => Set<Models.Unit>();
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Database=Qval1_1;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        }
    }
}
