using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Model.Enums;

namespace MarioPizzaOriginal.Model
{
    public class MarioPizzaOrder
    {
        public int OrderId { get; set; }
        public Dictionary<FoodSizeSauce, double> OrderList { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderTime { get; set; }
    }
}