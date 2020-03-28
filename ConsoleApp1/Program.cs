using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using WatsonDedupe;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool readIndex = false;
            DedupeLibrary l;
            bool r;

            int numObjects;
            int numChunks;
            long logicalBytes;
            long physicalBytes;
            decimal dedupeRatioX;
            decimal dedupeRatioPercent;

            if (readIndex)
            {
                l = new DedupeLibrary("ff.idx", WriteChunk, ReadChunk, DeleteChunk, false, false);
            }
            else
            //{
            //    l = new DedupeLibrary("ff.idx", 2048, 2048, 1, 4, WriteChunk, ReadChunk, DeleteChunk, false, false);
            //    byte[] d = File.ReadAllBytes(@"c:\Users\bsv79\source\repos\ConsoleApp1\ConsoleApp1\test_data\001.bin");
            //    List<Chunk> c = new List<Chunk>();
            //    r = l.StoreObject("test", d, out c);
            //    r = l.StoreObject("test1", d, out c);
            //}
            {
                l = new DedupeLibrary("ff.idx", 2048, 2048, 0, 4, WriteChunk, ReadChunk, DeleteChunk, false, false);
                l.Database.StartTransaction();
                List<Chunk> c = new List<Chunk>();
                FileStream f = File.OpenRead(@"c:\Users\bsv79\source\repos\ConsoleApp1\ConsoleApp1\test_data\001.bin");
                byte[] d = new byte[2048];
                int h = f.Read(d, 0, 2048);
                while (h == 2048)
                {
                    r = l.StoreObject("test_" + f.Position, d, out c);
                    h = f.Read(d, 0, 2048);

                }
                l.Database.EndTransaction();
            }

            r = l.IndexStats(out numObjects, out numChunks, out logicalBytes, out physicalBytes, out dedupeRatioX, out dedupeRatioPercent);

            Console.WriteLine("numObjects: {0}, numChunks: {1}, logicalBytes: {2}, physicalBytes: {3}, dedupeRatioX: {4}, dedupeRatioPercent: {5}", numObjects, numChunks, logicalBytes, physicalBytes, dedupeRatioX, dedupeRatioPercent);

            Console.WriteLine("res: {0}", r);
        }

        static bool WriteChunk(Chunk data)
        {
            //Directory.CreateDirectory("Chunks");

            //File.WriteAllBytes("Chunks\\" + data.Key, data.Value);
            return true;
        }

        // Called during read operations
        static byte[] ReadChunk(string key)
        {
            return File.ReadAllBytes("Chunks\\" + key);
        }

        // Called during delete operations
        static bool DeleteChunk(string key)
        {
            File.Delete("Chunks\\" + key);
            return true;
        }

    }
}
