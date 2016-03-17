using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.UnitTests
{
    [TestClass]
    public class EventThrottlerTests
    {
        [TestMethod]
        public async Task Event_Throttler_TResult()
        {
            var et = new EventThrottler<int>();

            Func<Task<int>> f1 = async () => { Debug.WriteLine("Starting 1"); await Task.Delay(50); Debug.WriteLine("Ending 1"); return 1; };
            Func<Task<int>> f2 = async () => { Debug.WriteLine("Starting 2"); await Task.Delay(50); Debug.WriteLine("Ending 2"); return 2; };
            Func<Task<int>> f3 = async () => { Debug.WriteLine("Starting 3"); await Task.Delay(50); Debug.WriteLine("Ending 3"); return 3; };
            var t1 = et.RunAsync(f1, -1);
            var t2 = et.RunAsync(f2, -2);
            var t3 = et.RunAsync(f3, -3);
            Assert.AreEqual(await t1, 1);
            Assert.AreEqual(await t2, -2);
            Assert.AreEqual(await t3, 3);
        }
    }
}
