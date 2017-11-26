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

            //Load directories
            foreach(var targetDirectory in folders)
            {
                var playlists = Directory.GetFiles(targetDirectory, "*.M3U8");
                if (playlists.Any())
                {
                    var filenames = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories).Select(f => Path.GetRelativePath(targetDirectory, f));
                    foreach(var file in filenames)
                    {
                        Console.WriteLine("Processed file '{0}'.", file);
                    }
                    
                }
                else
                {
                    //No Playlist in folder - Do NOT process
                    Console.WriteLine("No Walkman playlist found in directory. Please make sure you are selecting the root folder of your Walkman.");
                }
            }
            Console.ReadLine();
        }
    }
}
