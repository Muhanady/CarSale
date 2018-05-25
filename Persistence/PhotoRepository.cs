using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSale.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Persistence {
    public class PhotoRepository : IPhotoRepository {
        private readonly CarDbContext context;
        public PhotoRepository (CarDbContext context) {
            this.context = context;

        }
        public async Task<IEnumerable<Photo>> GetPhotos (int vehicleId) {
            return await context.Photos.Where (p => p.VehicleId == vehicleId).ToListAsync ();
        }
    }
}