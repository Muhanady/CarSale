using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CarSale.Controllers.Resources;

namespace CarSaleCore.Models {
    public class Make : KeyValuePairResource {
        public ICollection<Model> Models { get; set; }
        public Make () {
            Models = new Collection<Model> ();
        }
    }
}