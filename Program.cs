/*
 *  Console application to calculate the Shannon Entropy of an entire file.
 *  Displays the number of bits of entropy of all bytes of the file.
 *  
 *  The closer the value is to 8, the more entropy is contained in the file
 *  
 *     J.Azar 2017
 *     
 *   http://en.wikipedia.org/wiki/Entropy_(information_theory) 
 */

using System;
using System.IO;

namespace JEntropy
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                Console.WriteLine("Usage:  JEntropy filename");

            string fname = args[0];
            if (File.Exists(fname))
            {
                DateTime tstart = DateTime.Now;
                Console.Write("Entropy = " + ShannonEntropy(fname));
                DateTime tend = DateTime.Now;
                TimeSpan t = tend.Subtract(tstart);
                Console.Write("     time: " + t.TotalSeconds.ToString("0.000") + " seconds ");
            }
            else
                Console.Write("File does not exist: " + fname);
        }

/// <summary>
/// computes entropy for a file
/// </summary>
/// <param name="filename">string filename (full path)</param>
/// <returns>bits of entropy in all bytes of the file</returns>
        public static double ShannonEntropy(string filename)
        {
            int bufferLength = 65536;  // read and process data in 64K chunks
            long[] map = new long[256];

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read,FileShare.None,bufferLength);

            long flength = fs.Length;
            
            Byte[] fBuffer = new Byte[bufferLength];
              
            int count = 0;
            do
            {
                count = fs.Read(fBuffer, 0, bufferLength);  // fill buffer from file
                if (count <= 0)     // got nothing, EOF, done
                    break;
 
                for (int i = 0; i < count; i++)  // iterate through buffer and update map counts
                    map[fBuffer[i]]++;

            } while (true);

            fs.Close();

            double result = 0.0;
            double frequency;

            for (int i = 0; i<256; i++)
            {
            if (map[i] > 0)
                {
                    frequency = (double)map[i] / flength;
                    result -= frequency * (Math.Log(frequency) / Math.Log(2));
                }                 
            }
           
            return result;
        }
    }
}
