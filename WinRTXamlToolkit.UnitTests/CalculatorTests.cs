using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.UnitTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void CalculatorTests_SingleValue()
        {
            Assert.AreEqual(Calculator.Calculate("2"), 2);
            Assert.AreEqual(Calculator.Calculate("2.2"), 2.2);
            Assert.AreEqual(Calculator.Calculate("-2.2"), -2.2);
            Assert.AreEqual(Calculator.Calculate(".2"), .2);
            Assert.AreEqual(Calculator.Calculate("1.2E2"), 1.2E2);
        }

        [TestMethod]
        public void CalculatorTests_SimpleBinaryOperations()
        {
            Assert.AreEqual(Calculator.Calculate("2+4"), 2+4);
            Assert.AreEqual(Calculator.Calculate("4-3"), 4-3);
            Assert.AreEqual(Calculator.Calculate("4*2"), 4*2);
            Assert.AreEqual(Calculator.Calculate("4/2"), 4/2);
            Assert.AreEqual(Calculator.Calculate("3%2"), 3%2);
            Assert.AreEqual(Calculator.Calculate("4^3"), 4*4*4);
        }

        [TestMethod]
        public void CalculatorTests_Mixed()
        {
            Assert.AreEqual(Calculator.Calculate("(2+5)"), (2 + 5));
            Assert.AreEqual(Calculator.Calculate("2*3+4-5*10/2"), 2*3+4-5*10/2);
            Assert.AreEqual(Calculator.Calculate("2*(3+4)-(5+2)*10/2*((3))*2"), 2*(3+4)-(5+2)*10/2*((3))*2);
            Assert.AreEqual(Calculator.Calculate("2*3%4+4-(5*10)^3/2"), 2*3%4+4-Math.Pow(5*10, 3)/2);
            Assert.AreEqual(Calculator.Calculate("2*3%4+4-5*10^3/2"), 2*3%4+4-5*Math.Pow(10, 3)/2);

            double result;
            Assert.IsTrue(Calculator.TryCalculate("(2+5)", out result));
            Assert.AreEqual((2 + 5), result);
            Assert.IsTrue(Calculator.TryCalculate("2*3+4-5*10/2", out result));
            Assert.AreEqual(2*3+4-5*10/2, result);
            Assert.IsTrue(Calculator.TryCalculate("2*(3+4)-(5+2)*10/2*((3))*2", out result));
            Assert.AreEqual(2*(3+4)-(5+2)*10/2*((3))*2, result);
            Assert.IsTrue(Calculator.TryCalculate("2*3%4+4-(5*10)^3/2", out result));
            Assert.AreEqual(2*3%4+4-Math.Pow(5*10,3)/2, result);
            Assert.IsTrue(Calculator.TryCalculate("2*3%4+4-5*10^3/2", out result));
            Assert.AreEqual(2*3%4+4-5*Math.Pow(10,3)/2, result);
        }

        [TestMethod]
        public void CalculatorTests_ErrorCases()
        {
            double result;
            Assert.IsFalse(Calculator.TryCalculate(null, out result));
            Assert.IsFalse(Calculator.TryCalculate("", out result));
            Assert.IsFalse(Calculator.TryCalculate("   ", out result));
            Assert.IsFalse(Calculator.TryCalculate("z", out result));
            Assert.IsFalse(Calculator.TryCalculate("+", out result));
            Assert.IsFalse(Calculator.TryCalculate("-", out result));
            Assert.IsFalse(Calculator.TryCalculate("*", out result));
            Assert.IsFalse(Calculator.TryCalculate("/", out result));
            Assert.IsFalse(Calculator.TryCalculate("%", out result));
            Assert.IsFalse(Calculator.TryCalculate("^", out result));
            Assert.IsFalse(Calculator.TryCalculate(")", out result));
            Assert.IsFalse(Calculator.TryCalculate("(", out result));
            Assert.IsFalse(Calculator.TryCalculate("5+", out result));
            Assert.IsFalse(Calculator.TryCalculate("+5", out result));
            Assert.IsFalse(Calculator.TryCalculate("2+5+", out result));
            Assert.IsFalse(Calculator.TryCalculate("(2+5", out result));
            Assert.IsFalse(Calculator.TryCalculate("2+5)", out result));
            Assert.IsFalse(Calculator.TryCalculate("2+*5", out result));
            Assert.IsFalse(Calculator.TryCalculate("(2+5)(2+3)", out result));
            Assert.IsFalse(Calculator.TryCalculate("2(2+5)", out result));
            Assert.IsFalse(Calculator.TryCalculate("2(2+)", out result));
        }
    }
}
