using ApplicationCore.Models;

namespace ApplicationCore.DataAccess
{
    public class ProductsData
    {
        public List<Product> Products = new()
        {
            // SKU-001 — Wireless Mouse
            new() { Id = 1,  Guid = Guid.Parse("20000001-0000-0000-0000-000000000001"), Name = "Wireless Mouse – Black Edition",        Sku = "SKU-001", Currency = "INR", Amount = 1.12m },
            new() { Id = 2,  Guid = Guid.Parse("20000002-0000-0000-0000-000000000002"), Name = "Wireless Mouse – White Edition",        Sku = "SKU-001", Currency = "INR", Amount = 2.50m },
            new() { Id = 3,  Guid = Guid.Parse("20000003-0000-0000-0000-000000000003"), Name = "Wireless Mouse – Travel Edition",       Sku = "SKU-001", Currency = "USD", Amount = 0.99m },

            // SKU-002 — Mechanical Keyboard
            new() { Id = 4,  Guid = Guid.Parse("20000004-0000-0000-0000-000000000004"), Name = "Mechanical Keyboard – Blue Switch",     Sku = "SKU-002", Currency = "INR", Amount = 10.00m },
            new() { Id = 5,  Guid = Guid.Parse("20000005-0000-0000-0000-000000000005"), Name = "Mechanical Keyboard – Brown Switch",    Sku = "SKU-002", Currency = "EUR", Amount = 9.20m },
            new() { Id = 6,  Guid = Guid.Parse("20000006-0000-0000-0000-000000000006"), Name = "Mechanical Keyboard – Red Switch",      Sku = "SKU-002", Currency = "INR", Amount = 8.75m },

            // SKU-003 — USB-C Charger
            new() { Id = 7,  Guid = Guid.Parse("20000007-0000-0000-0000-000000000007"), Name = "USB-C Charger 65W – Compact",            Sku = "SKU-003", Currency = "USD", Amount = 5.00m },
            new() { Id = 8,  Guid = Guid.Parse("20000008-0000-0000-0000-000000000008"), Name = "USB-C Charger 65W – Fast Charge",        Sku = "SKU-003", Currency = "USD", Amount = 5.50m },
            new() { Id = 9,  Guid = Guid.Parse("20000009-0000-0000-0000-000000000009"), Name = "USB-C Charger 65W – Office Edition",     Sku = "SKU-003", Currency = "INR", Amount = 420.00m },

            // SKU-004 — Noise Cancelling Headphones
            new() { Id = 10, Guid = Guid.Parse("20000010-0000-0000-0000-000000000010"), Name = "Noise Cancelling Headphones – Black",    Sku = "SKU-004", Currency = "INR", Amount = 15.00m },
            new() { Id = 11, Guid = Guid.Parse("20000011-0000-0000-0000-000000000011"), Name = "Noise Cancelling Headphones – Silver",   Sku = "SKU-004", Currency = "INR", Amount = 15.00m },
            new() { Id = 12, Guid = Guid.Parse("20000012-0000-0000-0000-000000000012"), Name = "Noise Cancelling Headphones – Lite",     Sku = "SKU-004", Currency = "USD", Amount = 0.18m },

            // SKU-005 — Laptop Stand
            new() { Id = 13, Guid = Guid.Parse("20000013-0000-0000-0000-000000000013"), Name = "Aluminium Laptop Stand – Silver",        Sku = "SKU-005", Currency = "EUR", Amount = 2.30m },
            new() { Id = 14, Guid = Guid.Parse("20000014-0000-0000-0000-000000000014"), Name = "Aluminium Laptop Stand – Space Grey",    Sku = "SKU-005", Currency = "EUR", Amount = 2.50m },
            new() { Id = 15, Guid = Guid.Parse("20000015-0000-0000-0000-000000000015"), Name = "Foldable Laptop Stand – Travel",         Sku = "SKU-005", Currency = "INR", Amount = 199.00m },

            // SKU-006 — External SSD
            new() { Id = 16, Guid = Guid.Parse("20000016-0000-0000-0000-000000000016"), Name = "External SSD 1TB – USB 3.2",             Sku = "SKU-006", Currency = "USD", Amount = 12.00m },
            new() { Id = 17, Guid = Guid.Parse("20000017-0000-0000-0000-000000000017"), Name = "External SSD 1TB – Rugged",              Sku = "SKU-006", Currency = "USD", Amount = 12.75m },
            new() { Id = 18, Guid = Guid.Parse("20000018-0000-0000-0000-000000000018"), Name = "External SSD 1TB – Office Backup",       Sku = "SKU-006", Currency = "INR", Amount = 999.00m },

            // SKU-007 — USB Hub
            new() { Id = 19, Guid = Guid.Parse("20000019-0000-0000-0000-000000000019"), Name = "USB Hub 7-in-1 – Aluminium",              Sku = "SKU-007", Currency = "INR", Amount = 50.00m },
            new() { Id = 20, Guid = Guid.Parse("20000020-0000-0000-0000-000000000020"), Name = "USB Hub 7-in-1 – Slim",                   Sku = "SKU-007", Currency = "USD", Amount = 0.60m },
            new() { Id = 21, Guid = Guid.Parse("20000021-0000-0000-0000-000000000021"), Name = "USB Hub 7-in-1 – Desktop Edition",         Sku = "SKU-007", Currency = "EUR", Amount = 0.55m },

            // SKU-008 — Webcam
            new() { Id = 22, Guid = Guid.Parse("20000022-0000-0000-0000-000000000022"), Name = "Full HD Webcam – Autofocus",              Sku = "SKU-008", Currency = "INR", Amount = 75.00m },
            new() { Id = 23, Guid = Guid.Parse("20000023-0000-0000-0000-000000000023"), Name = "Full HD Webcam – Wide Angle",             Sku = "SKU-008", Currency = "INR", Amount = 80.00m },
            new() { Id = 24, Guid = Guid.Parse("20000024-0000-0000-0000-000000000024"), Name = "Full HD Webcam – Streaming Edition",      Sku = "SKU-008", Currency = "USD", Amount = 0.95m },

            // SKU-009 — Bluetooth Speaker
            new() { Id = 25, Guid = Guid.Parse("20000025-0000-0000-0000-000000000025"), Name = "Bluetooth Speaker – Portable",             Sku = "SKU-009", Currency = "EUR", Amount = 3.40m },
            new() { Id = 26, Guid = Guid.Parse("20000026-0000-0000-0000-000000000026"), Name = "Bluetooth Speaker – Bass Boost",           Sku = "SKU-009", Currency = "EUR", Amount = 3.60m },
            new() { Id = 27, Guid = Guid.Parse("20000027-0000-0000-0000-000000000027"), Name = "Bluetooth Speaker – Home Edition",         Sku = "SKU-009", Currency = "INR", Amount = 310.00m },

            // SKU-010 — Smart LED Bulb
            new() { Id = 28, Guid = Guid.Parse("20000028-0000-0000-0000-000000000028"), Name = "Smart LED Bulb – Warm White",              Sku = "SKU-010", Currency = "INR", Amount = 120.00m },
            new() { Id = 29, Guid = Guid.Parse("20000029-0000-0000-0000-000000000029"), Name = "Smart LED Bulb – RGB",                     Sku = "SKU-010", Currency = "USD", Amount = 1.40m },
            new() { Id = 30, Guid = Guid.Parse("20000030-0000-0000-0000-000000000030"), Name = "Smart LED Bulb – Energy Saver",            Sku = "SKU-010", Currency = "USD", Amount = 1.50m },

            // SKU-011 — Wi-Fi Router
            new() { Id = 31, Guid = Guid.Parse("20000031-0000-0000-0000-000000000031"), Name = "Wi-Fi Router AX3000 – Dual Band",          Sku = "SKU-011", Currency = "INR", Amount = 5.00m },
            new() { Id = 32, Guid = Guid.Parse("20000032-0000-0000-0000-000000000032"), Name = "Wi-Fi Router AX3000 – Mesh Ready",         Sku = "SKU-011", Currency = "USD", Amount = 0.07m },
            new() { Id = 33, Guid = Guid.Parse("20000033-0000-0000-0000-000000000033"), Name = "Wi-Fi Router AX3000 – Home Edition",       Sku = "SKU-011", Currency = "EUR", Amount = 0.06m },

            // SKU-012 — Power Bank
            new() { Id = 34, Guid = Guid.Parse("20000034-0000-0000-0000-000000000034"), Name = "Power Bank 20000mAh – Fast Charge",        Sku = "SKU-012", Currency = "INR", Amount = 60.00m },
            new() { Id = 35, Guid = Guid.Parse("20000035-0000-0000-0000-000000000035"), Name = "Power Bank 20000mAh – Slim",               Sku = "SKU-012", Currency = "USD", Amount = 0.75m },
            new() { Id = 36, Guid = Guid.Parse("20000036-0000-0000-0000-000000000036"), Name = "Power Bank 20000mAh – Travel Edition",     Sku = "SKU-012", Currency = "USD", Amount = 0.80m },

            // SKU-013 — Gaming Mouse Pad
            new() { Id = 37, Guid = Guid.Parse("20000037-0000-0000-0000-000000000037"), Name = "Gaming Mouse Pad XL – Control",             Sku = "SKU-013", Currency = "INR", Amount = 33.00m },
            new() { Id = 38, Guid = Guid.Parse("20000038-0000-0000-0000-000000000038"), Name = "Gaming Mouse Pad XL – Speed",               Sku = "SKU-013", Currency = "EUR", Amount = 0.40m },
            new() { Id = 39, Guid = Guid.Parse("20000039-0000-0000-0000-000000000039"), Name = "Gaming Mouse Pad XL – Desk Edition",        Sku = "SKU-013", Currency = "EUR", Amount = 0.42m },

            // SKU-014 — Smartwatch
            new() { Id = 40, Guid = Guid.Parse("20000040-0000-0000-0000-000000000040"), Name = "Smartwatch Series 5 – Black",               Sku = "SKU-014", Currency = "INR", Amount = 88.00m },
            new() { Id = 41, Guid = Guid.Parse("20000041-0000-0000-0000-000000000041"), Name = "Smartwatch Series 5 – Silver",              Sku = "SKU-014", Currency = "USD", Amount = 1.05m },
            new() { Id = 42, Guid = Guid.Parse("20000042-0000-0000-0000-000000000042"), Name = "Smartwatch Series 5 – Fitness Edition",    Sku = "SKU-014", Currency = "USD", Amount = 1.10m },

            // SKU-015 — Fitness Band
            new() { Id = 43, Guid = Guid.Parse("20000043-0000-0000-0000-000000000043"), Name = "Fitness Band – Heart Rate Monitor",         Sku = "SKU-015", Currency = "INR", Amount = 140.00m },
            new() { Id = 44, Guid = Guid.Parse("20000044-0000-0000-0000-000000000044"), Name = "Fitness Band – Sleep Tracking",             Sku = "SKU-015", Currency = "EUR", Amount = 1.70m },
            new() { Id = 45, Guid = Guid.Parse("20000045-0000-0000-0000-000000000045"), Name = "Fitness Band – Waterproof Edition",         Sku = "SKU-015", Currency = "EUR", Amount = 1.80m },

            // SKU-016 — Desk Lamp
            new() { Id = 46, Guid = Guid.Parse("20000046-0000-0000-0000-000000000046"), Name = "LED Desk Lamp – Touch Control",             Sku = "SKU-016", Currency = "INR", Amount = 200.00m },
            new() { Id = 47, Guid = Guid.Parse("20000047-0000-0000-0000-000000000047"), Name = "LED Desk Lamp – Adjustable Arm",            Sku = "SKU-016", Currency = "USD", Amount = 2.40m },
            new() { Id = 48, Guid = Guid.Parse("20000048-0000-0000-0000-000000000048"), Name = "LED Desk Lamp – Study Edition",             Sku = "SKU-016", Currency = "USD", Amount = 2.60m },

            // SKU-017 — Office Chair
            new() { Id = 49, Guid = Guid.Parse("20000049-0000-0000-0000-000000000049"), Name = "Ergonomic Office Chair – Mesh",              Sku = "SKU-017", Currency = "INR", Amount = 18.00m },
            new() { Id = 50, Guid = Guid.Parse("20000050-0000-0000-0000-000000000050"), Name = "Ergonomic Office Chair – Leather",           Sku = "SKU-017", Currency = "EUR", Amount = 0.22m },
            new() { Id = 51, Guid = Guid.Parse("20000051-0000-0000-0000-000000000051"), Name = "Ergonomic Office Chair – Executive",         Sku = "SKU-017", Currency = "EUR", Amount = 0.25m },

            // SKU-018 — Standing Desk
            new() { Id = 52, Guid = Guid.Parse("20000052-0000-0000-0000-000000000052"), Name = "Standing Desk Converter – Manual",           Sku = "SKU-018", Currency = "INR", Amount = 300.00m },
            new() { Id = 53, Guid = Guid.Parse("20000053-0000-0000-0000-000000000053"), Name = "Standing Desk Converter – Gas Lift",         Sku = "SKU-018", Currency = "USD", Amount = 3.50m },
            new() { Id = 54, Guid = Guid.Parse("20000054-0000-0000-0000-000000000054"), Name = "Standing Desk Converter – Pro Edition",     Sku = "SKU-018", Currency = "USD", Amount = 3.75m },

            // SKU-019 — Monitor
            new() { Id = 55, Guid = Guid.Parse("20000055-0000-0000-0000-000000000055"), Name = "27-inch QHD Monitor – IPS Panel",            Sku = "SKU-019", Currency = "INR", Amount = 66.00m },
            new() { Id = 56, Guid = Guid.Parse("20000056-0000-0000-0000-000000000056"), Name = "27-inch QHD Monitor – Gaming",               Sku = "SKU-019", Currency = "EUR", Amount = 0.80m },
            new() { Id = 57, Guid = Guid.Parse("20000057-0000-0000-0000-000000000057"), Name = "27-inch QHD Monitor – Office Edition",       Sku = "SKU-019", Currency = "EUR", Amount = 0.85m },

            // SKU-020 — Wireless Earbuds
            new() { Id = 58, Guid = Guid.Parse("20000058-0000-0000-0000-000000000058"), Name = "Wireless Earbuds – Noise Isolation",         Sku = "SKU-020", Currency = "INR", Amount = 500.00m },
            new() { Id = 59, Guid = Guid.Parse("20000059-0000-0000-0000-000000000059"), Name = "Wireless Earbuds – Long Battery",            Sku = "SKU-020", Currency = "USD", Amount = 6.00m },
            new() { Id = 60, Guid = Guid.Parse("20000060-0000-0000-0000-000000000060"), Name = "Wireless Earbuds – Sport Edition",           Sku = "SKU-020", Currency = "USD", Amount = 6.25m }
        };
    }
}
