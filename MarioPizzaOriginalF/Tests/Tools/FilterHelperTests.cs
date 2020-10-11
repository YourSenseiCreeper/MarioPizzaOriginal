using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.Filter;
using MarioPizzaOriginal.Tools;
using NUnit.Framework;

namespace MarioPizzaOriginal.Tests.Tools
{
    [TestFixture]
    public class FilterHelperTests
    {
        [Test]
        public void FilterInt_OK_WhenCorrectInput()
        {
            RunConsoleTest(new[] { "1 " }, (console, filter) =>
            {
                var answer = filter.FilterInt("Podaj liczbę: ", null);
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(1, answer);
            });
        }

        [Test]
        public void FilterInt_Repeat_WhenIncorrectInput()
        {
            // trailing spaces are required, they simulate ENTER key
            RunConsoleTest(new[] { "g ", " ", "1 " }, (console, filter) =>
            {
                var answer = filter.FilterInt("Podaj liczbę: ", null);
                Assert.AreEqual(3, console.Output.Count);
                Assert.AreEqual(1, answer);
            });
        }

        [Test]
        public void FilterInt_EditFilter_WhenIncorrectInput()
        {
            RunConsoleTest(new[] { "1 " }, (console, filter) =>
            {
                var answer = filter.FilterInt("Podaj liczbę: ", new object[]{ "3" });
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(31, answer);
            });
        }

        [Test]
        public void FilterInt_EditedValue_WhenRemovedLetter()
        {
            RunConsoleTest(new[] { "\b " }, (console, filter) =>
            {
                var answer = filter.FilterInt("Podaj liczbę: ", new object[] { "12345" });
                Assert.AreEqual(1, console.Output.Count);
                Assert.AreEqual(1234, answer);
            });
        }

        [Test]
        public void FilterInt_Null_WhenRemovedAllInput()
        {
            // last backspace is for bypassing pasting an ' ' and resulting loop
            RunConsoleTest(new[] { "\b\b\b " }, (console, filter) =>
            {
                var answer = filter.FilterInt("Podaj liczbę: ", new object[] { "123" });
                Assert.AreEqual(1, console.Output.Count);
                Assert.Null(answer);
            });
        }

        private static void RunConsoleTest(IEnumerable<string> input, Action<FakeConsole, FilterHelper> context)
        {
            var console = new FakeConsole(input);
            context(console, new FilterHelper(console));
        }
    }
}
