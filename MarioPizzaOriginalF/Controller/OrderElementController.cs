using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class OrderElementController
    {
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly MenuCreator _orderElementMenu;
        private readonly ViewHelper _viewHelper;
        public OrderElementController()
        {
            var container = TinyIoCContainer.Current;
            _viewHelper = container.Resolve<ViewHelper>();
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
            _viewHelper.WriteAndWait("Wszystkie elementy zamówień");
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

            var foodId = _viewHelper.AskForInt("Podaj id produktu, który chcesz dodać: ");
            if (!_foodRepository.Exists(foodId))
            {
                _viewHelper.WriteAndWait($"Produkt o id {foodId} nie istnieje!");
                return;
            }
            double amount = _viewHelper.AskForDouble("Podaj ilość: ");
            _orderElementRepository.Add(new OrderElement { OrderId = orderId, FoodId = foodId, Amount = amount });
            string foodName = _foodRepository.GetName(foodId);
            _viewHelper.WriteAndWait($"Dodano {foodName} (x{amount}) do zamówienia o nr {orderId}");
        }

        public void ChangeAmount()
        {
            if (!CheckIfOrderExists("Podaj id zamówienia dla którego chcesz zmienić ilość produktu: ", out var orderId))
                return;

            ShowOrderElementsForOrder(orderId);
            int orderElementId = _viewHelper.AskForInt("Podaj id elementu którego chcesz zmienić ilość: ", clear: false);
            if(!_orderElementRepository.IsElementInOrder(orderId, orderElementId))
            {
                _viewHelper.WriteAndWait($"Element nr {orderElementId} nie należy do tego zamówienia!");
                return;
            }
            var orderElement = _orderElementRepository.Get(orderElementId);
            double amount = orderElement.Amount;
            double newAmount = orderElement.Amount;
            string foodName = _foodRepository.GetName(orderElement.FoodId);
            string input = _viewHelper.AskForString($"Podaj ilość składnika {foodName} (x{orderElement.Amount}): ");
            if (!string.IsNullOrEmpty(input))
            {
                newAmount = Convert.ToDouble(input);
                if(newAmount <= 0)
                {
                    _viewHelper.WriteAndWait("Ilość składnika nie może być mniejsza lub równa zero!");
                    return;
                }
                orderElement.Amount = newAmount;
            }
            _orderElementRepository.Save(orderElement);
            _viewHelper.WriteAndWait($"Zmieniłeś ilość składnika {foodName} z {amount} na {newAmount}");
        }

        public void DeleteOrderElement()
        {
            if (!CheckIfOrderExists("Podaj id zamówienia dla którego chcesz sprawdzić elementy: ", out var orderId))
                return;

            ShowOrderElementsForOrder(orderId);
            int orderElementId = _viewHelper.AskForInt("Podaj id elementu który chcesz usunąć: ", clear: false);
            if (!_orderElementRepository.IsElementInOrder(orderId, orderElementId))
            {
                _viewHelper.WriteAndWait($"Element nr {orderElementId} nie należy do tego zamówienia!");
                return;
            }
            var orderElement = _orderElementRepository.Get(orderElementId);
            string foodName = _foodRepository.GetName(orderElement.FoodId);
            _orderElementRepository.Remove(orderElementId);
            _viewHelper.WriteAndWait($"Usunięto element {foodName} z zamówienia nr {orderId}");
        }

        private void ShowOrderElementsForOrder(int orderId)
        {
            var orderElements = _orderElementRepository.GetElements(orderId);
            foreach (var element in orderElements)
            {
                var food = _foodRepository.Get(element.FoodId);
                Console.WriteLine($"*id#{element.OrderElementId}* {food.FoodName} (x{element.Amount}) = {food.Price * element.Amount} zł");
            }
            _viewHelper.WriteAndWait($"Wszystkie elementy dla zamówienia nr {orderId}");
        }

        private bool CheckIfOrderExists(string message, out int orderId)
        {
            orderId = _viewHelper.AskForInt(message);
            if (_orderRepository.Exists(orderId)) 
                return true;
            _viewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
            return false;
        }

    }
}
