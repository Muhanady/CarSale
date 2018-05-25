using System.Collections.Generic;
using System.Threading.Tasks;
using CarSale.Core.Models;

namespace CarSale.Persistence {
    public interface IPhotoRepository {
        Task<IEnumerable<Photo>> GetPhotos (int vehicleId);
    }
}