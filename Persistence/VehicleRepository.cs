using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarSale.Core.Models;
using CarSale.Extentions;
using CarSaleCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Persistence {
    public class VehicleRepository : IVehicleRepository {
        private readonly CarDbContext context;
        public VehicleRepository (CarDbContext context) {
            this.context = context;

        }
        public void Add (Vehicle vehicle) {
            context.Add (vehicle);
        }

        public async Task<Vehicle> GetVehicle (int id, bool includeRelated) {
            if (!includeRelated)
                return await context.Vehicles.SingleOrDefaultAsync (f => f.Id == id);

            return await context.Vehicles.Include (i => i.Features)
                .ThenInclude (m => m.Feature)
                .Include (i => i.Model)
                .ThenInclude (i => i.Make)
                .SingleOrDefaultAsync (f => f.Id == id);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles (VehicleQuery queryObj) {
            var query = context.Vehicles.Include (i => i.Features)
                .ThenInclude (m => m.Feature)
                .Include (i => i.Model)
                .ThenInclude (i => i.Make).AsQueryable ();

            if (queryObj.MakeId.HasValue)
                query = query.Where (i => i.Model.MakeId == queryObj.MakeId);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>> > {
                    ["make"] = v => v.Model.Make.Name,
                    ["model"] = v => v.Model.Name,
                    ["contactName"] = v => v.ContactName,
                    ["id"] = v => v.Id
                };

            query = query.ApplyOrdering (queryObj, columnsMap);

            var result = new QueryResult<Vehicle> () {
                TotalItems = await query.CountAsync (),
                Items = await query.ToListAsync ()
            };

            query = query.ApplyPaging (queryObj);
            return result;
        }

        public void Remove (Vehicle vehicle) {
            context.Remove (vehicle);
        }
    }
}