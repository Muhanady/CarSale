using System.Collections.Generic;

namespace CarSale.Core.Models
{
    public class QueryResult<T>
    {
        public int TotalItems { get; set; }    
        public IEnumerable<T> Items { get; set; }
    }
}