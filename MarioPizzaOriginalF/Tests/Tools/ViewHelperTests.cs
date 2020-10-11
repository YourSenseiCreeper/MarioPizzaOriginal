using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Tools;
using NUnit.Framework;

namespace MarioPizzaOriginal.Tests.Tools
{
    [TestFixture]
    public class ViewHelperTests
    {

        [Test]
        public void AskForInt_CorrectInput_IntOutput()
        {
            var console = new FakeConsole(new [] {"0"});
            var helper = new ViewHelper(console);
            var result = helper.AskForInt("Podaj liczbę: ", clear: false);
            Assert.AreEqual(result, 0);
        }

        [Test]
        public void AskForInt_IncorrectInput_Repeated()
        {
            var console = new FakeConsole(new[] { "a", "", "0" });
            var helper = new ViewHelper(console);
            var result = helper.AskForInt("Podaj liczbę: ", clear: false);
            var response = console.Output[1];
            Assert.AreEqual(3, console.Output.Count);
            Assert.That(response.Contains("nie jest liczbą!"));
            Assert.AreEqual(result, 0);
        }

        [Test]
        public void AskForInt_HigherThanMinimum_IntOutput()
        {
            var console = new FakeConsole(new[] { "1" });
            var helper = new ViewHelper(console);
            var result = helper.AskForInt("Podaj liczbę: ", min: 0, clear: false);
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void AskForInt_LowerThanMinimum_Repeated()
        {
            var console = new FakeConsole(new[] { "-1", "", "1" });
            var helper = new ViewHelper(console);
            var result = helper.AskForInt("Podaj liczbę: ", min: 0, clear: false);
            var response = console.Output[1];
            Assert.AreEqual(3, console.Output.Count);
            Assert.That(response.Contains("nie może być mniejsza niż"));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void AskForDouble_BlankInputNoClear_Error()
        {
            RunConsoleTest(new []{ " ", "", "1" }, (console, helper) =>
            {
                var result = helper.AskForDouble("Podaj kwotę: ", clear: false);
                var response = console.Output[1];
                Assert.AreEqual(3, console.Output.Count);
                Assert.That(response.Contains("nie jest liczbą!"));
                Assert.AreEqual(1, result);
            });
        }

        [Test]
        public void AskForDouble_BlankInputWithClear_Error()
        {
            RunConsoleTest(new[] { " ", "", "1" }, (console, helper) =>
            {
                var result = helper.AskForDouble("Podaj kwotę: ", clear: true);
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(1, result);
            });
        }

        [Test]
        public void AskForDouble_ValueWithComma_Ok()
        {
            RunConsoleTest(new[] { "1,1" }, (console, helper) =>
            {
                var result = helper.AskForDouble("Podaj kwotę: ", clear: true);
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(1.1, result);
            });
        }

        [Test]
        public void AskForDouble_ValueWithPeriod_Ok()
        {
            RunConsoleTest(new[] { "1.1" }, (console, helper) =>
            {
                var result = helper.AskForDouble("Podaj kwotę: ", clear: true);
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(1.1, result);
            });
        }

        [Test]
        public void AskForYesNo_OK_WhenN()
        {
            RunConsoleTest(new[] { "n" }, (console, helper) =>
            {
                var answer = helper.AskForYesNo("Zgadzasz się na zesłanie na sybir?");
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(false, answer);
            });
        }

        [Test]
        public void AskForYesNo_OK_WhenY()
        {
            RunConsoleTest(new[] { "y" }, (console, helper) =>
            {
                var answer = helper.AskForYesNo("Chcesz dyszkę?");
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(true, answer);
            });
        }

        [Test]
        public void AskForYesNo_False_WhenWrongInput()
        {
            RunConsoleTest(new[] { "g" }, (console, helper) =>
            {
                var answer = helper.AskForYesNo("Chcesz dyszkę?");
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(false, answer);
            });
        }

        [Test]
        public void AskForOption_Ok_WhenInputFromListByNumber()
        {
            RunConsoleTest(new[] { "3" }, (console, helper) =>
            {
                var answer = helper.AskForOption<OrderStatus>("Wszystkie elementy:", "Wybierz status:");
                Assert.AreEqual(6, console.Output.Count);
                Assert.AreEqual(OrderStatus.DONE, answer);
            });
        }

        [Test]
        public void AskForOption_Ok_WhenInputFromListByName()
        {
            RunConsoleTest(new[] { "DONE" }, (console, helper) =>
            {
                var answer = helper.AskForOption<OrderStatus>("Wszystkie elementy:", "Wybierz status:");
                Assert.AreEqual(OrderStatus.DONE, answer);
            });
        }

        [Test]
        public void EditableValue_FiveLetters_Ok()
        {
            RunConsoleTest(new[] { "Peter " }, (console, helper) =>
            {
                console.LoadKeylist();
                var answer = helper.EditableString("Test", "");
                Assert.AreEqual("Peter", answer);
            });
        }

        [Test]
        public void AskForListFromPrepared_AddedToList_WhenTypeIsStringListIsBlank()
        {
            RunConsoleTest(new[] { "1", "0", "3", "" }, (console, helper) =>
            {
                var markers = new List<string> { "Niebieski", "Czerwony", "Zielony", "Żółty", "Różowy", "Czarny"};
                var answer = helper.AskForListFromPrepared("Wszystkie elementy: ", "Nowy element: ", e => e, markers,
                    "Dodaj pisak", "Usuń pisak");
                Assert.AreEqual(1, answer.Count);
                Assert.That(answer.Any(p => p == "Niebieski"));
            });
        }

        [Test]
        public void AskForListFromPrepared_RemovedFromList_WhenTypeIsStringListIsNotBlank()
        {
            RunConsoleTest(new[] { "2", "Niebieski", "3", "" }, (console, helper) =>
            {
                var markers = new List<string> { "Niebieski", "Czerwony", "Zielony", "Żółty", "Różowy", "Czarny" };
                var answer = helper.AskForListFromPrepared("Wszystkie elementy: ", "Nowy element: ", e => e, markers,
                    "Dodaj pisak", "Usuń pisak", new List<string>{ "Niebieski"});
                Assert.AreEqual(0, answer.Count);
            });
        }

        [Test]
        public void AskForListFromPrepared_RemovedFromList_WhenRemoveById()
        {
            RunConsoleTest(new[] { "2", "1", "3", "" }, (console, helper) =>
            {
                var markers = new List<string> { "Niebieski", "Czerwony", "Zielony", "Żółty", "Różowy", "Czarny" };
                var answer = helper.AskForListFromPrepared("Wszystkie elementy: ", "Nowy element: ", e => e, markers,
                    "Dodaj pisak", "Usuń pisak", new List<string> { "Niebieski" });
                Assert.AreEqual(0, answer.Count);
            });
        }

        [Test]
        public void AskForListFromPrepared_RemovedFromList_WhenRemoveOutOfRange()
        {
            RunConsoleTest(new[] { "2", "2", "", "3", "" }, (console, helper) =>
            {
                var markers = new List<string> { "Niebieski", "Czerwony", "Zielony", "Żółty", "Różowy", "Czarny" };
                var answer = helper.AskForListFromPrepared("Wszystkie elementy: ", "Nowy element: ", e => e, markers,
                    "Dodaj pisak", "Usuń pisak", new List<string> { "Niebieski" });
                Assert.AreEqual(1, answer.Count);
                Assert.That(console.Output.Any(line => line.Contains("Numer musi być z zakresu od 1 do 1")));
            });
        }

        private void RunConsoleTest(IEnumerable<string> input, Action<FakeConsole, ViewHelper> context)
        {
            var console = new FakeConsole(input);
            context(console, new ViewHelper(console));
        }
    }
}
