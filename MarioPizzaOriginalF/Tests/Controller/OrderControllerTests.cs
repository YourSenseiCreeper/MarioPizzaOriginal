using System;
using System.IO;
using NUnit.Framework;
using Moq;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.DataAccess;

namespace MarioPizzaOriginal.Controller.Tests
{
    public class OrderControllerTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IFoodRepository> _mockFoodRepository;
        private Mock<IOrderElementRepository> _mockOrderElementRepository;
        private Mock<IOrderSubElementRepository> _mockOrderSubElementRepository;
        private OrderController _orderController;
        private Order _sampleOrder;
        private List<OrderElement> _orderElements;
        private List<Food> _testFood;

        public OrderControllerTests()
        {
            var factory = new MockRepository(MockBehavior.Strict);
            _mockOrderRepository = factory.Create<IOrderRepository>();
            _mockFoodRepository = factory.Create<IFoodRepository>();
            _mockOrderElementRepository = factory.Create<IOrderElementRepository>();
            _mockOrderSubElementRepository = factory.Create<IOrderSubElementRepository>();
            
            _orderController = new OrderController(null);
            _sampleOrder = new Order
            {
                OrderId = 0,
                ClientPhoneNumber = "123",
                DeliveryAddress = "Test",
                Status = OrderStatus.WAITING
            };
            _orderElements = new List<OrderElement>() {
                new OrderElement { OrderId = _sampleOrder.OrderId, OrderElementId = 0, FoodId = 0, Amount = 1 },
                new OrderElement { OrderId = _sampleOrder.OrderId, OrderElementId = 1, FoodId = 1, Amount = 2 }
            };
            _testFood = new List<Food>() {
                new Food { FoodId = 0, FoodName = "Testowy kebab", Price = 15 },
                new Food { FoodId = 1, FoodName = "Testowa tortilla", Price = 12 }
            };
        } 

        [Test]
        public void GetOrderNullOrderTest()
        {
            // Arrange
            Order nullOrder = null;
            _mockOrderRepository.Setup(t => t.Get(_sampleOrder.OrderId)).Returns(nullOrder);
            using (StringWriter sw = new StringWriter())
            {
                using (StringReader sr = new StringReader("0"))
                {
                    Console.SetOut(sw);
                    Console.SetIn(sr);
                    // Act
                    _orderController.GetOrder();
                    //Assert
                    string result = sw.ToString();
                    // Close the stream to see the results of the test
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                    Assert.IsTrue(result.Contains("Zamówienie o id 0 nie istnieje!"));

                    _mockFoodRepository.Verify();
                    _mockOrderRepository.Verify();
                    _mockOrderElementRepository.Verify();
                    _mockOrderSubElementRepository.Verify();
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
            _mockOrderRepository.Setup(t => t.Get(_sampleOrder.OrderId)).Returns(_sampleOrder);
            _mockOrderRepository.Setup(t => t.CalculatePriceForOrder(_sampleOrder.OrderId)).Returns(10);
            // Program checks if the order element has its sub elements 
            _mockOrderElementRepository.Setup(t => t.GetElements(_sampleOrder.OrderId)).Returns(_orderElements);
            _mockOrderSubElementRepository.Setup(t => t.GetSubElements(0)).Returns(new List<OrderSubElement>());
            _mockOrderSubElementRepository.Setup(t => t.GetSubElements(1)).Returns(new List<OrderSubElement>());
            // Food names are taken from FoodRepository to show the ordered food and the price * amount
            _testFood.ForEach(food =>
            {
                _mockFoodRepository.Setup(t => t.GetName(food.FoodId)).Returns(food.FoodName);
                _mockFoodRepository.Setup(t => t.Get(food.FoodId)).Returns(food);
            });
            
            using (StringWriter sw = new StringWriter())
            {
                using (StringReader sr = new StringReader("0"))
                {
                    Console.SetOut(sw);
                    Console.SetIn(sr);

                    // Act
                    _orderController.GetOrder();
                    
                    //Assert
                    string result = sw.ToString();
                    // Close the stream to see the results of the test
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));

                    Assert.IsTrue(result.Contains(_sampleOrder.ClientPhoneNumber));
                    Assert.IsTrue(result.Contains(_sampleOrder.DeliveryAddress));

                    _testFood.ForEach(food => Assert.IsTrue(result.Contains(food.FoodName)));

                    _mockFoodRepository.Verify();
                    _mockOrderRepository.Verify();
                    _mockOrderElementRepository.Verify();
                    _mockOrderSubElementRepository.Verify();
                }
            }
        }
    }
}