
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuineMcCuskeyCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Number of Variables: ");
            var bits = int.Parse(Console.ReadLine());
            var minList = new List<int>();
            var all = new List<int>();

            Console.WriteLine("Enter Necessary Minterms (space deliniated)");
            foreach (var item in Console.ReadLine().Trim().Split())
            {
                minList.Add(int.Parse(item));
            }
            minList = minList.Distinct().ToList();

            Console.WriteLine("Enter Don't-Care Minterms (space deliniated)");
            foreach (var item in Console.ReadLine().Trim().Split())
            {
                all.Add(int.Parse(item));
            }
            all = all.Distinct().ToList();
            all.AddRange(minList);

            var primeImplicants = GetNecessaryPrimeImplicants(all, bits, minList);

            foreach (var item in primeImplicants)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        static List<string> GetNecessaryPrimeImplicants(List<int> minList, int bits, List<int> necessary)
        {
            var minterms = new List<string>();

            foreach (var item in minList)
            {
                minterms.Add(Convert.ToString(item, 2).PadLeft(bits, '0'));
            }

            var primeImplicants = GetPrimeImplicants(minterms);

            //TODO: Remove ones you don't need

            return primeImplicants;
        }

        static List<string> GetPrimeImplicants(List<string> minterms)
        {
            List<string> primeImplicants;
            do
            {
                primeImplicants = minterms.Select(x => x).ToList();
                minterms.Clear();
                foreach (var i in primeImplicants)
                {
                    var temp = new List<string>();
                    foreach (var j in primeImplicants)
                    {
                        if (Valid(i, j))
                        {
                            temp.Add(Merge(i, j));
                        }
                    }

                    if (temp.Count == 0)
                    {
                        temp.Add(i);
                    }
                    minterms.AddRange(temp);
                }
                minterms = minterms.Distinct().ToList();
            } while (!Enumerable.SequenceEqual(minterms.OrderBy(x => x), primeImplicants.OrderBy(x => x)));

            return minterms;
        }

        static bool Valid(string i, string j)
        {
            var oneOff = false;
            for (var ix = 0; ix < i.Length; ix++)
            {
                if (i[ix] != j[ix])
                {
                    if (oneOff || i[ix] == '-' || j[ix] == '-')
                    {
                        return false;
                    }
                    oneOff = true;
                }
            }
            return i != j;
        }

        static string Merge(string i, string j)
        {
            var t = "";
            for (var ix = 0; ix < i.Length; ix++)
            {
                t += i[ix] == j[ix] ? i[ix] : '-';
            }
            return t;
        }
    }
}
