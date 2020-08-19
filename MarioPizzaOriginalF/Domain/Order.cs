using System;
using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class Order
    {
        [Index]
        [AutoIncrement]
        public int OrderId { get; set; }
        [Required]
        public string ClientPhoneNumber { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public OrderPriority Priority { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        public Payment Payment { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public DateTime OrderTime { get; set; }
    }
}