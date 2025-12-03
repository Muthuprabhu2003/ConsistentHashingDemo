using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashingDemo
{
    public class ConsistentHashRing
    {
        private readonly SortedDictionary<uint, string> ring;
        private readonly Dictionary<string, List<uint>> nodeToHashes;
        private readonly int virtualNodes;
        private readonly object sync = new object();

        public ConsistentHashRing(int virtualNodesPerPhysical = 100)
        {
            if (virtualNodesPerPhysical <= 0) virtualNodesPerPhysical = 100;
            virtualNodes = virtualNodesPerPhysical;
            ring = new SortedDictionary<uint, string>();
            nodeToHashes = new Dictionary<string, List<uint>>();
        }

        public void AddNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName)) throw new ArgumentException(nameof(nodeName));
            lock (sync)
            {
                if (nodeToHashes.ContainsKey(nodeName)) return;

                var hashes = new List<uint>(virtualNodes);

                for (int i = 0; i < virtualNodes; i++)
                {
                    string vnodeKey = $"{nodeName}-VN{i}";
                    uint h = MurmurHash3.Hash(Encoding.UTF8.GetBytes(vnodeKey), 0);

                    int probe = 0;
                    while (ring.ContainsKey(h))
                    {
                        probe++;
                        h = MurmurHash3.Hash(Encoding.UTF8.GetBytes(vnodeKey + ":" + probe), 0);
                    }

                    ring[h] = nodeName;
                    hashes.Add(h);
                }

                nodeToHashes[nodeName] = hashes;
            }
        }

        public void RemoveNode(string nodeName)
        {
            if (!nodeToHashes.ContainsKey(nodeName)) return;

            lock (sync)
            {
                foreach (var h in nodeToHashes[nodeName])
                    ring.Remove(h);

                nodeToHashes.Remove(nodeName);
            }
        }

        public string GetNode(string key)
        {
            if (ring.Count == 0) throw new InvalidOperationException("Hash ring is empty.");

            uint h = MurmurHash3.Hash(Encoding.UTF8.GetBytes(key), 0);

            foreach (var kv in ring)
                if (kv.Key >= h) return kv.Value;

            return ring.First().Value;
        }

        public int VirtualNodeCount => ring.Count;
        public int PhysicalNodeCount => nodeToHashes.Count;
    }
}
