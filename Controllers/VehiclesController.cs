using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarSale.Controllers.Resources;
using CarSale.Core;
using CarSale.Core.Models;
using CarSale.Persistence;
using CarSaleCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Controllers {
    [Route ("/api/vehicle")]
    public class VehiclesController : Controller {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController (IVehicleRepository repository, IUnitOfWork unitOfWork, IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles (VehicleQueryResource filter) {
            var _filter = mapper.Map<VehicleQueryResource, VehicleQuery> (filter);
            var queryResult = await repository.GetVehicles (_filter);
            return mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>> (queryResult);

        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetVehicle (int id) {
            var vehicle = await repository.GetVehicle (id, true);

            if (vehicle == null)
                return NotFound ();

            var result = mapper.Map<Vehicle, VehicleResource> (vehicle);
            return Ok (result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateVehicle ([FromBody] SaveVehicleResource vehicleResource) {

            if (!ModelState.IsValid)
                return BadRequest (ModelState);

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle> (vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            repository.Add (vehicle);
            await unitOfWork.CompleteAsync ();
            vehicle = await repository.GetVehicle (vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource> (vehicle);

            return Ok (result);

        }

        [Authorize]
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateVehicle (int id, [FromBody] SaveVehicleResource vehicleResource) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);

            var vehicle = await repository.GetVehicle (id);
            if (vehicle == null)
                return BadRequest ();

            mapper.Map<SaveVehicleResource, Vehicle> (vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync ();

            vehicle = await repository.GetVehicle (vehicle.Id);
            var result = mapper.Map<Vehicle, SaveVehicleResource> (vehicle);

            return Ok (result);
        }

        [Authorize]
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteVehicle (int id) {
            var vehicle = await repository.GetVehicle (id, false);
            if (vehicle == null)
                return NotFound ();

            repository.Remove (vehicle);
            await unitOfWork.CompleteAsync ();

            return Ok ();
        }

    }
}