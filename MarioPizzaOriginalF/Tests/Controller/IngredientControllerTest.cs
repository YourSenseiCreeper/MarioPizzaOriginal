using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using Moq;
using NUnit.Framework;
using TinyIoC;

namespace MarioPizzaOriginal.Tests.Domain
{
    public class IngredientControllerTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void AddIngredientIngredientControllerTest()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var mockIngredientRepository = factory.Create<IIngredientRepository>();
            mockIngredientRepository.Setup(t => t);
            var mockController = new IngredientController();
        }

        [Test]
        public void AddIngredientToDatabaseTest()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var mockIngredientRepository = factory.Create<IIngredientRepository>();
            //IIngredientRepository repository = mockIngredientRepository.Object;
            Ingredient ingredient = new Ingredient { IngredientId = 0, IngredientName = "Shit", UnitOfMeasureType = UnitOfMeasure.KILOGRAM };
            mockIngredientRepository.Setup(t => t.Add(ingredient));
            IIngredientRepository repository = mockIngredientRepository.Object;

            Assert.AreEqual(ingredient, repository.Get(0));
            // A A A
            // Arrange
            // Mock<IIngredientRepository> ingredientRepository = new Mock<IIngredientRepository>();
            // Act
            // Assert

            // CustomerViewModel viewModel = new CustomerViewModel(ingredientRepository.Object, mockContainer.Object);
            // viewModel.CustomersRepository = new CustomersRepository();
            // viewModel.Customer = new Mock<Customer>().Object;

            // Act
            // viewModel.Save();

            // Assert

            // Arrange
            //string input = "Mateusz#Drozdowski";
            //string result = "mateuszrozowski";

            // Act
            //input = input.ToLower().Replace("#", "").Replace("d", "");

            //
            //_mockCustomerRepository.Setup(t => t.GetId()).Returns(() => i).Callback(() => i++);
        }
    }
}
