using MarioPizzaOriginal.Domain.Enums;
using System;

namespace Model.Filter
{
    public class OrderFilter
    {
        public int? OrderIdMin { get; set; }
        public int? OrderIdMax { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime LowerTimestamp { get; set; }
        public DateTime HigherTimestamp { get; set; }
    }
}
