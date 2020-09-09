using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class OrderSubElement
    {
        [AutoIncrement]
        public int OrderSubElementId { get; set; }
        public int OrderElementId { get; set; }
        public int FoodId { get; set; }
        [Reference]
        public Food Food { get; set; }
        public double Amount { get; set; }
    }
}
