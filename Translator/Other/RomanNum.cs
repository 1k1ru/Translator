using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator.Other
{
    public class RomanNum
    {
        private static readonly Dictionary<char, int> RomanDict = new Dictionary<char, int>
        {
            { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 }
        };

        public static int ToArabic(string romanNum)
        {
            foreach (var c in romanNum)
            {
                if (!RomanDict.Keys.Contains(c))
                    throw new Exception("input roman number contains invalid character");
            }

            int arabic = 0;
            int last = 0;
            for (int i = romanNum.Length - 1; i >= 0; i--)
            {
                int curr = RomanDict.GetValueOrDefault(romanNum[i]);
                if (curr >= last)
                    arabic += curr;
                else
                    arabic -= curr;
                last = curr;
            }

            return arabic;
        }
    }
}