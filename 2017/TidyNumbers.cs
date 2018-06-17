//see problem at https://code.google.com/codejam/contest/3264486/dashboard#s=p1

using System;

namespace Tidy_Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            int t = Int32.Parse(Console.ReadLine()); //# input lines

            for (int i = 1; i < t + 1; i++)
            {
                string inputStr = Console.ReadLine();
                long n = Int64.Parse(inputStr); //must use longs to support 18 digits
                int powBound = (int)Math.Log10(n); //highest place value in n

                if(powBound != 0) //single digit
                {                    
                    n = makeTidy(n, powBound);          
                }

                Console.WriteLine("Case #" + i + ": " + n);
            }
        } 

        static long longPow(long n, long power)
        {
            if(power == 0)
            {
                return 1;
            }
            long result = n;
            while(power > 1)
            {
                result *= n;
                power--;
            }
            return result;
        }

        //checks if a number is tidy. if not, it will make it so
        static long makeTidy(long n, int powBound)
        {
            long[] digits = new long[powBound + 1]; // holds all digits, whose indices correspond to place value
            long sum = 0; //used to track value of digits preceding currently-inspected digit
            int z = 0;

            while(z <= powBound)
            {
                if(z == 0)
                {
                    digits[z] = n / (longPow(10, powBound));
                    z++;
                }
                else
                {
                    sum = 0;
                    for(int i = 0; i < z; i++)
                    {
                        sum += digits[i] * longPow(10, z - i);
                    }
                    digits[z] = n / (longPow(10, powBound - z)) - sum;

                    if(digits[z] < digits[z - 1] || digits[z] == 0)
                    {
                        n = (sum * longPow(10, powBound - z)) - 1;
                        z = 0;
                    }
                    else
                    {
                        z++;
                    }
                }

            }
            return n;
        }
    }
}

/*
 * Logic
 * The digit 0 cannot appear in a tidy number. The prompt specifies that there are no leading 0s, and if 0 appears anywhere it must be preceded by a digit of greater value.
 * Getting rid of the leftmost 0 will be the starting point. Removing any further-right 0s would be pointless, because the 0s to its left would still need to be removed at some point.
 * Once the leftmost 0 is found, replace all digits following it with 0 and subtract 1. If the given number is 1320405, perform 1320000 - 1 = 1319999.
 * Now begin at the leftmost digit and move right, checking that each digit is >= its preceding digit.
 * On encountering a digit that is less than its preceding digit, set it and all digits to its right to 0 and subtract 1. In the above example, 1319999 becomes 1300000 - 1 = 12999999
 * The resultant number will now have all nondecreasing digits, quite possibly with some trailing 9s.
 * 
 * Strategy
 * The array digits[] stores each digit in n
 * Use integer division to get n's digits.
 * If a digit is less than its preceding digit, set this digit and all following it to 0. 
 * To do this, use each number's index in digits[] to determine its place value, and add them together. Then subtract 1 from this number to clip the trailing 0s.
 * This will trigger another pass through the while loop surrounding these operations. The while loop will exit if a decreasing digit is never encountered, thus ensuring tidiness.
 */
