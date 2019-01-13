using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

//Брижак Андрей Домашняя работа 4 C# продвинутый
//Дана коллекция List<T>, требуется подсчитать, сколько раз каждый элемент встречается в
//данной коллекции.

namespace Task02a_GenericCollectionInt
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> listiInts = new List<int>(); 
            Random Rnd = new Random();
            for (int i = 0; i < 100; i++)
                listiInts.Add(Rnd.Next(0, 100));

            var aFindPairs = FindPairs(listiInts) ?? throw new ArgumentNullException("FindPairs(listiInts)");

            Dictionary<int, int> FindPairs(List<int> list)
            {
                Dictionary<int, int> resInts = new Dictionary<int, int>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (resInts.ContainsKey(list[i]))
                    {
                        resInts[list[i]]++;
                    }
                    else
                    {
                        resInts.Add(list[i],1);
                    }
                   
                }

                return resInts;
            }

            foreach (var item in aFindPairs)
            {
               System.Console.WriteLine(item); 
            }

            Console.ReadKey();


        }

        
    }
}
