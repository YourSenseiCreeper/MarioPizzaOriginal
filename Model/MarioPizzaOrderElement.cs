﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MarioPizzaOrderElement
    {
        public int OrderElementId { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public double Amount { get; set; }
    }
}