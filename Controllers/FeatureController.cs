using System.Collections.Generic;
using System.Threading.Tasks;
using CarSale.Persistence;
using CarSaleCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Controllers {
    public class FeatureController : Controller {
        private readonly CarDbContext context;

        public FeatureController (CarDbContext context) {
            this.context = context;
        }

        [HttpGet ("/api/features")]
        public async Task<IEnumerable<Feature>> GetFeatures () {
            return await context.Features.ToListAsync ();
        }
    }
}