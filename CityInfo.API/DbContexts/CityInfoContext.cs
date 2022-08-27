using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City(name: "Odessa")
                {
                    Id = 1,
                    Description = "Odessa City"
                },
                new City(name: "Kharkiv")
                {
                    Id = 2,
                    Description = "Khrkiv City"
                },
                new City(name: "Kherson")
                {
                    Id = 3,
                    Description = "I love watermelons"
                }
           );
            modelBuilder.Entity<PointOfInterest>().HasData(
                        new PointOfInterest(name: "Opera theater") { Id = 1, Description = "Visit at night", CityId = 1 },
                        new PointOfInterest(name: "Shevchenko Park") { Id = 2, Name = "Shevchenko Park", Description = "Good to ride a bake", CityId = 1 },
                        new PointOfInterest(name: "Poskot") { Id = 3, Name = "Poskot", Description = "Criminal district" , CityId = 1 },
                        new PointOfInterest(name: "Saltovka") { Id = 4, Description = "Something to buy", CityId = 2 },
                        new PointOfInterest(name: "University") { Id = 5, Description = "Nice place to stutdy" , CityId = 2 },
                        new PointOfInterest(name: "Downtown") { Id = 6, Description = "Nice place to raise a meeting" , CityId = 3 }
               );
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
