using Model.DataAccess;
using Moq;
using NUnit.Framework;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal;
using MarioPizzaOriginal.Controller;
//using MarioPizzaOriginal.Domain;

namespace DomainTest
{
    public class Tests
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
            var mockController = new IngredientController(mockIngredientRepository.Object);

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