using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace MarioPizzaOriginal.Tests
{
    [TestFixture]
    public class ViewHelperTests
    {
        [TearDown]
        public void TearDown()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
        }

        [Test]
        public void AskForInt_CorrectInput_IntOutput()
        {
            StringReader sr = new StringReader("0");
            Console.SetIn(sr);
            int result = ViewHelper.AskForInt("Podaj liczbę: ", clear: false);
            Assert.AreEqual(result, 0);
            sr.Close();
        }

        [Test]
        public void AskForInt_IncorrectInput_Repeated()
        {
            StringReader sr = new StringReader("a\n\n0");
            StringWriter sw = new StringWriter();
            Console.SetIn(sr);
            Console.SetOut(sw);

            int result = ViewHelper.AskForInt("Podaj liczbę: ", clear: false);
            string response = sw.ToString();
            Assert.That(response.Contains("nie jest liczbą!"));
            Assert.AreEqual(result, 0);

            sr.Close();
            sw.Close();
        }

        [Test]
        public void AskForInt_HigherThanMinimum_IntOutput()
        {
            StringReader sr = new StringReader("1");
            Console.SetIn(sr);

            int result = ViewHelper.AskForInt("Podaj liczbę: ", min: 0, clear: false);
            Assert.AreEqual(result, 1);

            sr.Close();
        }

        [Test]
        public void AskForInt_LowerThanMinimum_Repeated()
        {
            StringReader sr = new StringReader("-1\n\n1");
            StringWriter sw = new StringWriter();
            Console.SetIn(sr);
            Console.SetOut(sw);

            int result = ViewHelper.AskForInt("Podaj liczbę: ", min: 0, clear: false);
            var response = sw.ToString();
            Assert.That(response.Contains("nie może być mniejsza niż"));
            Assert.AreEqual(result, 1);

            sr.Close();
            sw.Close();
        }
    }
}
