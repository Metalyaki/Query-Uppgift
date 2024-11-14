namespace Query_Expressions_Gruppövning
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastRestocked { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Category: {Category}, Quantity: {Quantity}, Price: {Price:C}, Last Restocked: {LastRestocked:d}";
        }
    }

    class Program
    {
        static List<Product> inventory;

        static void Main(string[] args)
        {
            InventoryFileGenerator inventoryFileGenerator = new InventoryFileGenerator();
            inventoryFileGenerator.GenerateInventoryFile("inventory.txt", 5000);
            var prod = LoadInventoryData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Lista alla produkter i kategorin \"Verktyg\" sorterade efter pris (stigande).");
                Console.WriteLine("2. Hitta de 5 produkter som har lägst lagersaldo och behöver beställas.");
                Console.WriteLine("3. Beräkna det totala värdet av alla produkter i lager.");
                Console.WriteLine("4. Gruppera produkterna efter kategori och visa antalet produkter i varje kategori. ");
                Console.WriteLine("5. Hitta alla produkter som inte har blivit påfyllda de senaste 30 dagarna.");
                Console.WriteLine("6. Öka priset med 10% för alla produkter i kategorin \"Elektronik");
                Console.WriteLine("7. Skapa en lista med produktnamn och dess lagervärde (pris * kvantitet) för produkter med ett lagervärde över 900K.");
                Console.WriteLine("8. Hitta den kategori som har det högsta genomsnittliga priset per produkt. ");
                Console.WriteLine("9. END");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        // Lista alla produkter i kategorin "Verktyg" sorterade efter pris (stigande). 
                        Console.WriteLine("Listar alla produkter i verktyg efter pris, stigande");
                        ListProductsAfterPrice();
                        Console.ReadKey();
                        break;
                    case "2":
                        //Hitta de 5 produkter som har lägst lagersaldo och behöver beställas 
                        Console.WriteLine("Hitta de 5 produkter som har lägst lagersaldo och behöver beställas");
                        DisplayProductsToOrder();
                        Console.ReadKey();
                        break;
                    case "3":
                        //Beräkna det totala värdet av alla produkter i lager.
                        Console.WriteLine("Totala värdet av alla produkter");
                        TotalWorthOfAllProducts();
                        Console.ReadKey();
                        break;
                    case "4":
                        // Gruppera produkterna efter kategori och visa antalet produkter i varje kategori. 
                        Console.WriteLine("Antal produkter per kategori");
                        GroupProducts();
                        Console.ReadKey();
                        break;
                    case "5":
                        // Hitta alla produkter som inte har blivit påfyllda de senaste 30 dagarna.
                        Console.WriteLine("Hittar alla produkter som ej blivit påfyllda senaste 30 dagarna");
                        GetLastRestocked();
                        Console.ReadKey();
                        break;
                    case "6":
                        //Öka priset med 10% för alla produkter i kategorin "Elektronik" 
                        Console.WriteLine("Ökar priset på elektronik med 10%");
                        RaisePricesForElectronics();
                        Console.ReadKey();
                        break;
                    case "7":
                        //Skapa en lista med produktnamn och dess lagervärde (pris * kvantitet) för produkter med ett lagervärde över.
                        ProductsInventoryValueOver();
                        Console.ReadKey();
                        break;
                    case "8":
                        // Hitta den kategori som har det högsta genomsnittliga priset per produkt.
                        Console.WriteLine("Hittar kategori med högsta genomsnittspris");
                        HighestPriceCat();
                        Console.ReadKey();
                        break;
                    case "9":
                        System.Environment.Exit(0);
                        break;
                        
                }
            }
        }

        static List<Product> LoadInventoryData()
        {
            string[] lines = File.ReadAllLines("inventory.txt");
            inventory = lines.Skip(1) // Hoppa över rubrikraden
                            .Select(line =>
                            {
                                var parts = line.Split(',');
                                return new Product
                                {
                                    Id = int.Parse(parts[0]),
                                    Name = parts[1],
                                    Category = parts[2],
                                    Quantity = int.Parse(parts[3]),
                                    Price = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                                    LastRestocked = DateTime.ParseExact(parts[5], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                };
                            }).ToList();
            return inventory;
        }
        static void ListProductsAfterPrice() // Pontus
        {
            var productPrice = from p in inventory
                               where p.Category == "Verktyg"
                               orderby p.Price ascending
                               select p;

            foreach (var price in productPrice)
            {
                Console.WriteLine($"Produkt: {price.Name} Pris: {price.Price}");
            } 
        }
        
        static void GetLastRestocked() // Pontus
        {
            var lastRestocked = DateTime.Now.AddDays(-30);
            var restockedProducts = from p in inventory
                                    where p.LastRestocked >= lastRestocked
                                    select p;

            foreach (var product in restockedProducts)
            {
                Console.WriteLine($"Produkt: {product.Name} Påfylld: {product.LastRestocked}");
            }
        }
    
        static void DisplayProductsToOrder() // Alexander
        {
            //Hitta de 5 produkter som har lägst lagersaldo och behöver beställas 
            var productsToOrder = inventory
                .OrderBy(p => p.Quantity)
                .Take(5);

            foreach (var product in productsToOrder)
            {
                Console.WriteLine($"Produkt: {product.Name}, Kvantitet: {product.Quantity}");
            }
            Console.ReadLine();
        }

        static void RaisePricesForElectronics() // Alexander
        {
            //Öka priset med 10% för alla produkter i kategorin "Elektronik" 
            var productsToRaisePrices = inventory.Where(p => p.Category == "Elektronik");
            foreach (var product in productsToRaisePrices)
            {
                product.Price *= 1.1m; //Ökar priset med 10%
                Console.WriteLine($"Produkt: {product.Name}, Nytt pris: {product.Price:C}");
            }
            Console.ReadLine();
        }

        public static void TotalWorthOfAllProducts()//Anders
        {
            //Beräkna det totala värdet av alla produkter i lager.
            decimal totalWorthOfAllProducts = 0;

            var productWorth = from product in inventory
                               select product.Quantity * product.Price;

            foreach (var p in productWorth)
            {
                totalWorthOfAllProducts += p;
            }
            
            Console.WriteLine(totalWorthOfAllProducts.ToString("0,0 KR", CultureInfo.InvariantCulture));
            
        }

        public static void ProductsInventoryValueOver()//Anders
        {

            //Skapa en lista med produktnamn och dess lagervärde (pris * kvantitet) för produkter med ett lagervärde över
            var productsOver = from product in inventory
                               where (product.Price * product.Quantity) > 900000
                               select product;
                                         
            List<Product> ProductsInventoryValueOver1000 = productsOver.ToList();
            Console.WriteLine("Produktnamn:");
            foreach (var item in ProductsInventoryValueOver1000)
            {
                Console.WriteLine(item.Name);
            }

        }

        static void GroupProducts() // Alfons Newberg
        {
            // Gruppera produkterna efter kategori och visa antalet produkter i varje kategori. 
            var groupProducts = from gP in inventory
                                group gP by gP.Category into g
                                orderby g.Key
                                select new
                                {
                                    Kategori = g.Key,
                                    Antal =  g.Count()
                                };

            foreach (var gp in groupProducts)
            {
                Console.WriteLine(gp);
            }

        }
        static void HighestPriceCat() // Alfons Newberg
        {
            // Hitta den kategori som har det högsta genomsnittliga priset per produkt.

            var findHighestPriceCat = (from product in inventory
                                      group product by product.Category into h

                                      select new
                                      {
                                          Category = h.Key,
                                          AvgPrice = h.Average(p => p.Price)

                                      }).OrderByDescending(c => c.AvgPrice)
                                       .First();

            Console.WriteLine($"Kategori med högsta genomsnittspris per produkt: {findHighestPriceCat.Category}");
            Console.WriteLine($"Genomsnittligt pris: {findHighestPriceCat.AvgPrice:C}");

        }
    }
}