using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Controller
{
    public class OrderElementController
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;
        public OrderElementController(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        public MarioResult GetAllOrderElements()
        {
            Console.Clear();
            var orderElements = _marioPizzaRepository.GetAllOrderElements();
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
