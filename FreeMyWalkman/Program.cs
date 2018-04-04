using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FreeMyWalkman
{
    class Program
    {
        static void Main(string[] args)
        {
            //Welcome message
            Console.WriteLine("Free My Walkman - Walkman Cleanup Utility");
            Console.WriteLine("UTOSOFT (C) 2015 - 2018");
            Console.WriteLine("=======================");

            if (args.Length == 0)
            {
                //No arguments given
                Console.WriteLine("You need to provide at least a directory as argument.");
                return;
            }

            Console.WriteLine("Paths Submitted:");

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
                        Console.WriteLine(arg);
                    }
                    else
                    {
                        Console.WriteLine($"`{arg}` - NOT Found.");
                    }
                }
            }
            Console.WriteLine();

            //Load directories
            foreach (var targetDirectory in folders)
            {
                 HandleWalkmanRootPath(targetDirectory);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Load the directory given in the path as arg for any playlist
        /// </summary>
        /// <param name="targetDirectory"></param>
        public static void HandleWalkmanRootPath(string targetDirectory)
        {
            var playlists = Directory.GetFiles(targetDirectory, "*.M3U8");
            if (playlists.Any())
            {
                var allFilePaths = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories)
                    .Where(x => Path.GetExtension(x) != ".M3U8")
                    .Select(f => Path.GetRelativePath(targetDirectory, f));

                var walkmanMusicList = GetSong(playlists);

                //Get remove list
                var removeList = allFilePaths.Where(m => !walkmanMusicList.Any(m2 => m == m2));
                var filesLeft = allFilePaths.Count() - removeList.Count();
                var filesMissing = walkmanMusicList.Where(m => !allFilePaths.Any(m2 => m == m2)).Count();

                Console.WriteLine("==========");
                Console.WriteLine("Summary for `{0}`", targetDirectory);
                Console.WriteLine("Files found: {0}", allFilePaths.Count());
                Console.WriteLine("Files reference missing: {0}", filesMissing);
                Console.WriteLine();
                Console.WriteLine("Files pending for removal: {0}", removeList.Count());
                Console.WriteLine("Files left after removal: {0}", filesLeft);
                Console.WriteLine();
                Console.WriteLine("========== VERIFY");
                Console.Write("Songs in playlist: {0} | Files Left: {1} | ", walkmanMusicList.Count, filesLeft);
                if(walkmanMusicList.Count == filesLeft + filesMissing)
                {
                    //Verified
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("VERIFIED");
                }
                else
                {
                    //Verification failed
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FAILED");
                    Console.WriteLine("You may lost access to an existing song if you proceed.");
                }
                //Change the color back
                Console.ForegroundColor = ConsoleColor.Gray;

                //Confirm for operation
                Console.Write("Are you sure you want to clean the directory? (y/n)");

                var confirm = Console.ReadKey().KeyChar;
                if (confirm == 'y')
                {
                    //Clean the directory
                    Console.WriteLine("Clean up confirmed.");
                }
                else
                {
                    //Exit current directory
                    Console.WriteLine("Clean up cancelled for `{0}`", targetDirectory);
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue... ");

                //Debug
                foreach (var music in removeList)
                {
                    //Console.WriteLine(music);
                }
            }
            else
            {
                //No Playlist in folder - Do NOT process
                Console.WriteLine("No Walkman playlist found in directory. Please make sure you are selecting the root folder of your Walkman.");
            }
        }

        /// <summary>
        /// Get all songs from playlists
        /// </summary>
        /// <param name="playlists"></param>
        /// <returns></returns>
        private static List<string> GetSong(string[] playlists)
        {
            var songList = new List<string>();
            foreach (var playlist in playlists)
            {
                songList.AddRange(PlaylistBLL.GetSongFromList(playlist));
            }
            return songList.Select(s => s).Distinct().ToList();
        }

        private static async Task<char> ReadKeyAsync()
        {
            return await Task.Run(() => Console.ReadKey().KeyChar);
        }
    }
}
