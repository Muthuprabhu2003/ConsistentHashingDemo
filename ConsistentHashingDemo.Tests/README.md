# ConsistentHashingDemo  
Advanced Implementation of a Consistent Hashing Ring (C# / .NET 7)

This project implements a production-ready **consistent hashing ring** using:

- 📌 MurmurHash3 (32-bit, non-cryptographic, industry-grade)
- 📌 Virtual nodes for smooth load balancing
- 📌 Sorted dictionary–based O(log N) lookup
- 📌 Collision handling & deterministic vnode generation
- 📌 Unit tests (MSTest)

Consistent hashing is used in distributed systems such as:
Redis, Cassandra, DynamoDB, Kafka, load balancers, distributed caches, CDNs, and microservice routing.

---

## 🚀 Features

### ✔ ConsistentHashRing Class
- Add & remove physical nodes dynamically  
- Virtual node support (default: 100–300 vnodes per physical node)  
- Fast lookup (binary search on sorted hash ring)  
- Minimizes key movement on topology changes  
- Handles hash collisions gracefully  

### ✔ MurmurHash3
- High-performance hash function  
- Excellent distribution characteristics  
- Widely used in distributed systems  

### ✔ Unit Tests
- Node addition/removal
- Empty ring error handling
- Migration percentage verification (25–45%)  
- Ensures correct load balancing behavior  

---

## 🏗 Project Structure

ConsistentHashingDemo/
│ README.md
│ LICENSE
│ ConsistentHashingDemo.sln
│
├── ConsistentHashingDemo/
│ ├── Program.cs
│ ├── ConsistentHashRing.cs
│ ├── MurmurHash3.cs
│ └── ConsistentHashingDemo.csproj
│
└── ConsistentHashingDemo.Tests/
├── ConsistentHashingTests.cs
├── GlobalUsings.cs
└── ConsistentHashingDemo.Tests.csproj


---

## ▶ Running the Project

### Build
```bash
dotnet build

dotnet run --project ConsistentHashingDemo/ConsistentHashingDemo.csproj

dotnet test

Example Output (Shortened)
Initial distribution:
NodeA : 23.46%
NodeB : 23.63%
NodeC : 27.40%
NodeD : 25.50%

After removing NodeC:
NodeA : 35.30%
NodeB : 31.55%
NodeD : 33.14%

Keys moved: 27.40%

---

## 📘 License

See LICENSE file.

📧 Author

Muthu Prabhu
C# / Distributed Systems / Consistent Hashing
GitHub: https://github.com/Muthuprabhu2003