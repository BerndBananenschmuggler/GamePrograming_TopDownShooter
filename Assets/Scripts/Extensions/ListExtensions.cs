using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {
        private static Random _random = new Random();

        public static void Randomize<T>(this List<T> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int i = _random.Next(0, count + 1);
                T obj = list[i];
                list[i] = list[count];
                list[count] = obj;
            }
        }
    }
}
