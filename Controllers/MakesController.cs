using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarSale.Controllers.Resources;
using CarSaleCore.Models;
using CarSale.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Controllers {
    public class MakesController : Controller {
        private readonly CarDbContext context;
        private readonly IMapper mapper;

        public MakesController (CarDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet ("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes () {
            var makes = await context.Makes.Include (ma => ma.Models).ToListAsync ();
            return mapper.Map<List<Make>, List<MakeResource>> (makes);
        }

    }
}