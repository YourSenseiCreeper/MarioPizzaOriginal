using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.Enums;
using Model;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class MarioPizzaOrder
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
        [Required]
        public DateTime OrderTime { get; set; }
    }
}