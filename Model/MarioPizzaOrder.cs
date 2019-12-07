using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.Enums;
using Model;

namespace MarioPizzaOriginal.Domain
{
    public class MarioPizzaOrder
    {
        public int OrderId { get; set; }
        public List<OrderElement> OrderElements { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderTime { get; set; }
    }
}