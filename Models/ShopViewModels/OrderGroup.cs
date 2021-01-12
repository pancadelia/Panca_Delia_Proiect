using System;
using System.ComponentModel.DataAnnotations;
namespace Panca_Delia_Proiect.Models.ShopViewModels
{
    public class OrderGroup
    {
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        public int ProductCount { get; set; }

    }
}