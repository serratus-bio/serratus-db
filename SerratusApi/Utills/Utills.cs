using System;
using System.Linq;

namespace SerratusApi.Utills
{
    public static class Utills
    {
        public static (int low, int high) parseQueryParameterRange(string range)
        {
            if (string.IsNullOrEmpty(range))
            {
                throw new ArgumentException();
            }
            var subrange = range.Substring(1, range.Length - 2);
            var numbers = subrange.Split("-").ToList();

            if (numbers.Count != 2)
            {
                throw new ArgumentException();
            }

            var low = Int16.Parse(numbers[0]);
            var high = Int16.Parse(numbers[1]);

            return (low, high);

        }
    }
}