using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model;
using Model.DataAccess;
using Model.Enums;
using Model.Filter;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Controller
{
    public class OrderController
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderSubElementRepository _orderSubElementRepository;
        public OrderController(IFoodRepository foodRepository, IOrderRepository orderRepository, 
            IOrderElementRepository orderElementRepository, IOrderSubElementRepository orderSubElementRepository)
        {
            _foodRepository = foodRepository;
            _orderRepository = orderRepository;
            _orderElementRepository = orderElementRepository;
            _orderSubElementRepository = orderSubElementRepository;
        }

        public void AddOrder()
        {
            //DB check
            //I still can add an order and group them by add timestamp and check if the first one has correct data - that's it
            var newOrder = new MarioPizzaOrder
            {
                OrderId = 404,  //OrderId will be replaced
                Status = OrderStatus.WAITING,
                Priority = OrderPriority.NORMAL,
                OrderTime = DateTime.Now,
            };
            int step = 1;
            int maxStep = 7;

            newOrder.ClientPhoneNumber = ViewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Numer telefonu klienta: ");
            if (newOrder.ClientPhoneNumber.Length > 9)
            {
                Console.WriteLine("UWAGA! Numer klienta przekracza 9 znaków!");
            }

            newOrder.DeliveryAddress = ViewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Adres dostawy: ");
            newOrder.Priority = ViewHelper.AskForOption<OrderPriority>("Dostępne priorytety:", $"(krok {step++} z {maxStep}) Priorytet zamówienia (domyślnie NORMAL): ", "NORMAL");

            bool addAnother = true;
            string input;
            List<OrderElement> orderElements = new List<OrderElement>();
            OrderElement tempOrderElement;

            Console.Clear();
            Console.WriteLine($"(krok {step++} z {maxStep}) Wybierz elementy zamówienia:");
            while (addAnother)
            {
                Console.WriteLine("Możliwe opcje:");
                Console.WriteLine("1. Dodaj element do zamówienia");
                Console.WriteLine("2. Zakończ dodawanie");
                input = Console.ReadLine();
                if (input.Equals("1"))
                {
                    tempOrderElement = new OrderElement
                    {
                        FoodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                        Amount = ViewHelper.AskForInt("Podaj ilość: "),
                    };
                    //SubOrder
                    if(ViewHelper.AskForYesNo("Czy chcesz dodać dodatki do tego elementu?"))
                    {
                        bool addAnotherSub = true;
                        string subInput;
                        tempOrderElement.SubOrderElements = new List<OrderSubElement>();
                        while (addAnotherSub)
                        {
                            Console.Clear();
                            Console.WriteLine("Możliwe opcje:");
                            Console.WriteLine("1. Dodaj dodatek do do elementu zamówienia");
                            Console.WriteLine("2. Zakończ dodawanie");
                            subInput = Console.ReadLine();
                            if (subInput.Equals("1"))
                            {
                                tempOrderElement.SubOrderElements.Add(new OrderSubElement
                                {
                                    FoodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                                    Amount = ViewHelper.AskForInt("Podaj ilość: ")
                                });
                            }
                            else if (subInput.Equals("2"))
                            {
                                addAnotherSub = false;
                                ViewHelper.WriteAndWait("Dodawanie dodatków zakończone!");
                            }
                            else Console.WriteLine($"Nie ma opcji: {subInput}!");
                        }
                    }
                    orderElements.Add(tempOrderElement);
                }
                else if (input.Equals("2"))
                {
                    addAnother = false;
                    ViewHelper.WriteAndWait("Dodawanie elementów zamówienia zakończone!");
                }
                else Console.WriteLine($"Nie ma opcji: {input}!");
            }

            newOrder.Payment = ViewHelper.AskForOption<Payment>("Dostępne opcje: ", $"({step++} z {maxStep}) Wybierz gdzie chcesz zapłacić: ");
            newOrder.PaymentMethod = ViewHelper.AskForOption<PaymentMethod>("Dostępne opcje: ", $"({step++} z {maxStep}) Wybierz w jaki sposób chcesz zapłacić: ");

            if (ViewHelper.AskForYesNo($"({step} z {maxStep}) Czy chcesz dodać to zamówienie?"))
            {
                //Edit all ids to the latest I hope
                newOrder.OrderId = _orderRepository.OrderNextId();
                var orderElementId = _orderElementRepository.OrderElementNextId();
                foreach (var orderElement in orderElements)
                {
                    orderElement.OrderId = newOrder.OrderId;
                    orderElement.OrderElementId = orderElementId++;
                    _orderElementRepository.Add(orderElement);
                    if (orderElement.SubOrderElements != null)
                    {
                        foreach (var subOrderElement in orderElement.SubOrderElements)
                        {
                            subOrderElement.OrderElementId = orderElement.OrderElementId;
                            _orderSubElementRepository.Add(subOrderElement);
                        }
                    }
                }
                _orderRepository.Add(newOrder);
            }
        }

        public void AddOrderElement()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia: ");
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            int foodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: ");
            int amount = ViewHelper.AskForInt("Podaj ilość: ");
            _orderElementRepository.AddToOrder(orderId, foodId, amount);
            string foodName = _foodRepository.GetName(foodId);
            ViewHelper.WriteAndWait($"Dodano {foodName} x {amount} do zamówienia o id {orderId}");
        }

        public void AddSubOrderElement()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia: ");
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            int orderElementId = ViewHelper.AskForInt("Podaj id zamówienia: ");
            if (!_orderElementRepository.Exists(orderElementId))
            {
                ViewHelper.WriteAndWait($"Element zamówienia o id {orderElementId} nie istnieje!");
                return;
            }
            int foodId = ViewHelper.AskForInt("Podaj id produktu który chcesz dodać: ");
            int amount = ViewHelper.AskForInt("Podaj ilość: ");

            _orderElementRepository.AddToOrder(orderId, foodId, amount);
            ViewHelper.WriteAndWait($"Dodano dodatek do elementu zamówienia nr {orderId}!");
        }

        public void ChangeOrderStatus()
        {
            var orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz zmienić status: ");
            var order = _orderRepository.Get(orderId);
            if (order == null)
            {
                ViewHelper.WriteAndWait($"Zamówienie o id = {orderId} nie istnieje!");
                return;
            }
            Console.WriteLine($"Obecny status zamówienia: {order.Status}");
            OrderStatus newStatus = ViewHelper.AskForOption<OrderStatus>("Dostępne statusy", "Nowy status dla zamówienia: ", order.Status.ToString());
            order.Status = newStatus;
            _orderRepository.Edit(order);

            ViewHelper.WriteAndWait($"Nowy status dla zamówienia: {newStatus}");
        }

        public void ChangeOrderPriority()
        {
            var orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz zmienić priorytet: ");
            var order = _orderRepository.Get(orderId);
            if (order == null)
            {
                ViewHelper.WriteAndWait($"Zamówienie o id = {orderId} nie istnieje!");
                return;
            }
            Console.WriteLine($"Obecny priorytet zamówienia: {order.Priority}");
            OrderPriority newPriority = ViewHelper.AskForOption<OrderPriority>("Dostępne priorytety", "Nowy priorytet dla zamówienia: ", order.Priority.ToString());
            order.Priority = newPriority;
            _orderRepository.Edit(order);

            ViewHelper.WriteAndWait($"Nowy priorytet dla zamówienia: {newPriority}");
        }

        public void DeleteOrder()
        {
            var orderId = ViewHelper.AskForInt("Podaj id zamówienia które chcesz usunąć: ");
            if(!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie od id {orderId} nie istnieje!");
                return;
            }
            _orderRepository.Remove(orderId);
            List<OrderElement> orderElements = _orderElementRepository.GetElements(orderId);
            foreach(var orderElement in orderElements)
            {
                _orderElementRepository.Remove(orderElement.OrderElementId);
                List<OrderSubElement> orderSubElements = _orderSubElementRepository.GetSubElements(orderElement.OrderElementId);
                if(orderSubElements.Count != 0)
                {
                    orderSubElements.ForEach(x => _orderSubElementRepository.Remove(x.SubOrderElementId));
                }
            }
            ViewHelper.WriteAndWait($"Usubnięto zamówienie nr {orderId} wraz z elementami i podelementami");
        }

        public void EditOrder()
        {
            //You can try to modify "Filter" to manage edited values and replace them when not DEFAULT
        }

        public void CalculatePriceForOrder()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz sprawdzić cenę: ");
            if (!_orderRepository.Exists(orderId))
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            double price = _orderRepository.CalculatePriceForOrder(orderId);
            ViewHelper.WriteAndWait($"Koszt zamówienia nr {orderId} wynosi {price} zł");
        }

        private void ShowOrders(List<MarioPizzaOrder> orderList)
        {
            Console.Clear();
            List<string> headerElements = new List<string> { "Nr zam.", "Data", "Status", "Priorytet", "Nr tel", "Adres" };
            var header = $"{headerElements[0].PadRight(7)}|" +
                    $"{headerElements[1].PadRight(20)}|" +
                    $"{headerElements[2].PadRight(12)}|" +
                    $"{headerElements[3].PadRight(10)}|" +
                    $"{headerElements[4].PadRight(10)}|" +
                    $"{headerElements[5].PadRight(15)}";
            Console.WriteLine(header);
            for(int i = 0; i<header.Length; i++)
            {
                Console.Write("=");
            }
            Console.Write("\n");
            foreach (var order in orderList)
            {
                Console.WriteLine($"{order.OrderId.ToString().PadRight(7)}|" +
                    $"{order.OrderTime.ToString().PadRight(20)}|" +
                    $"{order.Status.ToString().PadRight(12)}|" +
                    $"{order.Priority.ToString().PadRight(10)}|" +
                    $"{order.ClientPhoneNumber.PadRight(10)}|" +
                    $"{order.DeliveryAddress.PadRight(15)}");
            }
            if (orderList.Count > 1)
            {
                Console.WriteLine($"Znaleziono {orderList.Count} wyników");
            }
            Console.ReadLine();
        }
        public void GetAllOrders()
        {
            ShowOrders(_orderRepository.GetAll());
        }

        public void GetFilteredOrders()
        {
            var filter = new OrderFilter
            {
                LowerTimestamp = DateTime.MinValue,
                HigherTimestamp = DateTime.MaxValue
            };
            Console.WriteLine("Wybierz numer filtra by go dostosować. Wpisz -1 by anulować zmiany");
            List<string> filterList;
            string option = "";
            bool exit = false;
            bool filterAction = false;
            while (!exit)
            {
                filterList = new List<string> {
                    "1. Minimalne Id zamówienia" + (filter.OrderIdMin != null ? $"({filter.OrderIdMin})" : ""),
                    "2. Maksymalne Id zamówienia" + (filter.OrderIdMax != null ? $"({filter.OrderIdMax})" : ""),
                    "3. Numer klienta zawiera" + (filter.ClientPhoneNumber != null ? $"({filter.ClientPhoneNumber})" : ""),
                    "4. Adres dostawy zawiera" + (filter.DeliveryAddress != null ? $"({filter.DeliveryAddress})" : ""),
                    "5. Priorytet" + (filter.Priority != OrderPriority.NONE ? $"({filter.Priority})" : ""),
                    "6. Status" + (filter.Status != OrderStatus.NONE ? $"({filter.Status})" : ""),
                    "7. Dolny znacznik czasowy zamówienia" + (filter.LowerTimestamp != DateTime.MinValue ? $"({filter.LowerTimestamp.ToString()})" : ""),
                    "8. Górny znacznik czasowy zamówienia" + (filter.HigherTimestamp != DateTime.MaxValue ? $"({filter.HigherTimestamp.ToString()})" : ""),
                    "9. WYŚWIETL WYNIKI",
                    "10. Wyjdź"
                };
                filterList.ForEach(x => Console.WriteLine(x));
                option = Console.ReadLine();
                
                switch (option)
                {
                    case "1": filter.OrderIdMin = ViewHelper.FilterInt("Podaj wartość dla Minimalne Id zamówienia: ", true); break;
                    case "2": filter.OrderIdMax = ViewHelper.FilterInt("Podaj wartość dla Maksymalne Id zamówienia: ", true); break;
                    case "3": filter.ClientPhoneNumber = ViewHelper.FilterString("Podaj wartość ciąg który znajduje się w Numerze klienta: "); break;
                    case "4": filter.DeliveryAddress = ViewHelper.FilterString("Podaj wartość ciąg który znajduje się w Adresie dostawy: "); break;
                    case "5": filter.Priority = ViewHelper.FilterOption<OrderPriority>(""); break;
                    case "6": filter.Status = ViewHelper.FilterOption<OrderStatus>(""); break;
                    case "7": filter.LowerTimestamp = ViewHelper.FilterDateTime($"Wybierz dolny zakres czasu zamówienia.\n Datę wprowadź w formacie: dzień miesiąc rok godzina minuta sekunda", true); break;
                    case "8": filter.HigherTimestamp = ViewHelper.FilterDateTime($"Wybierz górny zakres czasu zamówienia.\n Datę wprowadź w formacie: dzień miesiąc rok godzina minuta sekunda", false); break;
                    case "9": filterAction = true; break;
                    case "10": exit = true; break;
                    default:
                        ViewHelper.WriteAndWait($"Nie ma opcji {option}!");
                        break;
                }
            }
            if (filterAction)
            {
                Console.WriteLine("Wybrane zamówienia:");
                var filteredElements = _orderRepository.Filter(filter);
                ShowOrders(filteredElements);
            }
        }

        public void GetOrdersWaiting() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.WAITING));
        public void GetOrdersInProgress() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.IN_PROGRESS));
        public void GetOrdersReadyForDelivery() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.DELIVERY));
        public void GetOrdersDone() => ShowOrders(_orderRepository.GetByStatus(OrderStatus.DONE));

        public void MoveToNextStatus()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz zmienić status: ");
            var order = _orderRepository.Get(orderId);
            if (order == null)
            {
                ViewHelper.WriteAndWait($"Nie znaleziono zamówienia o id {orderId}");
                return;
            }
            OrderStatus currentStatus = order.Status;
            order.Status = (OrderStatus) ((int)currentStatus + 1);
            if (currentStatus != OrderStatus.DONE)
            {
                _orderRepository.Edit(order);
                ViewHelper.WriteAndWait($"Nowy status zamówienia: {order.Status.ToString()}");
            }
            else
            {
                ViewHelper.WriteAndWait("Obecny status zamówienia to: DONE, nie możesz zmienić statusu!");
            }
        }

        private void ShowOrderElements(List<OrderElement> orderElements)
        {
            Console.WriteLine("Elementy zamówienia:");
            string orderSpace = " * ";
            string subOrderSpace = "    * ";
            Food food;
            Food subFood;
            foreach(var orderElement in orderElements)
            {
                food = _foodRepository.Get(orderElement.FoodId);
                Console.WriteLine($"{orderSpace}{food.FoodName} (x{orderElement.Amount}) = {food.Price*orderElement.Amount} zł");

                var subOrderElements = _orderSubElementRepository.GetSubElements(orderElement.OrderElementId);
                if(subOrderElements.Count != 0)
                {
                    foreach(var subOrderElement in subOrderElements)
                    {
                        subFood = _foodRepository.Get(subOrderElement.FoodId);
                        Console.WriteLine($"{subOrderSpace}{subFood.FoodName} (x{subOrderElement.Amount}) = {subFood.Price*subOrderElement.Amount} zł");
                    }
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

        public void GetOrder()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia: ");
            var selectedOrder = _orderRepository.Get(orderId);
            if (selectedOrder == null)
            {
                ViewHelper.WriteAndWait($"Zamówienie o id {orderId} nie istnieje!");
                return;
            }
            Console.Clear();
            Console.WriteLine($"=== Informacje dla zamówienia id = {orderId} ===");
            Console.WriteLine($"Data złożenia: {selectedOrder.OrderTime.ToString("dd/MM/yyyy HH:MM:ss")}");
            Console.WriteLine($"Adres dostawy: {selectedOrder.DeliveryAddress}");
            Console.WriteLine($"Numer tel. klienta: {selectedOrder.ClientPhoneNumber}");
            Console.WriteLine($"Priorytet: {selectedOrder.Priority}");
            Console.WriteLine($"Status: {selectedOrder.Status}");
            Console.WriteLine($"Cena: {_orderRepository.CalculatePriceForOrder(orderId)} zł");
            //And SubOrder Elements
            ShowOrderElements(_orderElementRepository.GetElements(orderId));
            Console.ReadLine();
        }

        public void ShowAllSubOrderElements()
        {
            var subOrderElements = _orderSubElementRepository.GetAll();
            foreach (var subOrderElement in subOrderElements)
            {
                Console.WriteLine($"{subOrderElement.SubOrderElementId}, {subOrderElement.OrderElementId}, {subOrderElement.FoodId}, {subOrderElement.Amount}");
            }
            ViewHelper.WriteAndWait($"{subOrderElements.Count} wyników");
        }

    }
}
