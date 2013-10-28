using Windows.System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WinRTXamlToolkit.Input;

namespace WinRTXamlToolkit.UnitTests
{
    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        public void Key_Parse()
        {
            Assert.AreEqual(Key.Parse(" "), VirtualKey.Space);
            Assert.AreEqual(Key.Parse("Control"), VirtualKey.Control);
        }
    }
}
