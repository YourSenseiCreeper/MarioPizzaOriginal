using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.DataAccess;
using Pastel;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class OrderController
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderSubElementRepository _orderSubElementRepository;
        private readonly OrderFilter _orderFilter;
        private readonly MenuCreator _orderMenu;
        public OrderController(TinyIoCContainer container)
        {
            _foodRepository = container.Resolve<IFoodRepository>();
            _orderRepository = container.Resolve<IOrderRepository>();
            _orderElementRepository = container.Resolve<IOrderElementRepository>();
            _orderSubElementRepository = container.Resolve<IOrderSubElementRepository>();
            _orderFilter = new OrderFilter(container);
            _orderMenu = MenuCreator.Create()
                .SetHeader("Dostępne opcje - Zamówienia: ")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Lista wszystkich zamówień", GetAllOrders},
                    {"Zawartość zamówienia", GetOrder},
                    {"Oczekujące", GetOrdersWaiting},
                    {"W trakcie", GetOrdersInProgress},
                    {"Gotowe do dostarczenia", GetOrdersReadyForDelivery},
                    {"Dodaj zamówienie", AddOrder},
                    {"Dodaj element do zamówienia", AddOrderElement},
                    {"Usuń zamówienie", DeleteOrder},
                    {"Zmień status zamówienia", ChangeOrderStatus},
                    {"Przenieś zamówienia do kolejnego etapu", MoveToNextStatus},
                    {"Zmień priorytet zamówienia", ChangeOrderPriority},
                    {"Policz cenę dla zamówienia", CalculatePriceForOrder},
                    {"Wszystkie podelementy zamówienia", ShowAllSubOrderElements},
                    {"Filtruj zamówienia", GetFilteredOrders}
                })
                .AddFooter("Powrót");
        }

        public void OrdersMenu() => _orderMenu.Present();

        public void AddOrder()
        {
            var newOrder = new Order
            {
                Status = OrderStatus.WAITING,
                Priority = OrderPriority.NORMAL,
                OrderTime = DateTime.Now,
                OrderElements = new List<OrderElement>()
            };
            var step = 1;
            var maxStep = 7;

            newOrder.ClientPhoneNumber = ViewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Numer telefonu klienta: ");
            if (newOrder.ClientPhoneNumber.Length != 9)
                ViewHelper.WriteAndWait("UWAGA! Numer klienta nie ma 9 znaków!");

            newOrder.DeliveryAddress = ViewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Adres dostawy: ");
            newOrder.Priority = ViewHelper.AskForOption<OrderPriority>(
                "Dostępne priorytety:", $"(krok {step++} z {maxStep}) Priorytet zamówienia (domyślnie NORMAL): ", "NORMAL");

            OrderElement tempOrderElement;
            Console.Clear();
            Console.WriteLine($"(krok {step++} z {maxStep}) Wybierz elementy zamówienia:");

            new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action> {
                {"Dodaj element do zamówienia", () =>
                {
                    tempOrderElement = new OrderElement
                    {
                        FoodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                        Amount = ViewHelper.AskForInt("Podaj ilość: "),
                        SubOrderElements = new List<OrderSubElement>()
                    };
                    if (ViewHelper.AskForYesNo("Czy chcesz dodać dodatki do tego elementu?"))
                    {
                        new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action>
                        {
                            {"Dodaj dodatek do elementu zamówienia", () =>
                            {
                                tempOrderElement.SubOrderElements.Add(new OrderSubElement
                                {
                                    FoodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                                    Amount = ViewHelper.AskForInt("Podaj ilość: ")
                                });
                            }}
                        }).PresentRightless();
                    }
                    newOrder.OrderElements.Add(tempOrderElement);
                }
            }}).PresentRightless();

            newOrder.Payment = ViewHelper.AskForOption<Payment>("Dostępne opcje: ", $"({step++} z {maxStep}) Wybierz gdzie chcesz zapłacić: ");
            newOrder.PaymentMethod = ViewHelper.AskForOption<PaymentMethod>("Dostępne opcje: ", $"({step++} z {maxStep}) Wybierz w jaki sposób chcesz zapłacić: ");

            if (ViewHelper.AskForYesNo($"({step} z {maxStep}) Czy chcesz dodać to zamówienie?"))
                _orderRepository.Add(newOrder);
        }

        public void AddOrderElement()
        {
            if (OrderNotExists("Podaj id zamówienia: ", out var orderId))
                return;

            var foodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: ");
            if (!_foodRepository.Exists(foodId))
            {
                ViewHelper.WriteAndWait($"Produkt o id {foodId} nie istnieje!");
                return;
            }
            
            var amount = ViewHelper.AskForInt("Podaj ilość: ");
            var order = _orderRepository.GetWithReferences(orderId);
            order.OrderElements.Add(new OrderElement
            {
                FoodId = foodId,
                Amount = amount
            });
            _orderRepository.Save(order);
            var foodName = _foodRepository.GetName(foodId);
            ViewHelper.WriteAndWait($"Dodano {foodName} x {amount} do zamówienia numer {orderId}");
        }

        public void AddSubOrderElement()
        {
            if (OrderNotExists("Podaj id zamówienia: ", out var orderId))
                return;

            int orderElementId = ViewHelper.AskForInt("Podaj id elementu zamówienia: ");
            if (!_orderElementRepository.Exists(orderElementId))
            {
                ViewHelper.WriteAndWait($"Element zamówienia o id {orderElementId} nie istnieje!");
                return;
            }
            var foodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: ");
            var amount = ViewHelper.AskForInt("Podaj ilość: ");

            _orderElementRepository.AddToOrder(orderId, foodId, amount);
            ViewHelper.WriteAndWait($"Dodano dodatek do elementu zamówienia nr {orderId}!");
        }

        public void ChangeOrderStatus()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz zmienić status: ", out var orderId))
                return;
            var order = _orderRepository.Get(orderId);
            Console.WriteLine($"Obecny status zamówienia: {order.Status}");
            OrderStatus newStatus = ViewHelper.AskForOption<OrderStatus>("Dostępne statusy", "Nowy status dla zamówienia: ", order.Status.ToString());
            order.Status = newStatus;
            _orderRepository.Save(order);

            ViewHelper.WriteAndWait($"Nowy status dla zamówienia: {newStatus}");
        }

        public void ChangeOrderPriority()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz zmienić priorytet: ", out var orderId))
                return;
            var order = _orderRepository.Get(orderId);

            Console.WriteLine($"Obecny priorytet zamówienia: {order.Priority}");
            OrderPriority newPriority = ViewHelper.AskForOption<OrderPriority>("Dostępne priorytety", "Nowy priorytet dla zamówienia: ", order.Priority.ToString());
            order.Priority = newPriority;
            _orderRepository.Save(order);

            ViewHelper.WriteAndWait($"Nowy priorytet dla zamówienia: {newPriority}");
        }

        public void DeleteOrder()
        {
            if (OrderNotExists("Podaj id zamówienia które chcesz usunąć: ", out var orderId))
                return;

            _orderRepository.DeleteOrderWithAllElements(orderId);
            ViewHelper.WriteAndWait($"Usunięto zamówienie nr {orderId} wraz z elementami i podelementami");
        }

        public void EditOrder()
        {
            throw new NotImplementedException();
            //You can try to modify "Filter" to manage edited values and replace them when not DEFAULT
        }

        public void CalculatePriceForOrder()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz sprawdzić cenę: ", out var orderId))
                return;
            double price = _orderRepository.CalculatePriceForOrder(orderId);
            ViewHelper.WriteAndWait($"Koszt zamówienia nr {orderId} wynosi {price} zł");
        }

        public void GetAllOrders() => ShowOrders(_orderRepository.GetAll());
        public void GetOrdersWaiting() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.WAITING));
        public void GetOrdersInProgress() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.IN_PROGRESS));
        public void GetOrdersReadyForDelivery() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.DELIVERY));
        public void GetOrdersDone() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.DONE));

        public void MoveToNextStatus()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz zmienić status: ", out var orderId))
                return;
 
            var order = _orderRepository.Get(orderId);
            OrderStatus currentStatus = order.Status;
            order.Status = (OrderStatus) ((int)currentStatus + 1);

            if (currentStatus == OrderStatus.DONE)
            {
                ViewHelper.WriteAndWait("Obecny status zamówienia to: DONE, nie możesz zmienić statusu!");
                return;
            }
            _orderRepository.Save(order);
            ViewHelper.WriteAndWait($"Nowy status zamówienia: {order.Status}");
        }

        public void GetOrder()
        {
            if (OrderNotExists("Podaj id zamówienia: ", out var orderId))
                return;

            var selectedOrder = _orderRepository.GetOrderWithAllElements(orderId);
            Console.Clear();
            Console.WriteLine($"=== Informacje dla zamówienia id = {orderId} ===");
            Console.WriteLine($"{"Data złożenia:",-20} {selectedOrder.OrderTime:dd/MM/yyyy HH:MM:ss}");
            Console.WriteLine($"{"Adres dostawy:",-20} {selectedOrder.DeliveryAddress}");
            Console.WriteLine($"{"Numer tel. klienta:",-20} {selectedOrder.ClientPhoneNumber}");
            Console.WriteLine($"{"Priorytet:",-20} {selectedOrder.Priority}");
            Console.WriteLine($"{"Status:",-20} {selectedOrder.Status}");
            var price = 0d;
            if (selectedOrder.OrderElements != null)
            {
                price = CalculateOrderPrice(selectedOrder.OrderElements);
                ShowOrderElements(selectedOrder.OrderElements);
            }
            else Console.WriteLine("Brak elementów zamówienia!");
            Console.WriteLine($"{"Cena:",-20} {price} zł");
            
            Console.ReadLine();
        }

        public void ShowAllSubOrderElements()
        {
            var subOrderElements = _orderSubElementRepository.GetAll();
            foreach (var subOrderElement in subOrderElements)
            {
                Console.WriteLine($"{subOrderElement.OrderSubElementId}, {subOrderElement.OrderElementId}, {subOrderElement.FoodId}, {subOrderElement.Amount}");
            }
            ViewHelper.WriteAndWait($"{subOrderElements.Count} wyników");
        }

        public void GetFilteredOrders()
        {
            if (!_orderFilter.FilterMenu()) return;
            var results = _orderFilter.Query();
            ShowOrders(results);
        }


        private double CalculateOrderPrice(IEnumerable<OrderElement> orderElements)
        {
            double price = 0;
            if (orderElements == null)
                return price;

            foreach (var element in orderElements)
            {
                price += element.Amount * element.Food.Price;
                if (element.SubOrderElements == null) 
                    continue;
                price += element.SubOrderElements.Sum(subElement => subElement.Amount * subElement.Food.Price);
            }
            return price;
        }

        private bool OrderNotExists(string message, out int orderId)
        {
            var result = ViewHelper.CheckIfElementNotExists(_orderRepository, 
                message, "Zamówienie o id {0} nie istnieje!", out var elmElementId);
            orderId = elmElementId;
            return result;
        }

        private void ShowOrders(List<Order> orderList)
        {
            Console.Clear();
            var header = $"{"Nr zam.",7}|{"Data",20}|{"Status",12}|{"Priorytet",10}|{"Nr tel",10}|{"Adres",15}";
            Console.WriteLine(header);
            Console.WriteLine(new string('=', header.Length));
            orderList.ForEach(order => Console.WriteLine($"{order.OrderId,7}|{order.OrderTime,20}|{ViewHelper.StatusColor(order.Status),12}|" +
                                                         $"{order.Priority,10}|{order.ClientPhoneNumber,10}|{order.DeliveryAddress,15}"));
            if (orderList.Count > 1)
                Console.WriteLine($"Znaleziono {orderList.Count} wyników");
            Console.ReadLine();
        }

        private void ShowOrderElements(IEnumerable<OrderElement> orderElements)
        {
            Console.WriteLine("Elementy zamówienia:");
            foreach (var orderElement in orderElements)
            {
                Console.WriteLine($"* {orderElement.Food.FoodName} x {orderElement.Amount} = {orderElement.Food.Price * orderElement.Amount} zł");
                if (orderElement.SubOrderElements == null)
                    continue;

                foreach (var subOrderElement in orderElement.SubOrderElements)
                {
                    Console.WriteLine($"\t* {subOrderElement.Food.FoodName} x{subOrderElement.Amount} = " +
                                      $"{subOrderElement.Food.Price * subOrderElement.Amount} zł");
                }
            }
        }

        private void ShowSubOrderElements(List<OrderSubElement> subOrderElements)
        {
            Console.WriteLine("Elementy dodatkowe dla elementu zamówienia:");
            string subOrderSpace = " * ";
            foreach (var subOrderElement in subOrderElements)
            {
                string foodName = _foodRepository.GetName(subOrderElement.FoodId);
                Console.WriteLine($"{subOrderSpace}{foodName} (x{subOrderElement.Amount})");
            }
        }
    }
}
