using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosestFractionEstimator
{
    class Program
    {
        static void Main(string[] args)
        {
            double value;

            Console.WriteLine("Enter desired number less than 1, or pi / e for those irrationals.");
            var input = Console.ReadLine();

            if (input == "pi") value = Math.PI-3;
            else if (input == "e") value = Math.E-2;
            else value = double.Parse(input);

            Fraction Next = new Fraction();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int i = 0;
            while (i<1000)
            {
                Fraction Upper = new Fraction(1, 1);
                Fraction Lower = new Fraction(0, 1);
                
                bool ExactMatch = false;

                while (!ExactMatch)
                {
                    Next = FareySequencer.NextFractionEuclidian(Lower, Upper);
                    if (Next.DoubleRepresentation == value)
                    {

                        ExactMatch = !ExactMatch;
                    }
                    else if (Next.DoubleRepresentation > value)
                    {
                        Upper = Next;
                    }
                    else
                    {
                        Lower = Next;
                    }
                }
                i++;
            }
            stopwatch.Stop();
            Console.WriteLine(Next.StringRepresentation);
            Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds} milliseconds");
            Console.Read();

        }
    }


    class FareySequencer
    {
        /// <summary>
        /// Uses the Euclidian GCD Algorithm (which is slow)
        /// </summary>
        /// <param name="LowerFraction"></param>
        /// <param name="upperFraction"></param>
        /// <returns></returns>
        public static Fraction NextFractionEuclidian (Fraction LowerFraction, Fraction upperFraction)
        {
            uint newNumerator = LowerFraction.Numerator + upperFraction.Numerator;
            uint newDenominator = LowerFraction.Denominator + upperFraction.Denominator;

            uint GCD = newNumerator;
            uint i = newDenominator;
            uint oldI;
            while (i != 0)
            {
                oldI = i;
                i = GCD % i;
                GCD = oldI;
            }

            return new Fraction(newNumerator / GCD, newDenominator / GCD);
        }
        /// <summary>
        /// uses the faster Binary GCD Algorithm (Faster)
        /// </summary>
        /// <param name="LowerFraction"></param>
        /// <param name="upperFraction"></param>
        /// <returns></returns>
        public static Fraction NextFractionBinaryGCD(Fraction LowerFraction, Fraction upperFraction)
        {
            uint newNumerator = LowerFraction.Numerator + upperFraction.Numerator;
            uint newDenominator = LowerFraction.Denominator + upperFraction.Denominator;

            uint GCD = BinaryGCD(newNumerator, newDenominator);
            return new Fraction(newNumerator / GCD, newDenominator / GCD);
        }

        private static uint BinaryGCD(uint u, uint v)
        {
            if (u == v || v == 0) return u;
            if (u == 0) return v;

            if ((u & 1) == 1) //u is odd
            {
                if ((v & 1) == 0) //v is even
                {
                    return BinaryGCD(u, v >> 1);
                }
                if (u > v) //both u and v are odd
                {
                    return BinaryGCD((u - v) >> 1, v);
                }else
                {
                    return BinaryGCD((v - u) >> 1, u);
                }
            } else
            {
                if ((v & 1) == 0) //v is even, u is even
                {
                    return 2 * BinaryGCD(u >> 1, v >> 1);
                } else //v is odd, u is even
                {
                    return BinaryGCD(u >> 1, v);
                }
            }
        }
    }

    struct Fraction
    {
        public uint Numerator;
        public uint Denominator;

        private double fNumerator;
        private double fDenominator;

        public Fraction(uint numerator, uint denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            fNumerator = numerator;
            fDenominator = denominator;
        }

        public double DoubleRepresentation { get { return fNumerator / fDenominator; } }
        public string StringRepresentation { get { return (Numerator + "/" + Denominator); } }
    }
}
