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

            // Implementera query expressions här
            //GroupProducts();
            HighestPriceCat();

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