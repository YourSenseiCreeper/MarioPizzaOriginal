using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class Order
    {
        [AutoIncrement]
        public int OrderId { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        [Reference]
        public OrderPriority Priority { get; set; }
        [Reference]
        public OrderStatus Status { get; set; }
        [Reference]
        public Payment Payment { get; set; }
        [Reference]
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime OrderTime { get; set; }
        [Reference]
        public List<OrderElement> OrderElements { get; set; }
        public string Comments { get; set; }
    }
}