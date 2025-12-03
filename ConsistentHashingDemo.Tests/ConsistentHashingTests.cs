using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConsistentHashingDemo.Tests
{
    [TestClass]
    public class ConsistentHashingTests
    {
        [TestMethod]
        public void AddNode_IncreasesCounts()
        {
            var ring = new ConsistentHashRing(50);
            ring.AddNode("X");
            ring.AddNode("Y");
            Assert.AreEqual(2, ring.PhysicalNodeCount);
            Assert.AreEqual(100, ring.VirtualNodeCount);
        }

        [TestMethod]
        public void GetNode_Throws_WhenEmpty()
        {
            var ring = new ConsistentHashRing(10);
            Assert.ThrowsException<System.InvalidOperationException>(() => ring.GetNode("k"));
        }

        [TestMethod]
        public void RemoveNode_Redistributes_Approximately()
        {
            var ring = new ConsistentHashRing(100);
            var nodes = new[] { "A", "B", "C" };
            foreach (var n in nodes) ring.AddNode(n);

            int totalKeys = 30000;

            var before = new Dictionary<int, string>();
            for (int i = 0; i < totalKeys; i++)
                before[i] = ring.GetNode($"k{i}");

            ring.RemoveNode("B");

            int moved = 0;
            for (int i = 0; i < totalKeys; i++)
                if (ring.GetNode($"k{i}") != before[i]) moved++;

            double ratio = (double)moved / totalKeys;
            Assert.IsTrue(ratio > 0.25 && ratio < 0.45);
        }
    }
}
