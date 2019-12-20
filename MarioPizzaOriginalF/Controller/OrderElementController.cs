using MarioPizzaOriginal.Domain;
using Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Controller
{
    public class OrderElementController
    {
        private readonly IOrderElementRepository _orderElementRepository;
        public OrderElementController(IOrderElementRepository orderElementRepository)
        {
            _orderElementRepository = orderElementRepository;
        }

        public MarioResult GetAllOrderElements()
        {
            Console.Clear();
            var orderElements = _orderElementRepository.GetAll();
            foreach (var element in orderElements)
            {
                Console.WriteLine($"{element.OrderElementId},{element.OrderId},{element.FoodId},{element.Amount}");
            }
            Console.WriteLine("Wszystkie elementy zamówień");
            Console.ReadLine();
            return new MarioResult { Success = true };
        }

        public MarioResult GetAllElementsForOrder()
        {
            return new MarioResult { Success = true };
        }

        public MarioResult AddOrderElement()
        {
            return new MarioResult { Success = true };
        }

        public MarioResult ChangeAmount()
        {
            return new MarioResult { Success = true };
        }

        public MarioResult DeleteOrderElement()
        {
            return new MarioResult { Success = true };
        }

    }
}
