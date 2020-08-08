using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Model
{
    public class OrderElement
    {
        [Index]
        [AutoIncrement]
        public int OrderElementId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int FoodId { get; set; }
        [Required]
        public double Amount { get; set; }
        [Ignore]
        public List<OrderSubElement> SubOrderElements { get; set; }
    }
}
