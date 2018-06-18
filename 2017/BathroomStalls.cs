//see problem at https://code.google.com/codejam/contest/3264486/dashboard#s=p2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BathroomStalls
{
    class Program
    {
        static void Main(string[] args)
        {
            int t = Int32.Parse(Console.ReadLine());

            for(int i = 1; i < t + 1; i++)
            {
                string inputStr = Console.ReadLine();
                int space = inputStr.IndexOf(' ');
                long n = Int64.Parse(inputStr.Substring(0, space));
                long k = Int64.Parse(inputStr.Substring(space));

                List<Stall> bathroom = new List<Stall>(); //create bathroom (list of stalls) and install guards
                Stall guardL = new Stall(0);
                guardL.nearL = guardL;
                Stall guardR = new Stall(n + 1);
                guardR.nearR = guardR;
                guardL.makeNeighbors(guardR);
                bathroom.Add(guardL);
                bathroom.Add(guardR);

                for(int p = 0; p < k; p++) //occupy stalls
                {
                    long greatestWidth = 0;
                    Stall leftBound = new Stall(0); //instantiate as placeholders. their values will be replaced before the foreach loop ends
                    Stall rightBound = new Stall(0);

                    foreach(var occupied in bathroom) //find the widest gap
                    {
                        if(occupied.lDist() > greatestWidth)
                        {
                            greatestWidth = occupied.lDist();
                            leftBound = occupied.nearL;
                            rightBound = occupied;
                        }
                        if(occupied.rDist() > greatestWidth)
                        {
                            greatestWidth = occupied.rDist();
                            leftBound = occupied;
                            rightBound = occupied.nearR;
                        }
                    }

                    Stall newOccupant = new Stall(leftBound.pos + (greatestWidth + 1) / 2);
                    newOccupant.makeNeighbors(leftBound);
                    newOccupant.makeNeighbors(rightBound);
                    bathroom.Add(newOccupant);

                    if (p == k - 1)
                    {
                        Console.WriteLine("Case #" + i + ": " + Math.Max(newOccupant.lDist(), newOccupant.rDist()) + " " + Math.Min(newOccupant.lDist(), newOccupant.rDist()));
                    }
                }


            }
        }
    }

    class Stall
    {
        public long pos { get; set; }
        public Stall nearL { get; set; }
        public Stall nearR { get; set; }

        public Stall(long p)
        {
            pos = p;
        }

        public Stall(long p, Stall nL, Stall nR)
        {
            pos = p;
            nearL = nL;
            nearR = nR;
        }

        public void makeNeighbors(Stall newNeighbor)
        {
            if(this.pos < newNeighbor.pos)
            {
                nearR = newNeighbor;
                newNeighbor.nearL = this;
            }
            else
            {
                nearL = newNeighbor;
                newNeighbor.nearR = this;
            }
        }

        public long lDist()
        {
            return pos - nearL.pos - 1; //subtract 1 because adjacent stalls have 0 distance between them, as defined in the prompt
        }

        public long rDist()
        {
            return nearR.pos - pos - 1;
        }
    }
}

/*
 * Logic:
 * Each stall stores its neighbors, and occupying a new stall update its neighbors' neighbor values. Emulates a doubly linked list.
 * 
 * Strategy:
 * Starting off, stall 0 and stall N+1 are occupied.
 * Define a stall class. 
 * Stall
 *      long pos: a number indicating the stall's position
 *      Stall nearL: the stall's nearest left neighbor
 *      STall nearR: the stall's nearest right neighbor
 * At the beginning we only have two occupied stalls.
 * Store the occupied stalls in a list.
 *
 * 
 * When looking for a stall to occupy, find the pair of stalls that has the greatest space between them.
 * Calculate this, with a stall S, by performing (S - nearL) and (nearR - S). 
 * After finding the widest gap in stalls, find the midpoint between them. If the gap is an even width, choose the left-midpoint. 
 * Set this stall to occupied. Set its nearL and nearR, and update the nearL and nearR of its neighbors who define the edges of this gap. 
 * 
 * Once all the stalls are occupied, check the distance between it and its neighbors for the answer.
 */
