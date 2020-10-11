using System.Linq;
using MarioPizzaOriginal.Controller;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using Moq;
using NUnit.Framework;

namespace MarioPizzaOriginal.Tests.Controller
{
    [TestFixture]
    public class IngredientControllerTest : 
        ControllerTestBase<IngredientController, IIngredientRepository, Ingredient>
    {
        // public IngredientControllerTest()
        // {
        //     var container = TinyIoCContainer.Current;
        //     container.Register(new Mock<IIngredientRepository>());
        //     container.Register(Mock.Of<IRepository<Ingredient>>());
        //     var fakeConsole = Mock.Of<IConsole>();
        //     container.Register(fakeConsole);
        //     container.Register(new ViewHelper(fakeConsole)).AsSingleton();
        //     container.Register(new FilterHelper(fakeConsole)).AsSingleton();
        // }

        // TEMPLATE
        // //prepare
        //
        // //mock, act, assert
        // RunControllerTest(new[] { "test", UnitOfMeasure.KILOGRAM.ToString(), "-1", "-1", "-1" },
        //     repo =>
        //     {
        //         repo.Setup(r => r.Get(5)).Returns(new Ingredient());
        //     },
        //     (console, controller) =>
        //     {
        //         controller.AddIngredient();
        //     }
        // );

        [Test]
        public void AddIngredientIngredientControllerTest()
        {
            var expectedIngredient = new Ingredient
            {
                IngredientName = "test",
                UnitOfMeasureType = UnitOfMeasure.KILOGRAM
            };
            //mock, act, assert
            RunControllerTest(new []{ expectedIngredient.IngredientName, expectedIngredient.UnitOfMeasureType.ToString(), "-1", "-1", "-1", ""}, null,
                context =>
                {
                    context.Controller.AddIngredient();
                    Assert.That(context.Console.Output.Any(msg => msg.Contains("Dodano")));
                    context.BaseRepository.Verify(r => r.Add(expectedIngredient), Times.Once);
                }
            );
        }
        //
        // private void RunControllerTest(IEnumerable<string> consoleInput, 
        //     Action<Mock<IIngredientRepository>, Mock<IRepository<Ingredient>>> mockDefinitions, 
        //     Action<TestContext<IngredientController, IIngredientRepository, Ingredient>> context)
        // {
        //     var console = new FakeConsole(consoleInput);
        //     var viewHelper = new ViewHelper(console);
        //     var filterHelper = new FilterHelper(console);
        //     var container = TinyIoCContainer.Current;
        //     container.Register<IConsole>(console);
        //     container.Register(viewHelper);
        //     container.Register(filterHelper);
        //     var ingredientRepository = new Mock<IIngredientRepository>();
        //     var baseRepository = new Mock<IRepository<Ingredient>>();
        //     mockDefinitions?.Invoke(ingredientRepository, baseRepository);
        //     container.Register(ingredientRepository.Object);  // to mockowanie trzeba wyciągnąć na zewnątrz
        //     container.Register(baseRepository.Object);  // to też
        //     var testContext = new TestContext<IngredientController, IIngredientRepository, Ingredient>
        //     {
        //         Console = console,
        //         Controller = new IngredientController(),
        //         BaseRepository = baseRepository,
        //         Repository = ingredientRepository,  // ?
        //     };
        //     context(testContext);
        // }

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
        }
    }
    // zrobić przekładkę
    //
    // class TestContext
    // {
    //     public FakeConsole Console { get; set; }
    //     public IngredientController Controller { get; set; }
    //     public Mock<IIngredientRepository> IngredientRepository { get; set; }
    //     public Mock<IRepository<Ingredient>> BaseRepository { get; set; }
    // }

}
