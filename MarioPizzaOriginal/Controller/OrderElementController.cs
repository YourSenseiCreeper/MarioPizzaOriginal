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
            foreach(var element in _marioPizzaRepository.GetAllOrderElements())
            {
                Console.WriteLine($"{element.OrderElementId},{element.OrderId},{element.FoodId},{element.Amount}");
            }
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
