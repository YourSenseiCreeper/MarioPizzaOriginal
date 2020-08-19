using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class OrderSubElement
    {
        [Index]
        [AutoIncrement]
        public int SubOrderElementId { get; set; }
        [Required]
        public int OrderElementId { get; set; }
        [Required]
        public int FoodId { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
