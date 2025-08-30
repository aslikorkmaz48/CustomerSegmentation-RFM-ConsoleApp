using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MusteriSegmentasyonu
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public int PurchaseCount { get; set; }
        public decimal TotalSpent { get; set; }

        // Hesaplanan alanlar
        public int RecencyDays { get; set; }
        public int R { get; set; }
        public int F { get; set; }
        public int M { get; set; }
        public int RfmScore { get { return R * 100 + F * 10 + M; } }
        public string Segment { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Müşteri Segmentasyonu (RFM) - .NET Framework Konsol Uygulaması ===\n");

            List<Customer> customers = new List<Customer>();

            // Kullanıcıya seçim sor
            Console.WriteLine("1) Örnek verilerle çalış");
            Console.WriteLine("2) CSV dosyasından yükle (Id,Name,LastPurchaseDate,PurchaseCount,TotalSpent)");
            Console.Write("Seçiminiz (1/2): ");
            var choice = Console.ReadLine();

            if (choice == "2")
            {
                Console.Write("CSV dosya yolunu giriniz: ");
                string path = Console.ReadLine();
                if (File.Exists(path))
                {
                    customers = LoadFromCsv(path);
                }
                else
                {
                    Console.WriteLine("❌ Dosya bulunamadı. Örnek veriler kullanılacak.\n");
                    customers = GetSampleData();
                }
            }
            else
            {
                customers = GetSampleData();
            }

            // RFM hesaplama
            DateTime today = DateTime.Today;
            foreach (var c in customers)
                c.RecencyDays = (today - c.LastPurchaseDate).Days;

            foreach (var c in customers)
            {
                c.R = (c.RecencyDays <= 30) ? 5 : (c.RecencyDays <= 60 ? 3 : 1);
                c.F = (c.PurchaseCount >= 10) ? 5 : (c.PurchaseCount >= 5 ? 3 : 1);
                c.M = (c.TotalSpent >= 2000) ? 5 : (c.TotalSpent >= 1000 ? 3 : 1);

                c.Segment = GetSegment(c.R, c.F, c.M);
            }

            // Tabloda yazdır
            Console.WriteLine("\n--- Müşteri Tablosu ---");
            Console.WriteLine("ID | Ad       | Gün  | Freq | Monet | RFM  | Segment");
            Console.WriteLine("------------------------------------------------------------");
            foreach (var c in customers)
            {
                Console.WriteLine($"{c.Id,2} | {c.Name,-8} | {c.RecencyDays,4} | {c.F,4} | {c.M,5} | {c.RfmScore,4} | {c.Segment}");
            }

            // Segment bazlı özet
            Console.WriteLine("\n--- Segment Dağılımı ---");
            var groups = customers.GroupBy(c => c.Segment).OrderByDescending(g => g.Count());
            foreach (var g in groups)
            {
                Console.WriteLine($"{g.Key,-15}: {g.Count()} müşteri");
            }

            // En değerli 3 müşteri
            Console.WriteLine("\n--- En Değerli 3 Müşteri (RFM ve Harcama) ---");
            var top = customers.OrderByDescending(c => c.RfmScore).ThenByDescending(c => c.TotalSpent).Take(3);
            foreach (var c in top)
            {
                Console.WriteLine($"#{c.Id} {c.Name} | RFM: {c.RfmScore} | Harcama: {c.TotalSpent}₺");
            }

            // CSV dışa aktarma
            string outPath = "segmente_musteriler.csv";
            SaveToCsv(customers, outPath);
            Console.WriteLine($"\n📂 Sonuç dışa aktarıldı: {outPath}");

            // Segment filtreleme
            Console.Write("\nBelirli bir segmenti görmek ister misiniz? (ör: Champions): ");
            string filter = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var filtered = customers.Where(c => c.Segment.Equals(filter, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine($"\n{filter} segmentindeki müşteriler:");
                foreach (var c in filtered)
                {
                    Console.WriteLine($"#{c.Id} {c.Name} | RFM:{c.RfmScore} | Gün:{c.RecencyDays} | F:{c.PurchaseCount} | ₺{c.TotalSpent}");
                }
            }

            Console.WriteLine("\n✔ Program tamamlandı. Çıkmak için bir tuşa bas...");
            Console.ReadKey();
        }

        // Segment belirleme
        static string GetSegment(int r, int f, int m)
        {
            if (r == 5 && f >= 3 && m >= 3) return "Champions";
            if (r >= 3 && f >= 3) return "Loyal";
            if (r == 5 && f == 1) return "New Customer";
            if (r <= 2 && f <= 2 && m <= 2) return "Hibernating";
            return "Others";
        }

        // Örnek veri seti
        static List<Customer> GetSampleData()
        {
            return new List<Customer>
            {
                new Customer{ Id=1, Name="Ayşe",   LastPurchaseDate=DateTime.Today.AddDays(-5),  PurchaseCount=12, TotalSpent=2450 },
                new Customer{ Id=2, Name="Mehmet", LastPurchaseDate=DateTime.Today.AddDays(-51), PurchaseCount=5,  TotalSpent=630 },
                new Customer{ Id=3, Name="Zeynep", LastPurchaseDate=DateTime.Today.AddDays(-2),  PurchaseCount=20, TotalSpent=5200 },
                new Customer{ Id=4, Name="Ali",    LastPurchaseDate=DateTime.Today.AddDays(-92), PurchaseCount=3,  TotalSpent=220 },
                new Customer{ Id=5, Name="Ece",    LastPurchaseDate=DateTime.Today.AddDays(-18), PurchaseCount=8,  TotalSpent=1450 },
                new Customer{ Id=6, Name="Merve",  LastPurchaseDate=DateTime.Today.AddDays(-10), PurchaseCount=15, TotalSpent=3000 },
                new Customer{ Id=7, Name="Deniz",  LastPurchaseDate=DateTime.Today.AddDays(-200),PurchaseCount=2,  TotalSpent=120 },
                new Customer{ Id=8, Name="Berk",   LastPurchaseDate=DateTime.Today.AddDays(-1),  PurchaseCount=25, TotalSpent=7800 },
            };
        }

        // CSV'den yükleme
        static List<Customer> LoadFromCsv(string path)
        {
            var list = new List<Customer>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines.Skip(1)) // başlığı atla
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                try
                {
                    var c = new Customer
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        LastPurchaseDate = DateTime.Parse(parts[2], CultureInfo.InvariantCulture),
                        PurchaseCount = int.Parse(parts[3]),
                        TotalSpent = decimal.Parse(parts[4], CultureInfo.InvariantCulture)
                    };
                    list.Add(c);
                }
                catch { }
            }
            return list;
        }

        // CSV'ye kaydetme
        static void SaveToCsv(List<Customer> customers, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine("Id,Name,LastPurchaseDate,PurchaseCount,TotalSpent,R,F,M,RfmScore,Segment");
                foreach (var c in customers)
                {
                    sw.WriteLine($"{c.Id},{c.Name},{c.LastPurchaseDate:yyyy-MM-dd},{c.PurchaseCount},{c.TotalSpent},{c.R},{c.F},{c.M},{c.RfmScore},{c.Segment}");
                }
            }
        }
    }
}
