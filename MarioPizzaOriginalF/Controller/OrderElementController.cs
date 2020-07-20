using MarioPizzaOriginal.Domain;
using Model.DataAccess;
using System;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class OrderElementController
    {
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        public OrderElementController(TinyIoCContainer container)
        {
            _orderElementRepository = container.Resolve<IOrderElementRepository>();
            _orderRepository = container.Resolve<IOrderRepository>();
            _foodRepository = container.Resolve<IFoodRepository>();
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
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            ShowOrderElementsForOrder(orderId);
        }

        public void AddOrderElement()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz dodać element: ");
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            int foodId = ViewHelper.AskForInt("Podaj id produktu, który chcesz dodać: ");
            double amount = ViewHelper.AskForDouble("Podaj ilość: ");
            _orderElementRepository.Add(new Model.OrderElement { OrderId = orderId, FoodId = foodId, Amount = amount });
            string foodName = _foodRepository.GetName(foodId);
            ViewHelper.WriteAndWait($"Dodano {foodName} (x{amount}) do zamówienia o nr {orderId}");
        }

        public void ChangeAmount()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz zmienić ilość produktu: ");
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
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
            _orderElementRepository.Edit(orderElement);
            ViewHelper.WriteAndWait($"Zmieniłeś ilość składnika {foodName} z {amount} na {newAmount}");
        }

        public void DeleteOrderElement()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz sprawdzić elementy: ");
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
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

    }
}
