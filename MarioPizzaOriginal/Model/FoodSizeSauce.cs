using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Model
{
    public class FoodSizeSauce : Food
    {
        public FoodSize FoodSizeType { get; set; }
        public List<Ingredient> SauceList { get; set; }
    }
}
