﻿using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.DataAccess;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class OrderElementController
    {
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly MenuCreator _orderElementMenu;
        public OrderElementController(TinyIoCContainer container)
        {
            _orderElementRepository = container.Resolve<IOrderElementRepository>();
            _orderRepository = container.Resolve<IOrderRepository>();
            _foodRepository = container.Resolve<IFoodRepository>();
            _orderElementMenu = MenuCreator.Create()
                .SetHeader("Dostępne opcje - Elementy zamówienia:")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Lista wszystkich elementów zamówień", GetAllOrderElements},
                    {"Lista elementów zamówienia", GetAllElementsForOrder},
                    {"Dodaj element do zamówienia", AddOrderElement},
                    {"Zmień ilość", ChangeAmount},
                    {"Usuń element", DeleteOrderElement}
                })
                .AddFooter("Powrót");
        }

        public void OrderElementsMenu() => _orderElementMenu.Present();

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
            if (!CheckIfOrderExists("Podaj id zamówienia dla którego chcesz sprawdzić elementy: ", out var orderId))
                return;
            ShowOrderElementsForOrder(orderId);
        }

        public void AddOrderElement()
        {
            if (!CheckIfOrderExists("Podaj id zamówienia dla którego chcesz dodać element: ", out var orderId))
                return;

            int foodId = ViewHelper.AskForInt("Podaj id produktu, który chcesz dodać: ");
            //TODO: Check if food exists
            double amount = ViewHelper.AskForDouble("Podaj ilość: ");
            _orderElementRepository.Add(new OrderElement { OrderId = orderId, FoodId = foodId, Amount = amount });
            string foodName = _foodRepository.GetName(foodId);
            ViewHelper.WriteAndWait($"Dodano {foodName} (x{amount}) do zamówienia o nr {orderId}");
        }

        public void ChangeAmount()
        {
            if (!CheckIfOrderExists("Podaj id zamówienia dla którego chcesz zmienić ilość produktu: ", out var orderId))
                return;

            ShowOrderElementsForOrder(orderId);
            int orderElementId = ViewHelper.AskForInt("Podaj id elementu którego chcesz zmienić ilość: ", clear: false);
            if(!_orderElementRepository.IsElementInOrder(orderId, orderElementId))
            {
                ViewHelper.WriteAndWait($"Element nr {orderElementId} nie należy do tego zamówienia!");
                return;
            }
            var orderElement = _orderElementRepository.Get(orderElementId);
            double amount = orderElement.Amount;
            double newAmount = orderElement.Amount;
            string foodName = _foodRepository.GetName(orderElement.FoodId);
            string input = ViewHelper.AskForString($"Podaj ilość składnika {foodName} (x{orderElement.Amount}): ");
            if (!string.IsNullOrEmpty(input))
            {
                newAmount = Convert.ToDouble(input);
                if(newAmount <= 0)
                {
                    ViewHelper.WriteAndWait("Ilość składnika nie może być mniejsza lub równa zero!");
                    return;
                }
                orderElement.Amount = newAmount;
            }
            _orderElementRepository.Save(orderElement);
            ViewHelper.WriteAndWait($"Zmieniłeś ilość składnika {foodName} z {amount} na {newAmount}");
        }

        public void DeleteOrderElement()
        {
            if (!CheckIfOrderExists("Podaj id zamówienia dla którego chcesz sprawdzić elementy: ", out var orderId))
                return;

            ShowOrderElementsForOrder(orderId);
            int orderElementId = ViewHelper.AskForInt("Podaj id elementu który chcesz usunąć: ", clear: false);
            if (!_orderElementRepository.IsElementInOrder(orderId, orderElementId))
            {
                ViewHelper.WriteAndWait($"Element nr {orderElementId} nie należy do tego zamówienia!");
                return;
            }
            var orderElement = _orderElementRepository.Get(orderElementId);
            string foodName = _foodRepository.GetName(orderElement.FoodId);
            _orderElementRepository.Remove(orderElementId);
            ViewHelper.WriteAndWait($"Usunięto element {foodName} z zamówienia nr {orderId}");
        }

        private void ShowOrderElementsForOrder(int orderId)
        {
            Food food;
            var orderElements = _orderElementRepository.GetElements(orderId);
            foreach (var element in orderElements)
            {
                food = _foodRepository.Get(element.FoodId);
                Console.WriteLine($"*id#{element.OrderElementId}* {food.FoodName} (x{element.Amount}) = {food.Price * element.Amount} zł");
            }
            ViewHelper.WriteAndWait($"Wszystkie elementy dla zamówienia nr {orderId}");
        }

        private bool CheckIfOrderExists(string message, out int orderId)
        {
            orderId = ViewHelper.AskForInt(message);
            if (_orderRepository.Exists(orderId)) 
                return true;
            ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
            return false;
        }

    }
}
