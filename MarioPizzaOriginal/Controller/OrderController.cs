using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Model;
using MarioPizzaOriginal.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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

            //First i need to have a possibility to add a FoodSizeSauce (pick from a list) to my Dictionary
            //OrderList (required!)
            //ClientPhoneNumber (required!)
            //DeliveryAddress (required!)
            //OrderId (default)
            //OrderTime (default)
            //Status (default Waiting)
            //Priority (optional)
        }

        public void DeleteOrder()
        {
            //By orderId
        }

        public void EditOrder()
        {
            //
        }

        private void ShowOrders(List<MarioPizzaOrder> orderList)
        {
            List<string> headerElements = new List<string> { "Numer zamówienia", "Data i czas", "Status", "Priorytet", "Numer tel. klienta", "Adres dostawy" };
            headerElements.ForEach(x => Console.Write(x.PadLeft(20)));
            Console.Write("\n");
            foreach (var order in orderList)
            {
                Console.WriteLine($"{order.OrderId.ToString().PadLeft(5)} |" +
                    $" {order.OrderTime.ToString().PadLeft(20)} |" +
                    $" {order.Status.ToString().PadLeft(20)} |" +
                    $" {order.Priority.ToString().PadLeft(20)} |" +
                    $" {order.ClientPhoneNumber.PadLeft(20)} |" +
                    $" {order.DeliveryAddress.PadLeft(20)}");
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
                    Console.WriteLine("Wybierz dolny zakres czasu zamówienia. Datę wprowadź w formacie: dzień miesiąc rok godzina minuta sekunda");
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
                (orderIdMin != -1 && x.OrderId >= orderIdMin) &&
                (orderIdMax != -1 && x.OrderId <= orderIdMax) &&
                (clientPhoneNumber != "" && x.ClientPhoneNumber.Contains(clientPhoneNumber)) &&
                (deliveryAddress != "" && x.DeliveryAddress.Contains(deliveryAddress)) &&
                (priority != OrderPriority.NONE && x.Priority == priority) &&
                (status != OrderStatus.NONE && x.Status == status) &&
                (lowerTimestamp != new DateTime(1,1,1970) && x.OrderTime >= lowerTimestamp) &&
                (higherTimestamp != new DateTime(1,1,2170) && x.OrderTime <= higherTimestamp)
            );
            Console.WriteLine("Historia wszystkich zamówień:");
            ShowOrders(filter);

        }
    }
}
