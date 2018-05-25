using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarSale.Controllers.Resources;
using CarSale.Core;
using CarSale.Core.Models;
using CarSale.Persistence;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CarSale.Controllers {
    [Route ("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller {
        private readonly IVehicleRepository repository;
        public IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;

        private readonly IHostingEnvironment host;
        private readonly IOptionsSnapshot<PhotoSettings> options;
        private readonly PhotoSettings photoSettings;
        public PhotosController (IHostingEnvironment host, IVehicleRepository repository, IPhotoRepository photoRepository, IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options) {
            this.photoRepository = photoRepository;
            this.photoSettings = options.Value;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.host = host;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos (int vehicleId) {
            var photos = await photoRepository.GetPhotos (vehicleId);
            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>> (photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload (int vehicleId, IFormFile file) {
            var vehicle = await repository.GetVehicle (vehicleId, includeRelated : false);
            if (vehicle == null) return NotFound ();

            if (file == null) return BadRequest ("Null File");
            if (file.Length == 0) return BadRequest ("Empty File");
            if (file.Length > photoSettings.MaxBytes) return BadRequest ("Maximum File size exceeded");
            if (!photoSettings.IsSupported (file.FileName)) return BadRequest ("File type not supported");

            var uploadFolderPath = Path.Combine (host.WebRootPath, "uploads");
            if (!Directory.Exists (uploadFolderPath))
                Directory.CreateDirectory (uploadFolderPath);

            var fileName = Guid.NewGuid ().ToString () + Path.GetExtension (file.FileName);
            var filePath = Path.Combine (uploadFolderPath, fileName);

            using (var stream = new FileStream (filePath, FileMode.Create)) {
                await file.CopyToAsync (stream);
            }
            var photo = new Photo () {
                FileName = fileName
            };
            vehicle.Photos.Add (photo);
            await unitOfWork.CompleteAsync ();
            return Ok (mapper.Map<Photo, PhotoResource> (photo));
        }
    }
}