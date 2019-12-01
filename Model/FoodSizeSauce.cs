using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Domain
{
    public class FoodSizeSauce : Food
    {
        public FoodSize FoodSizeType { get; set; }
        public List<Ingredient> SauceList { get; set; }
    }
}
