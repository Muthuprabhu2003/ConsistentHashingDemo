using System;

namespace ConsistentHashingDemo
{
    public static class MurmurHash3
    {
        public static uint Hash(byte[] data, uint seed = 0)
        {
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            uint h1 = seed;
            int length = data.Length;
            int roundedEnd = (length & ~0x3);

            for (int i = 0; i < roundedEnd; i += 4)
            {
                uint k1 = (uint)(data[i]
                    | (data[i + 1] << 8)
                    | (data[i + 2] << 16)
                    | (data[i + 3] << 24));

                k1 *= c1;
                k1 = RotateLeft(k1, 15);
                k1 *= c2;

                h1 ^= k1;
                h1 = RotateLeft(h1, 13);
                h1 = h1 * 5 + 0xe6546b64;
            }

            uint k1_tail = 0;
            int tailIndex = roundedEnd;
            switch (length & 3)
            {
                case 3:
                    k1_tail ^= (uint)data[tailIndex + 2] << 16;
                    goto case 2;
                case 2:
                    k1_tail ^= (uint)data[tailIndex + 1] << 8;
                    goto case 1;
                case 1:
                    k1_tail ^= data[tailIndex];
                    k1_tail *= c1;
                    k1_tail = RotateLeft(k1_tail, 15);
                    k1_tail *= c2;
                    h1 ^= k1_tail;
                    break;
            }

            h1 ^= (uint)length;
            h1 = FMix(h1);

            return h1;
        }

        private static uint RotateLeft(uint x, byte r) => (x << r) | (x >> (32 - r));

        private static uint FMix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}
