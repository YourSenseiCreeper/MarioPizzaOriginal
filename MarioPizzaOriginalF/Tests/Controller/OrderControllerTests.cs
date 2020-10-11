using System;
using System.IO;
using NUnit.Framework;
using Moq;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Filter;
using MarioPizzaOriginal.Tests.Controller;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal.Controller.Tests
{
    public class OrderControllerTests
    {
        public OrderControllerTests()
        {
            // var factory = new MockRepository(MockBehavior.Strict);
            // _mockOrderRepository = factory.Create<IOrderRepository>();
            // _mockFoodRepository = factory.Create<IFoodRepository>();
            // _mockOrderElementRepository = factory.Create<IOrderElementRepository>();
            // _mockOrderSubElementRepository = factory.Create<IOrderSubElementRepository>();
            //
            // _orderController = new OrderController();
            // _sampleOrder = new Order
            // {
            //     OrderId = 0,
            //     ClientPhoneNumber = "123",
            //     DeliveryAddress = "Test",
            //     Status = OrderStatus.WAITING
            // };
            // _orderElements = new List<OrderElement>() {
            //     new OrderElement { OrderId = _sampleOrder.OrderId, OrderElementId = 0, FoodId = 0, Amount = 1 },
            //     new OrderElement { OrderId = _sampleOrder.OrderId, OrderElementId = 1, FoodId = 1, Amount = 2 }
            // };
            // _testFood = new List<Food>() {
            //     new Food { FoodId = 0, FoodName = "Testowy kebab", Price = 15 },
            //     new Food { FoodId = 1, FoodName = "Testowa tortilla", Price = 12 }
            // };
        } 

        [Test]
        public void GetOrderNullOrderTest()
        {
            // Arrange
            // Order nullOrder = null;
            // _mockOrderRepository.Setup(t => t.Get(_sampleOrder.OrderId)).Returns(nullOrder);
            // using (StringWriter sw = new StringWriter())
            // {
            //     using (StringReader sr = new StringReader("0"))
            //     {
            //         Console.SetOut(sw);
            //         Console.SetIn(sr);
            //         // Act
            //         _orderController.GetOrder();
            //         //Assert
            //         string result = sw.ToString();
            //         // Close the stream to see the results of the test
            //         Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            //         Assert.IsTrue(result.Contains("Zamówienie o id 0 nie istnieje!"));
            //
            //         _mockFoodRepository.Verify();
            //         _mockOrderRepository.Verify();
            //         _mockOrderElementRepository.Verify();
            //         _mockOrderSubElementRepository.Verify();
            //     }
            // }
        }

        /// <summary>
        /// User inputs valid data, everything is okey
        /// </summary>
        [Test]
        public void GetOrderTest()
        {
            var orderElements = new List<OrderElement>
            {
                new OrderElement
                {
                    OrderElementId = 1, 
                    Amount = 1, 
                    Food = new Food
                    {
                        FoodName = "Guacamole", 
                        Price = 10, 
                    }
                }
            };
            var sampleOrder = new Order
            {
                OrderId = 1,
                ClientPhoneNumber = "123",
                DeliveryAddress = "ul. Sezamkowa 1",
                OrderTime = DateTime.Now,
                Payment = Payment.AT_PLACE,
                Priority = OrderPriority.HIGH,
                Status = OrderStatus.IN_PROGRESS,
                OrderElements = orderElements
            };

            RunControllerTest(new[] {"1", ""},
                mock =>
                {
                    mock.OrderRepository.Setup(t => t.Exists(sampleOrder.OrderId)).Returns(true);
                    mock.OrderRepository.Setup(t => t.GetOrderWithAllElements(sampleOrder.OrderId)).Returns(sampleOrder);
                },
                act => act.Controller.GetOrder(),
                assert =>
                {
                    // sprawdzanie output konsoli
                    Assert.That(assert.Console.Output.Any(line => line.Contains("id = 1")));
                    Assert.That(assert.Console.Output.Any(line => line.Replace(" ", "").Contains("Cena:10zł")));
                }
                );
        }


        private void RunControllerTest(IEnumerable<string> consoleInput,
            Action<TestContext> mock,
            Action<TestContext> act,
            Action<TestContext> assert)
        {
            var console = new FakeConsole(consoleInput);
            var container = TinyIoCContainer.Current;
            container.Register<IConsole>(console);
            container.Register(new ViewHelper(console));
            container.Register(new FilterHelper(console));
            var mockRepository = new MockRepository(MockBehavior.Strict);
            
            var testContext = new TestContext
            {
                Console = console,
                BaseRepository = mockRepository.Create<IRepository<Order>>(MockBehavior.Strict),
                OrderRepository = mockRepository.Create<IOrderRepository>(MockBehavior.Strict),
                OrderElementRepository = mockRepository.Create<IOrderElementRepository>(MockBehavior.Strict),
                OrderSubElementRepository = mockRepository.Create<IOrderSubElementRepository>(MockBehavior.Strict),
                FoodRepository = mockRepository.Create<IFoodRepository>(MockBehavior.Strict)
            };

            mock(testContext);
            container.Register(testContext.BaseRepository.Object);
            container.Register(testContext.OrderRepository.Object);
            container.Register(testContext.OrderElementRepository.Object);
            container.Register(testContext.OrderSubElementRepository.Object);
            container.Register(testContext.FoodRepository.Object);

            testContext.Controller = new OrderController();
            act(testContext);
            assert(testContext);
            mockRepository.VerifyAll();
            TinyIoCContainer.Current.Dispose();
        }

        internal class TestContext
        {
            public FakeConsole Console { get; set; }
            public OrderController Controller { get; set; }
            public Mock<IOrderRepository> OrderRepository { get; set; }
            public Mock<IFoodRepository> FoodRepository { get; set; }
            public Mock<IOrderElementRepository> OrderElementRepository { get; set; }
            public Mock<IOrderSubElementRepository> OrderSubElementRepository { get; set; }
            public Mock<IRepository<Order>> BaseRepository { get; set; }
        }
    }
}