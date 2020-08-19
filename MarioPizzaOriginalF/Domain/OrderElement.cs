using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain
{
    public class OrderElement
    {
        [AutoIncrement]
        public int OrderElementId { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public double Amount { get; set; }
        [Reference]
        public List<OrderSubElement> SubOrderElements { get; set; }
    }
}
