using System;
using System.IO;
using System.Numerics;

namespace bbs
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "./../output.txt";
            string fileName2 = "./../cryptedOutput.txt";
            File.Delete(fileName);
            File.Delete(fileName2);
            //BigInteger p = 30000000091;
            //BigInteger q = 40000000003;
            BigInteger p = 100002979;
            BigInteger q = 100003243;
            //BigInteger p = 100000004483;
            //BigInteger q = 100000004987;

            var M = p * q;
            BigInteger N = 20000;
            //BigInteger seed = 4882516701;
            BigInteger seed = new Random().Next(0, Int32.MaxValue);
            Console.WriteLine($"input values: \n p: {p}, q: {q}, M: {M}, N: {N}, seed: {seed}\noutput value:");
            var count0 = 0;
            var count1 = 0;

            for (int i = 0; i < N; i++)
            {
                seed = seed * seed % M;
                var bit = seed % 2;
                if (bit == 0)
                    count0++;
                else
                    count1++;

                File.AppendAllText(fileName, bit.ToString());
                //Console.Write(bit);
            }
            Console.WriteLine($"\nnumber of 0: {count0} \nnumber of 1: {count1}");
        }
    }
}
