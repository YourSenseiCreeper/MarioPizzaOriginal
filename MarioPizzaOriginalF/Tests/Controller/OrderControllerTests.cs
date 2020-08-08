using MarioPizzaOriginal.Controller;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Model.DataAccess;
using NUnit.Framework;
using Moq;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Controller.Tests
{
    public class OrderControllerTests
    {
        private Mock<IOrderRepository> mockOrderRepository;
        private Mock<IFoodRepository> mockFoodRepository;
        private Mock<IOrderElementRepository> mockOrderElementRepository;
        private Mock<IOrderSubElementRepository> mockOrderSubElementRepository;
        private OrderController orderController;
        private Order sampleOrder;
        private List<OrderElement> orderElements;
        private List<Food> testFood;

        [SetUp]
        public void OrderControllerSetup()
        {
            var factory = new MockRepository(MockBehavior.Strict);
            mockOrderRepository = factory.Create<IOrderRepository>();
            mockFoodRepository = factory.Create<IFoodRepository>();
            mockOrderElementRepository = factory.Create<IOrderElementRepository>();
            mockOrderSubElementRepository = factory.Create<IOrderSubElementRepository>();
            
            orderController = new OrderController(null);
            sampleOrder = new Order
            {
                OrderId = 0,
                ClientPhoneNumber = "123",
                DeliveryAddress = "Test",
                Status = OrderStatus.WAITING
            };
            orderElements = new List<OrderElement>() {
                new OrderElement { OrderId = sampleOrder.OrderId, OrderElementId = 0, FoodId = 0, Amount = 1 },
                new OrderElement { OrderId = sampleOrder.OrderId, OrderElementId = 1, FoodId = 1, Amount = 2 }
            };
            testFood = new List<Food>() {
                new Food { FoodId = 0, FoodName = "Testowy kebab", Price = 15 },
                new Food { FoodId = 1, FoodName = "Testowa tortilla", Price = 12 }
            };
        }

        [Test]
        public void GetOrderNullOrderTest()
        {
            // Arrange
            Order nullOrder = null;
            mockOrderRepository.Setup(t => t.Get(sampleOrder.OrderId)).Returns(nullOrder);
            using (StringWriter sw = new StringWriter())
            {
                using (StringReader sr = new StringReader("0"))
                {
                    Console.SetOut(sw);
                    Console.SetIn(sr);
                    // Act
                    orderController.GetOrder();
                    //Assert
                    string result = sw.ToString();
                    // Close the stream to see the results of the test
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                    Assert.IsTrue(result.Contains("Zamówienie o id 0 nie istnieje!"));

                    mockFoodRepository.Verify();
                    mockOrderRepository.Verify();
                    mockOrderElementRepository.Verify();
                    mockOrderSubElementRepository.Verify();
                }
            }
        }

        /// <summary>
        /// User inputs valid data, everything is okey
        /// </summary>
        [Test]
        public void GetOrderTest()
        {
            // Arrange
            mockOrderRepository.Setup(t => t.Get(sampleOrder.OrderId)).Returns(sampleOrder);
            mockOrderRepository.Setup(t => t.CalculatePriceForOrder(sampleOrder.OrderId)).Returns(10);
            // Program checks if the order element has its sub elements 
            mockOrderElementRepository.Setup(t => t.GetElements(sampleOrder.OrderId)).Returns(orderElements);
            mockOrderSubElementRepository.Setup(t => t.GetSubElements(0)).Returns(new List<OrderSubElement>());
            mockOrderSubElementRepository.Setup(t => t.GetSubElements(1)).Returns(new List<OrderSubElement>());
            // Food names are taken from FoodRepository to show the ordered food and the price * amount
            testFood.ForEach(food =>
            {
                mockFoodRepository.Setup(t => t.GetName(food.FoodId)).Returns(food.FoodName);
                mockFoodRepository.Setup(t => t.Get(food.FoodId)).Returns(food);
            });
            
            using (StringWriter sw = new StringWriter())
            {
                using (StringReader sr = new StringReader("0"))
                {
                    Console.SetOut(sw);
                    Console.SetIn(sr);

                    // Act
                    orderController.GetOrder();
                    
                    //Assert
                    string result = sw.ToString();
                    // Close the stream to see the results of the test
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));

                    Assert.IsTrue(result.Contains(sampleOrder.ClientPhoneNumber));
                    Assert.IsTrue(result.Contains(sampleOrder.DeliveryAddress));

                    testFood.ForEach(food => Assert.IsTrue(result.Contains(food.FoodName)));

                    mockFoodRepository.Verify();
                    mockOrderRepository.Verify();
                    mockOrderElementRepository.Verify();
                    mockOrderSubElementRepository.Verify();
                }
            }
        }
    }
}