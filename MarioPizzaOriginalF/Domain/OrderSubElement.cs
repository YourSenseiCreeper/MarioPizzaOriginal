using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class OrderSubElement
    {
        [Index]
        [AutoIncrement]
        public int SubOrderElementId { get; set; }
        public int OrderElementId { get; set; }
        public int FoodId { get; set; }
        [Reference]
        public Food Food { get; set; }
        public double Amount { get; set; }
    }
}
