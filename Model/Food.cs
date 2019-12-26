using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Domain
{
    public class Food
    {
        [Index]
        [AutoIncrement]
        public int FoodId { get; set; }
        [Required]
        public string FoodName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double? NettPrice { get; set; }
        public double? Price { get; set; }
        public double? Weight { get; set; }
        public int? ProductionTime { get; set; }
        public double? PriceSmall { get; set; }
        public double? PriceMedium { get; set; }
        public double? PriceLarge { get; set; }
    }
}
