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
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        public OrderElementController(IOrderElementRepository orderElementRepository, IOrderRepository orderRepository, IFoodRepository foodRepository)
        {
            _orderElementRepository = orderElementRepository;
            _orderRepository = orderRepository;
            _foodRepository = foodRepository;
        }

        public void GetAllOrderElements()
        {
            Console.Clear();
            var orderElements = _orderElementRepository.GetAll();
            foreach (var element in orderElements)
            {

                Console.WriteLine($"{element.OrderElementId},{element.OrderId},{element.FoodId},{element.Amount}");
            }
            ViewHelper.WriteAndWait("Wszystkie elementy zamówień");
        }

        public void GetAllElementsForOrder()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz sprawdzić elementy: ");
            Food food;

            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            var orderElements = _orderElementRepository.GetElements(orderId);
            foreach (var element in orderElements)
            {
                food = _foodRepository.Get(element.FoodId);
                Console.WriteLine($"* {food.FoodName} (x{element.Amount}) = {food.Price * element.Amount} zł");
            }
            ViewHelper.WriteAndWait($"Wszystkie elementy dla zamówienia nr {orderId}");
        }

        public void AddOrderElement()
        {
        }

        public void ChangeAmount()
        {
        }

        public void DeleteOrderElement()
        {
        }

    }
}
