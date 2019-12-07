using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Controller
{
    public class OrderController
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;
        public OrderController(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        public void AddOrder()
        {
            //DB check
            //I still can add an order and group them by add timestamp and check if the first one has correct data - that's it
            var newOrder = new MarioPizzaOrder();
            newOrder.OrderId = 69;
            int step = 1;
            int maxStep = 7;
            //First i need to have a possibility to add a FoodSizeSauce (pick from a list) to my Dictionary
            //OrderList (required!)
            //ClientPhoneNumber (required!)
            Console.Write($"(krok {step++} z {maxStep}) Numer telefonu klienta: ");
            newOrder.ClientPhoneNumber = Console.ReadLine();
            if (newOrder.ClientPhoneNumber.Length > 9)
            {
                Console.WriteLine("UWAGA! Numer klienta przekracza 9 znaków!");
            }

            Console.Write($"(krok {step++} z {maxStep}) Adres dostawy: ");
            newOrder.DeliveryAddress = Console.ReadLine();
            if (newOrder.DeliveryAddress.Equals(""))
            {
                Console.WriteLine("Adres nie może być pusty!");
            }

            var allPriorities = String.Join(',', Enum.GetNames(typeof(OrderPriority)));
            Console.WriteLine($"Dostępne priorytety: {allPriorities}");
            Console.Write($"(krok {step++} z {maxStep}) Priorytet zamówienia (domyślnie NORMAL): ");
            var orderPriority = Console.ReadLine();
            OrderPriority priority = orderPriority.Equals("") ? OrderPriority.NORMAL : (OrderPriority)Enum.Parse(typeof(OrderPriority), orderPriority.ToUpper());
            newOrder.Priority = priority;
            newOrder.Status = OrderStatus.WAITING;

            bool addAnother = true;
            string input;
            newOrder.OrderElements = new List<OrderElement>();
            OrderElement tempOrderElement;
            SubOrderElement tempSubOrderElement;
            Console.WriteLine($"(krok {step++} z {maxStep}) Wybierz elementy zamówienia:");
            while (addAnother)
            {
                Console.WriteLine("Możliwe opcje:");
                Console.WriteLine("1. Dodaj element do zamówienia");
                Console.WriteLine("2. Zakończ dodawanie");
                input = Console.ReadLine();
                if (input.Equals("1"))
                {
                    tempOrderElement = new OrderElement();
                    Console.Write("Podaj id produktu który chcesz dodać: ");
                    tempOrderElement.FoodId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Podaj ilość: ");
                    tempOrderElement.Amount = Convert.ToInt32(Console.ReadLine());

                    //SubOrder
                    Console.Write("Czy chcesz dodać dodatki do tego elementu? (y/n): ");
                    string addOrderSubElement = Console.ReadLine();

                    if(addOrderSubElement.ToLower().Equals("y") || 
                        addOrderSubElement.ToLower().Equals("yes") || 
                        addOrderSubElement.ToLower().Equals("tak"))
                    {
                        bool addAnotherSub = true;
                        string subInput;
                        tempOrderElement.SubOrderElements = new List<SubOrderElement>();
                        while (addAnotherSub)
                        {
                            Console.WriteLine("Możliwe opcje:");
                            Console.WriteLine("1. Dodaj dodatek do do elementu zamówienia");
                            Console.WriteLine("2. Zakończ dodawanie");
                            subInput = Console.ReadLine();
                            if (subInput.Equals("1"))
                            {
                                tempSubOrderElement = new SubOrderElement();
                                Console.Write("Podaj id produktu który chcesz dodać: ");
                                tempSubOrderElement.FoodId = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Podaj ilość: ");
                                tempSubOrderElement.Amount = Convert.ToInt32(Console.ReadLine());
                                tempOrderElement.SubOrderElements.Add(tempSubOrderElement);
                            }
                            else if (subInput.Equals("2"))
                            {
                                addAnotherSub = false;
                                Console.WriteLine("Dodawanie dodatków zakończone!");
                            }
                            else Console.WriteLine($"Nie ma opcji: {subInput}!");
                        }
                    }
                    newOrder.OrderElements.Add(tempOrderElement);
                }
                else if (input.Equals("2"))
                {
                    addAnother = false;
                    Console.WriteLine("Dodawanie elementów zamówienia zakończone!");
                }
                else
                {
                    Console.WriteLine($"Nie ma opcji: {input}!");
                }
            }
            Console.Write("Czy chcesz dodać to zamówienie? (y/n): ");
            string addThisOrder = Console.ReadLine();

            if (addThisOrder.ToLower().Equals("y") || addThisOrder.ToLower().Equals("yes") || addThisOrder.ToLower().Equals("tak"))
            {
                //Edit all ids to the latest I hope
                newOrder.OrderId = _marioPizzaRepository.OrderCount();
                var orderElementId = _marioPizzaRepository.OrderElementsCount();
                foreach (var orderElement in newOrder.OrderElements)
                {
                    orderElement.OrderElementId = orderElementId++;
                    if (orderElement.SubOrderElements != null)
                    {
                        foreach (var subOrderElement in orderElement.SubOrderElements)
                        {
                            subOrderElement.OrderElementId = orderElement.OrderElementId;
                        }
                    }
                }
                _marioPizzaRepository.AddOrder(newOrder);
            }
        }

        public MarioResult AddOrderElement()
        {
            Console.WriteLine("Podaj id zamówienia:");
            int orderId = Convert.ToInt32(Console.ReadLine());
            if (!_marioPizzaRepository.OrderExists(orderId))
            {
                var message = $"Zamówienie o id {orderId} nie istnieje!";
                Console.WriteLine(message);
                return new MarioResult { Success = false, Message = message };
            }
            Console.WriteLine("Podaj id produktu który chcesz dodać:");
            int foodId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj ilość:");
            int amount = Convert.ToInt32(Console.ReadLine());

            _marioPizzaRepository.AddElementToOrder(orderId, foodId, amount);

            return new MarioResult { Success = true };
        }

        public MarioResult AddSubOrderElement()
        {
            Console.WriteLine("Podaj id zamówienia:");
            int orderId = Convert.ToInt32(Console.ReadLine());
            if (!_marioPizzaRepository.OrderExists(orderId))
            {
                var message = $"Zamówienie o id {orderId} nie istnieje!";
                Console.WriteLine(message);
                return new MarioResult { Success = false, Message = message };
            }
            Console.WriteLine("Podaj id elementu zamówienia:");
            int orderElementId = Convert.ToInt32(Console.ReadLine());
            if (!_marioPizzaRepository.OrderElementExists(orderElementId))
            {
                var message = $"Element zamówienia o id {orderElementId} nie istnieje!";
                Console.WriteLine(message);
                return new MarioResult { Success = false, Message = message };
            }
            Console.WriteLine("Podaj id produktu który chcesz dodać:");
            int foodId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj ilość:");
            int amount = Convert.ToInt32(Console.ReadLine());

            _marioPizzaRepository.AddElementToOrder(orderId, foodId, amount);
            Console.WriteLine("Dodano dodatek do elementu zamówienia!");

            return new MarioResult { Success = true };
        }

        public MarioResult ChangeOrderStatus()
        {
            return new MarioResult { Success = true };
        }

        public MarioResult ChangeOrderPriority()
        {
            return new MarioResult { Success = true };
        }

        public MarioResult DeleteOrder()
        {
            Console.WriteLine("Podaj id zamówienia które chcesz usunąć:");
            var orderId = Convert.ToInt32(Console.ReadLine());
            var order = _marioPizzaRepository.GetOrder(orderId);
            if(order == null)
            {
                return new MarioResult { Message = $"Zamówienie o id = {orderId} nie istnieje!", Success = false };
            }
            _marioPizzaRepository.DeleteOrder(orderId);
            return new MarioResult { Success = true };
        }

        public void EditOrder()
        {
            //You can try to modify "Filter" to manage edited values and replace them when not DEFAULT
        }

        private void ShowOrders(List<MarioPizzaOrder> orderList)
        {
            
            List<string> headerElements = new List<string> { "Nr zam.", "Data", "Status", "Priorytet", "Nr tel", "Adres" };
            var header = $"{headerElements[0].PadRight(10)}|" +
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
                Console.WriteLine($"{order.OrderId.ToString().PadRight(10)}|" +
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
        }
        public void GetAllOrders()
        {
            Console.WriteLine("Historia wszystkich zamówień:");
            ShowOrders(_marioPizzaRepository.GetAllOrders());
        }

        public void GetFilteredOrders()
        {
            Console.WriteLine("Wybierz numer filtra by go dostosować. Wpisz -1 by anulować zmiany");
            List<string> filterList = new List<string> { "1. Minimalne Id zamówienia", "2. Maksymalne Id zamówienia",
            "3. Numer klienta zawiera", "4. Adres dostawy zawiera", "5. Priorytet", "6. Status",
            "7. Dolny znacznik czasowy zamówienia", "8. Górny znacznik czasowy zamówienia", "9. WYŚWIETL WYNIKI"};
            int orderIdMin = -1, orderIdMax = -1;
            string clientPhoneNumber = "", deliveryAddress = "";
            OrderPriority priority = OrderPriority.NONE;
            OrderStatus status = OrderStatus.NONE;
            var lowerTimestamp = new DateTime(1970, 1, 1);
            var higherTimestamp = new DateTime(2170, 1, 1);
            var option = Console.ReadLine();
            while(!option.Equals("-1"))
            {
                if (option.Equals("1"))
                {
                    Console.WriteLine("Podaj wartość dla Minimalne Id zamówienia: ");
                    // Not a number exception!!!
                    var input = Convert.ToInt32(Console.ReadLine());
                    if(input > 0){ orderIdMin = input; }
                    else { orderIdMin = -1; }
                }
                else if (option.Equals("2"))
                {
                    Console.WriteLine("Podaj wartość dla Maksymalne Id zamówienia: ");
                    // Not a number exception!!!
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (input > 0) { orderIdMax = input; }
                    else { orderIdMax = -1; }
                }
                else if (option.Equals("3"))
                {
                    Console.WriteLine("Podaj wartość ciąg który znajduje się w Numerze klienta: ");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { clientPhoneNumber = input; }
                    else { clientPhoneNumber = ""; }
                }
                else if (option.Equals("4"))
                {
                    Console.WriteLine("Podaj wartość ciąg który znajduje się w Adresie dostawy: ");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { deliveryAddress = input; }
                    else { deliveryAddress = ""; }
                }
                else if (option.Equals("5"))
                {
                    Console.WriteLine("Wybierz priorytet zamówienia:");
                    Console.WriteLine($"Dostępne wartości: {Enum.GetValues(typeof(OrderPriority)).ToString()}");
                    var input = Console.ReadLine();
                    if (input.Equals("-1")) { priority = OrderPriority.NONE; }
                    else { priority = (OrderPriority)Enum.Parse(typeof(OrderPriority), input); }
                }
                else if (option.Equals("6"))
                {
                    Console.WriteLine("Wybierz status zamówienia:");
                    Console.WriteLine($"Dostępne wartości: {Enum.GetValues(typeof(OrderStatus)).ToString()}");
                    var input = Console.ReadLine();
                    if (input.Equals("-1")) { status = OrderStatus.NONE; }
                    else { status = (OrderStatus)Enum.Parse(typeof(OrderStatus), input); }
                }
                else if (option.Equals("7") || option.Equals("8"))
                {
                    var lowerUpper = option.Equals("7") ? "dolny" : "górny";
                    Console.WriteLine($"Wybierz {lowerUpper} zakres czasu zamówienia. Datę wprowadź w formacie: dzień miesiąc rok godzina minuta sekunda");
                    var input = Console.ReadLine();
                    var someTimestamp = new DateTime();
                    if (input.Equals("-1")) { someTimestamp = new DateTime(1, 1, 1970); }
                    else {
                        var splitedInput = input.Split(' ');
                        if (splitedInput.Length != 3 || splitedInput.Length != 6)
                        {
                            Console.WriteLine($"Niepoprawny format! Twój format: {input}");
                        }
                        else
                        {
                            if(splitedInput.Length == 3)
                            {
                                someTimestamp = new DateTime(Convert.ToInt32(splitedInput[0]),
                                Convert.ToInt32(splitedInput[1]), Convert.ToInt32(splitedInput[2]));
                            }
                            else
                            {
                                someTimestamp = new DateTime(Convert.ToInt32(splitedInput[0]),
                                Convert.ToInt32(splitedInput[1]), Convert.ToInt32(splitedInput[2]), Convert.ToInt32(splitedInput[3]),
                                Convert.ToInt32(splitedInput[4]), Convert.ToInt32(splitedInput[5]));
                            }
                        }
                    }
                    if (option.Equals("7")) { lowerTimestamp = someTimestamp; }
                    else { higherTimestamp = someTimestamp; }
                }
                else if (option.Equals("9"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nie ma takiej opcji!");
                }
                option = Console.ReadLine();
            }
            var filter = _marioPizzaRepository.GetAllOrders().FindAll(x =>
                (orderIdMin == -1 || x.OrderId >= orderIdMin) &&
                (orderIdMax == -1 || x.OrderId <= orderIdMax) &&
                (clientPhoneNumber == "" || x.ClientPhoneNumber.Contains(clientPhoneNumber)) &&
                (deliveryAddress == "" || x.DeliveryAddress.Contains(deliveryAddress)) &&
                (priority == OrderPriority.NONE || x.Priority == priority) &&
                (status == OrderStatus.NONE || x.Status == status) &&
                (lowerTimestamp == new DateTime(1,1,1970) || x.OrderTime >= lowerTimestamp) &&
                (higherTimestamp == new DateTime(1,1,2170) || x.OrderTime <= higherTimestamp)
            );
            Console.WriteLine("Historia wszystkich zamówień:");
            ShowOrders(filter);
        }

        public MarioResult GetOrdersWaiting()
        {
            ShowOrders(_marioPizzaRepository.GetOrdersByStatus(OrderStatus.WAITING));
            return new MarioResult { Success = true };
        }

        public MarioResult GetOrdersInProgress()
        {
            ShowOrders(_marioPizzaRepository.GetOrdersByStatus(OrderStatus.IN_PROGRESS));
            return new MarioResult { Success = true };
        }

        public MarioResult GetOrdersReadyForDelivery()
        {
            ShowOrders(_marioPizzaRepository.GetOrdersByStatus(OrderStatus.DELIVERY));
            return new MarioResult { Success = true };
        }

        public MarioResult GetOrdersDone()
        {
            ShowOrders(_marioPizzaRepository.GetOrdersByStatus(OrderStatus.DISPOSED));
            return new MarioResult { Success = true };
        }

        public MarioResult MoveToNextStatus()
        {
            Console.WriteLine("Podaj id zamówienia dla którego chcesz zmienić status:");
            var orderId = Convert.ToInt32(Console.ReadLine());
            var order = _marioPizzaRepository.GetOrder(orderId);
            if (order == null)
            {
                return new MarioResult { Message = $"Nie znaleziono zamówienia o id {orderId}", Success = false };
            }
            OrderStatus currentStatus = order.Status;
            OrderStatus newStatus = (OrderStatus) ((int)currentStatus + 1);
            if (currentStatus != OrderStatus.DONE)
            {
                _marioPizzaRepository.ChangeOrderStatus(orderId, newStatus);
            }
            Console.WriteLine($"Nowy status zamówienia: {Enum.GetName(typeof(OrderStatus), newStatus)}");
            return new MarioResult { Success = true };
        }

        private void ShowOrderElements(List<OrderElement> orderElements)
        {
            Console.WriteLine("Elementy zamówienia:");
            string orderSpace = " * ";
            string subOrderSpace = "    * ";
            foreach(var orderElement in orderElements)
            {
                string foodName = _marioPizzaRepository.GetFoodNameById(orderElement.FoodId);
                Console.WriteLine($"{orderSpace}{foodName} (x{orderElement.Amount})");

                var subOrderElements = _marioPizzaRepository.GetSubOrderElements(orderElement.OrderId, orderElement.OrderElementId);
                if(subOrderElements != null)
                {
                    foreach(var subOrderElement in subOrderElements)
                    {
                        foodName = _marioPizzaRepository.GetFoodNameById(subOrderElement.FoodId);
                        Console.WriteLine($"{subOrderSpace}{foodName} (x{subOrderElement.Amount})");
                    }
                }
            }
        }

        private void ShowSubOrderElements(List<SubOrderElement> subOrderElements)
        {
            Console.WriteLine("Elementy dodatkowe dla elementu zamówienia:");
            string subOrderSpace = " * ";
            foreach (var subOrderElement in subOrderElements)
            {
                string foodName = _marioPizzaRepository.GetFoodNameById(subOrderElement.FoodId);
                Console.WriteLine($"{subOrderSpace}{foodName} (x{subOrderElement.Amount})");
            }
        }

        public MarioResult GetOrder()
        {
            Console.Write("Podaj id zamówienia: ");
            var orderId = Convert.ToInt32(Console.ReadLine());
            if (!_marioPizzaRepository.OrderExists(orderId))
            {
                var message = $"Zamówienie o id {orderId} nie istnieje!";
                Console.WriteLine(message);
                return new MarioResult { Success = false, Message = message};
            }
            var selectedOrder = _marioPizzaRepository.GetOrder(orderId);
            Console.WriteLine($"=== Informacje dla zamówienia id = {orderId} ===");
            Console.WriteLine($"Data złożenia: {selectedOrder.OrderTime.ToString("dd/MM/yyyy HH:MM:ss")}");
            Console.WriteLine($"Adres dostawy: {selectedOrder.DeliveryAddress}");
            Console.WriteLine($"Numer tel. klienta: {selectedOrder.ClientPhoneNumber}");
            Console.WriteLine($"Priorytet: {selectedOrder.Priority}");
            Console.WriteLine($"Status: {selectedOrder.Status}");
            //And SubOrder Elements
            ShowOrderElements(_marioPizzaRepository.GetOrderElements(orderId));
            return new MarioResult { Success = true };
        }
    }
}
