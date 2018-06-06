using System;
using System.Linq;
using System.Collections.Generic;

namespace ManagingLists
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciou");

            int limitBreak = 2;

            var listItens = new List<Item>
            {
                new Item 
                { 
                    Id = 1,
                    Nome = "Celular",
                    Quantidade = 5
                }, 
                new Item 
                {
                    Id = 2,
                    Nome = "Notebook",
                    Quantidade = 4
                }
            };

            var newList = new List<Item>();
            int count = 0;

            foreach(var item in listItens)
            {                      
                count = item.Quantidade;             

                while(count > 0)
                { 
                    newList.Add(new Item {
                        Id = item.Id,
                        Nome = item.Nome,
                        Quantidade = count > limitBreak 
                            ? limitBreak 
                            : count
                    });

                    count = count - limitBreak;
                }
            }

            newList.ForEach(x => Console.WriteLine($"Final: {x.Quantidade}"));
        }
    }

    public class Item {
        public int Id {get; set;}
        public string Nome {get; set;}
        public int Quantidade {get; set;}
    }
}
