using MarioPizzaOriginal.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Model
{
    public class Pizza : FoodSizeSauce
    {
        bool Discounted { get; set; }
        double DiscountPercent { get; set; }
    }
}
