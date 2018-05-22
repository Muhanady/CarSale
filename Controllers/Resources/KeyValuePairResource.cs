using System.ComponentModel.DataAnnotations;

namespace CarSale.Controllers.Resources {
    public class KeyValuePairResource {
        public int Id { get; set; }

        [Required]
        [StringLength (155)]
        public string Name { get; set; }
    }
}