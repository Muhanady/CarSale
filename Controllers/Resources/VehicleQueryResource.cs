using CarSale.Extentions;

namespace CarSale.Controllers.Resources {
    public class VehicleQueryResource : IQueryObject {
        public int? MakeId { get; set; }

        public string SortingBy { get; set; }
        public bool IsSortingAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}