using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Tools;
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
        private readonly ViewHelper _viewHelper;
        private readonly IConsole _console;

        public OrderController()
        {
            var container = TinyIoCContainer.Current;
            _console = container.Resolve<IConsole>();
            _viewHelper = new ViewHelper(_console);
            _orderFilter = new OrderFilter(_console);
            _foodRepository = container.Resolve<IFoodRepository>();
            _orderRepository = container.Resolve<IOrderRepository>();
            _orderElementRepository = container.Resolve<IOrderElementRepository>();
            _orderSubElementRepository = container.Resolve<IOrderSubElementRepository>();
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
            var maxStep = 8;

            newOrder.ClientPhoneNumber = _viewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Numer telefonu klienta: ");
            if (newOrder.ClientPhoneNumber.Length != 9)
                _viewHelper.WriteAndWait("UWAGA! Numer klienta nie ma 9 znaków!");

            newOrder.DeliveryAddress = _viewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Adres dostawy: ");
            newOrder.Priority = _viewHelper.AskForOption<OrderPriority>(
                "Dostępne priorytety:", $"(krok {step++} z {maxStep}) Priorytet zamówienia (domyślnie NORMAL): ", "NORMAL");

            OrderElement tempOrderElement;
            _console.Clear();
            _console.WriteLine($"(krok {step++} z {maxStep}) Wybierz elementy zamówienia:");

            new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action> {
                {"Dodaj element do zamówienia", () =>
                {
                    tempOrderElement = new OrderElement
                    {
                        FoodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                        Amount = _viewHelper.AskForInt("Podaj ilość: "),
                        SubOrderElements = new List<OrderSubElement>()
                    };
                    if (_viewHelper.AskForYesNo("Czy chcesz dodać dodatki do tego elementu?"))
                    {
                        new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action>
                        {
                            {"Dodaj dodatek do elementu zamówienia", () =>
                            {
                                tempOrderElement.SubOrderElements.Add(new OrderSubElement
                                {
                                    FoodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                                    Amount = _viewHelper.AskForInt("Podaj ilość: ")
                                });
                            }}
                        }).PresentRightless();
                    }
                    newOrder.OrderElements.Add(tempOrderElement);
                }
            }}).PresentRightless();

            newOrder.Payment = _viewHelper.AskForOption<Payment>("Dostępne opcje: ", $"({step++} z {maxStep}) Wybierz gdzie chcesz zapłacić: ");
            newOrder.PaymentMethod = _viewHelper.AskForOption<PaymentMethod>("Dostępne opcje: ", $"({step++} z {maxStep}) Wybierz w jaki sposób chcesz zapłacić: ");
            newOrder.Comments = _viewHelper.AskForString($"(krok {step++} z {maxStep}) Uwagi do zamówienia: ");

            if (_viewHelper.AskForYesNo($"({step} z {maxStep}) Czy chcesz dodać to zamówienie?"))
                _orderRepository.Add(newOrder);
        }

        public void AddOrderElement()
        {
            if (OrderNotExists("Podaj id zamówienia: ", out var orderId))
                return;

            var foodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: ");
            if (!_foodRepository.Exists(foodId))
            {
                _viewHelper.WriteAndWait($"Produkt o id {foodId} nie istnieje!");
                return;
            }
            
            var amount = _viewHelper.AskForInt("Podaj ilość: ");
            var order = _orderRepository.Get(o => o.OrderId == orderId, true);
            order.OrderElements.Add(new OrderElement
            {
                FoodId = foodId,
                Amount = amount
            });
            _orderRepository.Save(order);
            var foodName = _foodRepository.GetName(foodId);
            _viewHelper.WriteAndWait($"Dodano {foodName} x {amount} do zamówienia numer {orderId}");
        }

        public void AddSubOrderElement()
        {
            if (OrderNotExists("Podaj id zamówienia: ", out var orderId))
                return;

            int orderElementId = _viewHelper.AskForInt("Podaj id elementu zamówienia: ");
            if (!_orderElementRepository.Exists(orderElementId))
            {
                _viewHelper.WriteAndWait($"Element zamówienia o id {orderElementId} nie istnieje!");
                return;
            }
            var foodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: ");
            var amount = _viewHelper.AskForInt("Podaj ilość: ");

            _orderElementRepository.AddToOrder(orderId, foodId, amount);
            _viewHelper.WriteAndWait($"Dodano dodatek do elementu zamówienia nr {orderId}!");
        }

        public void ChangeOrderStatus()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz zmienić status: ", out var orderId))
                return;
            var order = _orderRepository.Get(orderId);
            _console.WriteLine($"Obecny status zamówienia: {order.Status}");
            OrderStatus newStatus = _viewHelper.AskForOption<OrderStatus>("Dostępne statusy", "Nowy status dla zamówienia: ", order.Status.ToString());
            order.Status = newStatus;
            _orderRepository.Save(order);

            _viewHelper.WriteAndWait($"Nowy status dla zamówienia: {newStatus}");
        }

        public void ChangeOrderPriority()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz zmienić priorytet: ", out var orderId))
                return;
            var order = _orderRepository.Get(orderId);

            _console.WriteLine($"Obecny priorytet zamówienia: {order.Priority}");
            OrderPriority newPriority = _viewHelper.AskForOption<OrderPriority>("Dostępne priorytety", "Nowy priorytet dla zamówienia: ", order.Priority.ToString());
            order.Priority = newPriority;
            _orderRepository.Save(order);

            _viewHelper.WriteAndWait($"Nowy priorytet dla zamówienia: {newPriority}");
        }

        public void DeleteOrder()
        {
            if (OrderNotExists("Podaj id zamówienia które chcesz usunąć: ", out var orderId))
                return;

            _orderRepository.DeleteOrderWithAllElements(orderId);
            _viewHelper.WriteAndWait($"Usunięto zamówienie nr {orderId} wraz z elementami i podelementami");
        }

        public void EditOrder()
        {
            //You can try to modify "Filter" to manage edited values and replace them when not DEFAULT
            if (OrderNotExists("Podaj id zamówienia do edycji: ", out var orderId))
                return;

            var order = _orderRepository.GetOrderWithAllElements(orderId);
            _console.WriteLine("Jeżeli nic nie chcesz zmienić, kliknij ENTER");
            order.ClientPhoneNumber = _viewHelper.EditableString("Numer klienta: ", order.ClientPhoneNumber);
            _console.Clear();
            order.DeliveryAddress = _viewHelper.EditableString("Adres dostawy: ", order.DeliveryAddress);
            _console.Clear();
            order.Priority = _viewHelper.EditableValue("Dostępne priorytety: ", "Wybierz priorytet: ", 
                order.Priority);
            _console.Clear();
            order.Status = _viewHelper.EditableValue("Dostępne statusy: ", "Wybierz status: ", order.Status);
            _console.Clear();
            order.Payment = _viewHelper.EditableValue("Dostępne miejsca płatności: ", "Wybierz miejsce płatności: ",
                order.Payment);
            _console.Clear();
            order.PaymentMethod = _viewHelper.EditableValue("Dostępne metody płatności: ", "Wybierz metodę płatności: ",
                order.PaymentMethod);

            _console.Clear();
            new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action> {
            {"Dodaj element do zamówienia", () =>
                {
                    var tempOrderElement = new OrderElement
                    {
                        FoodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                        Amount = _viewHelper.AskForInt("Podaj ilość: "),
                        SubOrderElements = new List<OrderSubElement>()
                    };
                    if (_viewHelper.AskForYesNo("Czy chcesz dodać dodatki do tego elementu?"))
                    {
                        new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action>
                        {
                            {"Dodaj dodatek do elementu zamówienia", () =>
                            {
                                tempOrderElement.SubOrderElements.Add(new OrderSubElement
                                {
                                    FoodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                                    Amount = _viewHelper.AskForInt("Podaj ilość: ")
                                });
                            }}
                        }).PresentRightless();
                    }
                    order.OrderElements.Add(tempOrderElement);
                }
            },
            {"Edytuj element do zamówienia", () =>
                {
                    // pobieranie tego produktu
                    // jeżeli błędne id to powrót do głównego menu
                    // dać to w osobną metodę
                    var orderElementNumber = _viewHelper.AskForInt("Podaj numer elementu zamówienia: ");
                    if (orderElementNumber > order.OrderElements.Count || orderElementNumber < 1)
                    {
                        _viewHelper.WriteAndWait("Numer jest większy niż ilość elementów!");
                        return;
                    }
                    // zakładamy, że użytkownik wpisuje numery o 1 większe od indeksu
                    var orderElement = order.OrderElements[orderElementNumber - 1];
                    orderElement.FoodId = _viewHelper.EditableInt($"Podaj id produktu ({orderElement.Food.FoodName}): ",
                        orderElement.FoodId);
                    orderElement.Amount = _viewHelper.EditableInt("Podaj ilość: ", (int) orderElement.Amount); //TODO: unsafe casting
                    if (_viewHelper.AskForYesNo("Czy chcesz edytować dodatki tego elementu?"))
                    {
                        new MenuCreator("Możliwe opcje:", "Zakończ dodawanie", new Dictionary<string, Action>
                        {
                            {"Dodaj dodatek do elementu zamówienia", () =>
                            {
                                orderElement.SubOrderElements.Add(new OrderSubElement
                                {
                                    FoodId = _viewHelper.AskForInt("Podaj id produktu który chcesz dodać: "),
                                    Amount = _viewHelper.AskForInt("Podaj ilość: ")
                                });
                            }}
                        }).PresentRightless();
                    }
                }
            },
            {"Usuń element zamówienia", () =>
                {
                    var orderElementNumber = _viewHelper.AskForInt("Podaj numer elementu zamówienia: ");
                    if (orderElementNumber > order.OrderElements.Count || orderElementNumber < 1)
                    {
                        _viewHelper.WriteAndWait("Numer jest większy niż ilość elementów!");
                        return;
                    }
                    // zakładamy, że użytkownik wpisuje numery o 1 większe od indeksu
                    var orderElement = order.OrderElements[orderElementNumber - 1];
                    order.OrderElements.Remove(orderElement);
                }
            }
            }).PresentRightless();
            _orderRepository.Save(order);
            _viewHelper.WriteAndWait("Zapisano zamówienie nr "+orderId);
        }

        public void CalculatePriceForOrder()
        {
            if (OrderNotExists("Podaj id zamówienia dla którego chcesz sprawdzić cenę: ", out var orderId))
                return;
            var price = _orderRepository.CalculatePriceForOrder(orderId);
            _viewHelper.WriteAndWait($"Koszt zamówienia nr {orderId} wynosi {price} zł");
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
                _viewHelper.WriteAndWait("Obecny status zamówienia to: DONE, nie możesz zmienić statusu!");
                return;
            }
            _orderRepository.Save(order);
            _viewHelper.WriteAndWait($"Nowy status zamówienia: {order.Status}");
        }

        public void GetOrder()
        {
            if (OrderNotExists("Podaj id zamówienia: ", out var orderId))
                return;

            var selectedOrder = _orderRepository.GetOrderWithAllElements(orderId);
            _console.Clear();
            _console.WriteLine($"=== Informacje dla zamówienia id = {orderId} ===");
            _console.WriteLine($"{"Data złożenia:",-20} {selectedOrder.OrderTime:dd/MM/yyyy HH:MM:ss}");
            _console.WriteLine($"{"Adres dostawy:",-20} {selectedOrder.DeliveryAddress}");
            _console.WriteLine($"{"Numer tel. klienta:",-20} {selectedOrder.ClientPhoneNumber}");
            _console.WriteLine($"{"Priorytet:",-20} {selectedOrder.Priority}");
            _console.WriteLine($"{"Status:",-20} {selectedOrder.Status}");
            _console.WriteLine($"{"Uwagi:",-20} {selectedOrder.Comments}");
            var price = 0d;
            if (selectedOrder.OrderElements != null)
            {
                price = CalculateOrderPrice(selectedOrder.OrderElements);
                ShowOrderElements(selectedOrder.OrderElements);
            }
            else _console.WriteLine("Brak elementów zamówienia!");
            _viewHelper.WriteAndWait($"{"Cena:",-20} {price} zł");
        }

        public void ShowAllSubOrderElements()
        {
            var subOrderElements = _orderSubElementRepository.GetAll();
            foreach (var subOrderElement in subOrderElements)
            {
                _console.WriteLine($"{subOrderElement.OrderSubElementId}, {subOrderElement.OrderElementId}, {subOrderElement.FoodId}, {subOrderElement.Amount}");
            }
            _viewHelper.WriteAndWait($"{subOrderElements.Count} wyników");
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
            var result = _viewHelper.CheckIfElementNotExists(_orderRepository, 
                message, "Zamówienie o id {0} nie istnieje!", out var elmElementId);
            orderId = elmElementId;
            return result;
        }

        private void ShowOrders(List<Order> orderList)
        {
            _console.Clear();
            var header = $"{"Nr zam.",7}|{"Data",20}|{"Status",12}|{"Priorytet",10}|{"Nr tel",10}|{"Adres",15}";
            _console.WriteLine(header);
            _console.WriteLine(new string('=', header.Length));
            orderList.ForEach(order => _console.WriteLine($"{order.OrderId,7}|{order.OrderTime,20}|{_viewHelper.StatusColor(order.Status),12}|" +
                                                          $"{order.Priority,10}|{order.ClientPhoneNumber,10}|{order.DeliveryAddress,15}"));
            _viewHelper.WriteAndWait($"Znaleziono {orderList.Count} pasujących wyników");
        }

        private void ShowOrderElements(IEnumerable<OrderElement> orderElements)
        {
            _console.WriteLine("Elementy zamówienia:");
            foreach (var orderElement in orderElements)
            {
                _console.WriteLine($"* {orderElement.Food.FoodName} x {orderElement.Amount} = {orderElement.Food.Price * orderElement.Amount} zł");
                if (orderElement.SubOrderElements == null)
                    continue;

                foreach (var subOrderElement in orderElement.SubOrderElements)
                {
                    _console.WriteLine($"\t* {subOrderElement.Food.FoodName} x{subOrderElement.Amount} = " +
                                       $"{subOrderElement.Food.Price * subOrderElement.Amount} zł");
                }
            }
        }
    }
}
