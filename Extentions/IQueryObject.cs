namespace CarSale.Extentions {
    public interface IQueryObject {
        string SortingBy { get; set; }
        bool IsSortingAscending { get; set; }

        int Page { get; set; }
        int PageSize { get; set; }
    }
}