using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2b_GenericCollection
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    T string str = new T();


        //    //foreach (var item in aFindPairs)
        //    //{
        //    //    System.Console.WriteLine(item);
        //    //}
        //    //Console.ReadKey();
        //}

        
    }

    public class T
    {
        Dictionary<T, int> FindPairs(List<T> list)
        {
            Dictionary<T, int> res = new Dictionary<T, int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (res.ContainsKey(list[i]))
                {
                    res[list[i]]++;
                }
                else
                {
                    res.Add(list[i], 1);
                }
            }

            return res;
        }
    }
}
