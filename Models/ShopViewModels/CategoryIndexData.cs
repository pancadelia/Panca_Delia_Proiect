using ShopModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panca_Delia_Proiect.Models.ShopViewModels
{
    public class CategoryIndexData
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Order> Orders { get; set; }

    }
}
