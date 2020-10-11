using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Tools;
using NUnit.Framework;

namespace MarioPizzaOriginal.Tests.Tools
{
    [TestFixture]
    public class FakeConsoleTests
    {
        [Test]
        public void WriteLine_OneLine()
        {
            var console = new FakeConsole(null);
            console.WriteLine("xd");
            Assert.That(console.Output.Contains("xd"));
        }

        [Test]
        public void WriteLine_MultipleLines()
        {
            var console = new FakeConsole(null);
            console.WriteLine("xd");
            console.WriteLine("asdfasdfaf asdf as safda");
            console.WriteLine("1231233 1313");
            Assert.That(console.Output.Count == 3);
            Assert.That(console.Output.Contains("1231233 1313"));
        }

        [Test]
        public void Write_OneTime()
        {
            var console = new FakeConsole(null);
            console.Write("1231233");
            Assert.That(console.Output.Count == 1);
            Assert.That(console.Output.Contains("1231233"));
        }

        [Test]
        public void Write_ManyTimes()
        {
            var console = new FakeConsole(null);
            console.Write("1231233");
            console.Write("000");
            console.Write(" hello");
            Assert.That(console.Output.Count, Is.EqualTo(1));
            Assert.That(console.Output[0].Contains("hello"));
        }

        [Test]
        public void Clear_Nothing()
        {
            var console = new FakeConsole(null);
            console.WriteLine("xd");
            console.WriteLine("asdfasdfaf asdf as safda");
            console.WriteLine("1231233 1313");
            console.Clear();
            Assert.That(console.Output.Count == 0);
        }

        [Test]
        public void ReadKey_EnoughCharacters()
        {
            var console = new FakeConsole(new[]{"asdf"});
            var key1 = console.ReadKey(true);
            var key2 = console.ReadKey(true);
            var key3 = console.ReadKey(true);
            var key4 = console.ReadKey(true);
            Assert.That(key1.KeyChar == 'a');
            Assert.That(key2.KeyChar == 's');
            Assert.That(key3.KeyChar == 'd');
            Assert.That(key4.KeyChar == 'f');
        }

        [Test]
        public void ReadKey_NotEnoughCharacters()
        {
            var console = new FakeConsole(new[] { "a" });
            var key1 = console.ReadKey(true);
            Assert.That(key1.KeyChar == 'a');
            Assert.Throws<FakeConsoleNoInputException>(() => console.ReadKey(true));
        }

        [Test]
        public void ReadKey_Ok_WhenManyLines()
        {
            var console = new FakeConsole(new[] { "a", "b", "c" });
            var key1 = console.ReadKey(true);
            var key2 = console.ReadKey(true);
            var key3 = console.ReadKey(true);
            Assert.That(key1.KeyChar == 'a');
            Assert.That(key2.KeyChar == 'b');
            Assert.That(key3.KeyChar == 'c');
        }

        [Test]
        public void ReadLine_OneLine()
        {
            var console = new FakeConsole(new []{"xd"});
            var newLine = console.ReadLine();
            Assert.That(newLine == "xd");
            Assert.That(!console.Input.Contains("xd"));
            Assert.That(console.Input.Count == 0);
        }

        [Test]
        public void ReadLine_MultipleLines()
        {
            var console = new FakeConsole(new[] { "xd", "Hello lady!", "Hello there!" });
            Assert.That(console.ReadLine() == "xd");
            Assert.That(console.ReadLine() == "Hello lady!");
            Assert.That(console.Input.Count == 1);
        }

        [Test]
        public void ReadLine_ReadKeyMixed()
        {
            var console = new FakeConsole(new[] { "Hello lady!", "Hello there!" });
            Assert.That(console.ReadKey(true).KeyChar == 'H');
            Assert.That(console.ReadLine() == "ello lady!");
            Assert.That(console.Input.Count == 1);
        }
    }
}
