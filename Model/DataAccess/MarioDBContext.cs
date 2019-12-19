using MarioPizzaOriginal.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public class MarioDBContext : DbContext
    {
        public MarioDBContext()
        {
            /*if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
            Database.SetInitializer<MarioDBContext>(null);
            */
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            if (!optionbuilder.IsConfigured)
            {
                optionbuilder.UseSqlite(@"Data Source =.\MarioPizza.db");
            }
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Food> Food { get; set; }

    }
}