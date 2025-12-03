using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsistentHashingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consistent Hashing Ring - Demo\n");

            int virtualNodesPerPhysical = 200;
            int totalKeys = 100_000;

            var ring = new ConsistentHashRing(virtualNodesPerPhysical);

            var physicalNodes = new[] { "NodeA", "NodeB", "NodeC", "NodeD" };
            foreach (var n in physicalNodes)
            {
                ring.AddNode(n);
                Console.WriteLine($"Added {n}");
            }

            Console.WriteLine($"\nPhysical nodes: {ring.PhysicalNodeCount}, Virtual nodes: {ring.VirtualNodeCount}\n");

            var before = new Dictionary<int, string>(totalKeys);
            var distBefore = new Dictionary<string, int>();

            Console.WriteLine($"Mapping {totalKeys:N0} keys...");
            for (int i = 0; i < totalKeys; i++)
            {
                string key = $"user:{i}";
                string node = ring.GetNode(key);
                before[i] = node;
                if (!distBefore.ContainsKey(node)) distBefore[node] = 0;
                distBefore[node]++;
            }

            Print("Initial distribution", distBefore, totalKeys);

            string removed = "NodeC";
            Console.WriteLine($"\nRemoving {removed}\n");
            ring.RemoveNode(removed);

            var after = new Dictionary<string, int>();
            int moved = 0;

            for (int i = 0; i < totalKeys; i++)
            {
                string key = $"user:{i}";
                string newNode = ring.GetNode(key);

                if (!after.ContainsKey(newNode)) after[newNode] = 0;
                after[newNode]++;

                if (before[i] != newNode) moved++;
            }

            Print("After removal", after, totalKeys);

            Console.WriteLine($"\nKeys moved: {moved} / {totalKeys} = {(100.0 * moved / totalKeys):F4}%");

            Console.WriteLine("\nSample lookups:");
            foreach (var s in new[] { "user:10", "user:9999", "order:12345", "file:abcd" })
                Console.WriteLine($"{s} -> {ring.GetNode(s)}");

            Console.WriteLine("\nDone.");
        }

        static void Print(string title, Dictionary<string, int> dist, int total)
        {
            Console.WriteLine($"\n{title}:");
            foreach (var kv in dist.OrderBy(k => k.Key))
            {
                double pct = 100.0 * kv.Value / total;
                Console.WriteLine($"  {kv.Key,-6} : {kv.Value,7:N0} keys ({pct:F4}%)");
            }
        }
    }
}
