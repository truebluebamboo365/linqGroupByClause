using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace LinqGroupBy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SalePerson> sale = new List<SalePerson>
            {
               new SalePerson { Id = 1, Name="Quyen" },
               new SalePerson { Id = 2, Name="Cuc" },
               new SalePerson { Id = 3, Name="Dorothy" },
               new SalePerson { Id = 4, Name="Angela" },
               new SalePerson { Id = 5, Name="Xavier" }
            };

            SalePerson[] person = new SalePerson[]
            {
               new SalePerson { Id = 1, Name="Quyen" },
               new SalePerson { Id = 2, Name="Cuc" },
               new SalePerson { Id = 3, Name="Dorothy" },
               new SalePerson { Id = 4, Name="Angela" },
               new SalePerson { Id = 5, Name="Xavier" }
            };

            Commission[] comm = new Commission[]
            {
               new Commission { PersonId = 1, Quantity=1, SaleDate= DateTime.Now },
               new Commission { PersonId = 2, Quantity=2, SaleDate= DateTime.Now },
               new Commission { PersonId = 3, Quantity=3, SaleDate= DateTime.Now },
               new Commission { PersonId = 4, Quantity=4, SaleDate= DateTime.Now },
               new Commission { PersonId = 5, Quantity=5, SaleDate= DateTime.Now },
               new Commission { PersonId = 1, Quantity=1, SaleDate= DateTime.Now },
               new Commission { PersonId = 2, Quantity=2, SaleDate= DateTime.Now },
               new Commission { PersonId = 3, Quantity=3, SaleDate= DateTime.Now },
               new Commission { PersonId = 4, Quantity=4, SaleDate= DateTime.Now },
               new Commission { PersonId = 5, Quantity=5, SaleDate= DateTime.Now }
            };

            //CheckWebsite();

            int[] arInt = new int[] { 10, -2, -5, 1, 3, 4, 7, 8, 2 };
            int MinPositiveNum = GetMinPositiveNumber(arInt);
            Console.WriteLine("Min missing number is {0} ", MinPositiveNum   );

            var sl = comm.Where((x, index) => index % 2 == 0).ToList(); //        <== Find returns ONE record only
            foreach (var tmp in sl)
                Console.WriteLine(tmp.PersonId);
            Console.WriteLine("");

            SalePerson sp = sale.Find(x => x.Id == 1); //        <== Find returns ONE record only
            Console.WriteLine(sp.Name);

            Console.WriteLine("");

            var sort = comm.OrderByDescending(x => x.PersonId).ToList();
            foreach (var c in sort)
            {
                Console.WriteLine(c.PersonId);
            }

            var cm = comm.Where(x => x.PersonId > 1).ToList();
            foreach (var c in cm)
            {
                Console.WriteLine(c.PersonId);
            }
            //if we want to deal with it in the foreach loop
            var myOrder = from c in comm
                          group c by c.PersonId;//NOT group c.Quantity

            foreach (IGrouping<int, Commission> g in myOrder)
            {
                Console.WriteLine(g.Key);
                foreach (Commission c in g)
                    Console.WriteLine("\t{0}", c.Quantity);
            }

            //if we try to group inside the query
            var commision0 = from c in comm
                             group c.Quantity by c.PersonId
                            into s
                             select new
                             {
                                 PersonId = s.Key,
                                 TotalQty = s.Sum(),
                                 Count = s.Count()
                             };

            foreach (var com in commision0)
            {
                Console.WriteLine("Name: {0} has {1} orders with total of {2} quantities",
                    com.PersonId, com.Count, com.TotalQty);
            }


            var commision = from p in person
                            join c in comm on p.Id equals c.PersonId
                            group c.Quantity by p.Name//c.PersonId can also be the key
                            into s
                            select new
                            {
                                PersonName = s.Key,
                                TotalQty = s.Sum(),
                                Count = s.Count()
                            };

            foreach (var com in commision)
            {
                Console.WriteLine("Name: {0} has {1} orders with total of {2} quantities",
                    com.PersonName, com.Count, com.TotalQty);
            }

            /*
            Fruit[] data = new Fruit[]
            {
               new Fruit { Name="Fugi", Type="Apple", Price=0.75m, Quantity=10 },
               new Fruit { Name="Chinese apple", Type="Apple", Price=0.80m, Quantity=7 },
               new Fruit { Name="Dau", Type="Strawberries", Price=1.90m, Quantity=20 }
            };

            //group all type together
            var group = from f in data
                        group f by f.Type;

            foreach (IGrouping<string, Fruit> f in group)
            {
                Console.WriteLine("Type is {0}", f.Key );
                foreach(Fruit fruit in f)
                {
                    Console.WriteLine("\t{0}", fruit.Name);
                }
            }

            // sum all price of the same type together

            var query = from f in data
                        group f.Price * f.Quantity by f.Type
                        into grouped
                        select new
                        {
                            Name = grouped.Key,
                            Price = grouped.Sum(),
                            Total = grouped.Count()
                        };

            foreach(var f in query)
            {
                Console.WriteLine(f.Price);

            }
            */
            //IEnumerable<IGrouping<string, Fruit>> grouped =
            //        data.GroupBy(record => record.Type);

            //var grouped = from fruit in data
            //              group fruit by fruit.Type;

            //foreach (IGrouping<string, Fruit> group in grouped)
            //{
            //    Console.WriteLine("Key is {0}", group.Key);

            //    foreach (Fruit fruit in group)
            //    {
            //        Console.WriteLine("\t{0}", fruit.Name);
            //    }
            //}

            // var query =
            //from fruit in data
            //group fruit.Price * fruit.Quantity by fruit.Type
            //  into grouped
            //select new
            //{
            //    Name = grouped.Key,
            //    Total = grouped.Sum(),
            //    Entries = grouped.Count()
            //};

            // foreach (var group in query)
            // {
            //     Console.WriteLine(group);
            // }
        }

        private static int GetMinPositiveNumber(int[] arInt)
        {
            int iResult = 0;// if zero means not found
            //int min = arInt.Where(x => x > 0).Min();
            //int max = arInt.Where(x => x > 0).Max();

            int min = 1;
            int max = arInt.Where(x => x > 0).Max();// in case they are all negative!

            for (int i = min; i < max; i++)
            {
                if (!arInt.Contains(i)) // <-- I was working on this line. The "!" is the key! 
                {
                    iResult = i;
                    break;
                }
            }

            return iResult;
        }

        static async void CheckWebsite()
        {
            HttpClient client = new HttpClient();
            var checkingResponse = await client.GetAsync("https://portal.apvma.gov.au/");
            if (!checkingResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Offline");
            }
            else
            {
                Console.WriteLine("Online");
            }
        }
    }

    class Fruit
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    class SalePerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    class Commission
    {
        public int PersonId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
    }

}
