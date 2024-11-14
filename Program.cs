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
                Console.WriteLine("7. Skapa en lista med produktnamn och dess lagervärde (pris * kvantitet) för produkter med ett lagervärde över 900 000KR.");
                Console.WriteLine("8. Hitta den kategori som har det högsta genomsnittliga priset per produkt. ");
                Console.WriteLine("9. END");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":


                        Console.ReadKey();
                        break;
                    case "2":


                        Console.ReadKey();
                        break;
                    case "3":
                        //Beräkna det totala värdet av alla produkter i lager.
                        Console.WriteLine("[Total of all products in stock]");
                        TotalWorthOfAllProducts();
                        Console.ReadKey();
                        break;
                    case "4":

                        Console.ReadKey();
                        break;
                    case "5":

                        Console.ReadKey();
                        break;
                    case "6":

                        Console.ReadKey();
                        break;
                    case "7":
                        //Skapa en lista med produktnamn och dess lagervärde (pris * kvantitet) för produkter med ett lagervärde över.
                        ProductsInventoryValueOver1000();
                        Console.ReadKey();
                        break;
                    case "8":

                        Console.ReadKey();
                        break;
                    case "9":
                        System.Environment.Exit(0);
                        break;
                        

                }


            }

            Console.ReadLine();
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
    }
}
        static void GroupProducts()
        {
            // Gruppera produkterna efter kategori och visa antalet produkter i varje kategori. Alfons Newberg
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
        static void HighestPriceCat()
        {
            // Hitta den kategori som har det högsta genomsnittliga priset per produkt. Alfons Newberg

            var findHighestPriceCat = (from product in inventory
                                      group product by product.Category into h

                                      select new
                                      {
                                          Category = h.Key,
                                          AvgPrice = h.Average(p => p.Price)

                                      }).OrderByDescending(c => c.AvgPrice) // Sort by average price in descending order
                                       .First();

            Console.WriteLine($"Kategori med högsta genomsnittspris per produkt: {findHighestPriceCat.Category}");
            Console.WriteLine($"Genomsnittligt pris: {findHighestPriceCat.AvgPrice:C}");

        }
}
}