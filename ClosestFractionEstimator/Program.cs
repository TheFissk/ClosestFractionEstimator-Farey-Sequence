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

            Console.WriteLine("Enter desired number less than 1, or pi / e for those irrationals. I'll run for 10s or until I find the exact value");
            var input = Console.ReadLine();

            if (input == "pi") value = Math.PI-3;
            else if (input == "e") value = Math.E-2;
            else value = double.Parse(input);

            Fraction Upper = new Fraction(1, 1);
            Fraction Lower = new Fraction(0, 1);
            Fraction Next;
            bool ExistingIsUpper;
            bool ExactMatch = false;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 10000 && !ExactMatch)
            {
                ExistingIsUpper = false;
                Next = FareySequencer.NextFraction(Lower, Upper);
                if (Next.DoubleRepresentation == value)
                {
                    Console.WriteLine(Next.StringRepresentation);
                    Console.WriteLine("Exact Match");
                    ExactMatch = !ExactMatch;
                }else if (Next.DoubleRepresentation > value)
                {
                    Upper = Next;
                } else
                {
                    Lower = Next;
                    ExistingIsUpper = true;
                }
                if (ExistingIsUpper)
                {
                    if ((Upper.DoubleRepresentation - value) > (value - Lower.DoubleRepresentation)) Console.WriteLine(Lower.StringRepresentation);
                } else
                {
                    if ((Upper.DoubleRepresentation - value) < (value - Lower.DoubleRepresentation)) Console.WriteLine(Upper.StringRepresentation);
                }
            }
            Console.WriteLine("done");
            Console.Read();
        }
    }


    class FareySequencer
    {
        public static Fraction NextFraction (Fraction LowerFraction, Fraction upperFraction)
        {
            int newNumerator = LowerFraction.Numerator + upperFraction.Numerator;
            int newDenominator = LowerFraction.Denominator + upperFraction.Denominator;

            int GCD = newNumerator;
            int i = newDenominator;
            int oldI;
            while (i != 0)
            {
                oldI = i;
                i = GCD % i;
                GCD = oldI;
            }

            return new Fraction(newNumerator / GCD, newDenominator / GCD);
        }
    }

    struct Fraction
    {
        public int Numerator;
        public int Denominator;

        private double fNumerator;
        private double fDenominator;

        public Fraction(int numerator, int denominator)
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
