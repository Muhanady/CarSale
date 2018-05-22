using CarSale.Controllers.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSaleCore.Models {
    [Table ("Models")]
    public class Model : KeyValuePairResource {
        public int MakeId { get; set; }
        public Make Make { get; set; }
    }
}