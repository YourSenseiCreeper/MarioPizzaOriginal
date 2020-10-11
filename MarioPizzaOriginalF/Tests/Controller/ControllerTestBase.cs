using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Filter;
using MarioPizzaOriginal.Tools;
using Moq;
using TinyIoC;

namespace MarioPizzaOriginal.Tests.Controller
{
    public class ControllerTestBase<TController, TRepository, TClass>
        where TController : class, new()
        where TRepository : class
        where TClass : class, new()
    {
        protected void RunControllerTest(IEnumerable<string> consoleInput,
            Action<Mock<TRepository>, Mock<IRepository<TClass>>> mockDefinitions,
            Action<TestContext<TController, TRepository, TClass>> context)
        {
            var console = new FakeConsole(consoleInput);
            var viewHelper = new ViewHelper(console);
            var filterHelper = new FilterHelper(console);
            var container = TinyIoCContainer.Current;
            container.Register<IConsole>(console);
            container.Register(viewHelper);
            container.Register(filterHelper);
            var ingredientRepository = new Mock<TRepository>();
            var baseRepository = new Mock<IRepository<TClass>>();
            mockDefinitions?.Invoke(ingredientRepository, baseRepository);
            container.Register(ingredientRepository.Object);  // to mockowanie trzeba wyciągnąć na zewnątrz
            container.Register(baseRepository.Object);  // to też
            var testContext = new TestContext<TController, TRepository, TClass>
            {
                Console = console,
                Controller = new TController(),
                BaseRepository = baseRepository,
                Repository = ingredientRepository,
            };
            context(testContext);
            TinyIoCContainer.Current.Dispose();
        }
    }

    public class TestContext<TController, TRepository, TClass>
        where TController : class, new()
        where TRepository : class
        where TClass : class, new()
    {
        public FakeConsole Console { get; set; }
        public TController Controller { get; set; }
        public Mock<TRepository> Repository { get; set; }
        public Mock<IRepository<TClass>> BaseRepository { get; set; }
    }
}
