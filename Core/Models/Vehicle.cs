using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CarSale.Core.Models;

namespace CarSaleCore.Models {
    public class Vehicle {
        public int Id { get; set; }

        [Required]
        [StringLength (255)]
        public string ContactName { get; set; }

        [Required]
        [StringLength (255)]
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool IsRegistered { get; set; }
        public DateTime LastUpdate { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public ICollection<VehicleFeature> Features { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public Vehicle () {
            Features = new Collection<VehicleFeature> ();
            Photos = new Collection<Photo> ();
        }

    }
}