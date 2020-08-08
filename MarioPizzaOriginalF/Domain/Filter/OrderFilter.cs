using MarioPizzaOriginal.Domain.Enums;
using System;
using System.Collections.Generic;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class OrderFilter : Filter<Order>
    {
        public OrderFilter(TinyIoCContainer container) : base(container)
        {
            FilterObjects = new List<FilterObject>
            {
                new FilterObject("Minimalne Id zamówienia", "Podaj wartość dla Minimalne Id zamówienia: ", "OrderId >= {0}", typeof(int)),
                new FilterObject("Maksymalne Id zamówienia", "Podaj wartość dla Maksymalne Id zamówienia: ", "OrderId <= {0}", typeof(int)),
                new FilterObject ( "Numer telefonu klienta zawiera", "Podaj ciąg który znajduje się w nr tel klienta: ", "ClientPhoneNumber like '%{0}%'", typeof(string)),
                new FilterObject ( "Adres dostawy zawiera", "Podaj ciąg który znajduje się w Adresie dostawy: ", "DeliveryAddress like '%{0}%'", typeof(string)),
                new FilterObject ( "Priorytet zamówienia", "Wybierz priorytet zamówienia: ", "Priority = {0}", typeof(OrderPriority)),
                new FilterObject ( "Status zamówienia", "Wybierz status zamówienia: ", "Status = {0}", typeof(OrderStatus)),
                new FilterObject ( "Dolna granica czasu zamówienia", "Podaj dolną granicę czasu zamówienia zamówienia np. 01/01/2000 06:15: ", "OrderTime >= '{0}'", typeof(DateTime)),
                new FilterObject ( "Górna granica czasu zamówienia", "Podaj górną granicę czasu zamówienia zamówienia np. 01/01/2000 06:15: ", "OrderTime <= '{0}'", typeof(DateTime)),
            };
        }
    }
}
