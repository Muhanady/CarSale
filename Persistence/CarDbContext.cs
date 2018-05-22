using Microsoft.EntityFrameworkCore;
using CarSaleCore.Models;
namespace CarSale.Persistence {
    public class CarDbContext : DbContext {

        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
         public DbSet<Vehicle> Vehicles { get; set; }

        public CarDbContext (DbContextOptions<CarDbContext> options) : base (options) {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<VehicleFeature> ().HasKey (VehicleFeature => new {
                VehicleFeature.VehicleId, VehicleFeature.FeatureId
            });
        }

    }
}