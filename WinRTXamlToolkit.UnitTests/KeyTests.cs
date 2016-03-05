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

        [TestMethod]
        public void KeyGesture_Parse()
        {
            // TODO: Modify the code so that the Ctrl/Shift/Alt+ modifiers apply to a gesture rather than key command.
            var cordGesture = KeyGesture.Parse("Ctrl+D");
            Assert.AreEqual(cordGesture.Count, 1);
            Assert.AreEqual(cordGesture[0].Count, 2);
            Assert.AreEqual(cordGesture[0][0], VirtualKey.Control);
            Assert.AreEqual(cordGesture[0][1], VirtualKey.D);

            var threeKeyCordGesture = KeyGesture.Parse("Ctrl+Shift+D");
            Assert.AreEqual(threeKeyCordGesture.Count, 1);
            Assert.AreEqual(threeKeyCordGesture[0].Count, 3);
            Assert.AreEqual(threeKeyCordGesture[0][0], VirtualKey.Control);
            Assert.AreEqual(threeKeyCordGesture[0][1], VirtualKey.Shift);
            Assert.AreEqual(threeKeyCordGesture[0][2], VirtualKey.D);

            var doubleCordGesture = KeyGesture.Parse("Ctrl+S,D");
            Assert.AreEqual(doubleCordGesture.Count, 2);
            Assert.AreEqual(doubleCordGesture[0].Count, 2);
            Assert.AreEqual(doubleCordGesture[0][0], VirtualKey.Control);
            Assert.AreEqual(doubleCordGesture[0][1], VirtualKey.S);
            Assert.AreEqual(doubleCordGesture[1].Count, 1);
            Assert.AreEqual(doubleCordGesture[1][0], VirtualKey.D);
        }
    }
}
