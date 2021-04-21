using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamsLab5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int stepLength = 5;
            var stream = GetStream();

            //r - max index of first non-zero bit of Hash result
            int i = 0, r = 0;
            var uniqueElements = new List<int>();
            await foreach (var value in stream)
            {
                i++;
                if (i % stepLength == 0)
                {
                    var FMResult = Math.Pow(2, r);
                    Console.WriteLine($"After {i}-th element actual count of distinct " +
                        $"elements = {uniqueElements.Count}, by FM = {FMResult}");
                }

                //add value if it is first entry
                if (!uniqueElements.Any(x => x == value))
                {
                    uniqueElements.Add(value);
                }

                var hash = Hash(value);
                //for hash = 0, r = 0 and do not affect the result
                if (hash != 0)
                {
                    //get binary representation of hash result and find index of first non-zero bit
                    var currentIndex = new string(Convert.ToString(hash, 2).Reverse().ToArray()).IndexOf("1");
                    r = currentIndex > r ? currentIndex : r;
                }
            }
        }

        public static int Hash(int value)
        {
            return (34 * value + 1) % 33;
        }

        public async static IAsyncEnumerable<int> GetStream()
        {
            //get random value [1; 32];
            int minValue = 1, maxValue = 33;

            while (true)
            {
                await Task.Delay(100);
                yield return new Random().Next(minValue, maxValue);
            }
        }
    }
}
