using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FreeMyWalkman
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //No arguments given
                Console.WriteLine("You need to provide at least a directory as argument.");
                return;
            }

            //Add Paths
            var folders = new List<string>();
            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {

                }
                else
                {
                    if (Directory.Exists(arg))
                    {
                        //Add path only if exists
                        folders.Add(arg);
                    }
                    else
                    {
                        Console.WriteLine($"Directory: `{arg}` is not found.");
                    }
                }
            }

        }
    }
}
