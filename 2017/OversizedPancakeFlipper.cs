//see problem at https://code.google.com/codejam/contest/3264486/dashboard#s=p0

using System;
using System.Collections.Generic;
using System.Linq;

namespace Oversized_Pancake_Flipper
{
    class Program
    {
        static void Main(string[] args)
        {
            int t = Int32.Parse(Console.ReadLine()); //# input lines

            for (int i = 1; i < t + 1; i++)
            {
                //input handling
                string inputStr = Console.ReadLine();
                int pivot = inputStr.IndexOf(' ');
                int flipper = Int32.Parse(inputStr.Substring(pivot + 1));
                string pancakes = inputStr.Remove(pivot); //trim off flipper width number, leaving just the pancake string

                List<int> flipTrack = new List<int>();
                List<int> results = tryFlips(pancakes, flipper, 0, 0, flipTrack);

                if (results.Count == 0)
                {
                    Console.WriteLine("Case #" + i + ": IMPOSSIBLE");
                }
                else
                {
                    Console.WriteLine("Case #" + i + ": " + results.Min());
                }
            }
        }

        //attempt all flip permutations and report the lowest flip #
        //track # flips per solution in 'total' list, then report smallest # (if any)
        static List<int> tryFlips(string pc, int width, int flips, int depth, List<int> total)
        {
            if(pc.IndexOf('-') == -1) //check if solved
            {
                total.Add(flips);
            }
            else if (width + depth <= pc.Length)
            {
                total = tryFlips(pc, width, flips, depth + 1, total);

                string postFlip = pc.Substring(0, depth);
                for (int i = depth; i < depth + width; i++)
                {
                    
                    if (pc[i].Equals('+'))
                    {
                        postFlip += '-';
                    }
                    else
                    {
                        postFlip += '+';
                    }
                }
                postFlip += pc.Substring(depth + width);
                total = tryFlips(postFlip, width, flips + 1, depth + 1, total);
            }
            return total;
        }
    }
}

/* LOGIC
 * There are x number of k-width clusters of pancakes, where x = (total pancakes) - k + 1
 * Example input:   +-+-- 3
 * Clusters:        +-+
 *                   -+-
 *                    +--
 * x = 5 - 3 + 1 = 3 clusters
 * In any solution, each cluster will be flipped 0 or 1 times. 2 flips is equivalent to 0 flips state-wise, but is a waste of flips and will give a suboptimal solution.
 * 
 * STRATEGY
 * Attempt every permutation of cluster flips. Report the smallest number of flips that led to a solved state.
 * If all permutations are attempted and we never reach a state where all pancakes are happy-side-up, the problem is IMPOSSIBLE.
 */
