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
            Console.WriteLine("Walkman cleanup Utility");
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
                 Task.Run(() => HandleWalkmanRootPath(targetDirectory));
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Load the directory given in the path as arg for any playlist
        /// </summary>
        /// <param name="targetDirectory"></param>
        public async static Task HandleWalkmanRootPath(string targetDirectory)
        {
            var playlists = Directory.GetFiles(targetDirectory, "*.M3U8");
            if (playlists.Any())
            {
                var filepaths = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories).Select(f => Path.GetRelativePath(targetDirectory, f));

                var walkmanMusicList = await GetSong(playlists);

                //Get clean list
                var cleanList = filepaths.Where(m => !walkmanMusicList.Any(m2 => m == m2));

                Console.WriteLine("==========");
                Console.WriteLine("Summary for {0}", targetDirectory);
                Console.WriteLine("Files found: {0}", filepaths.Count());
                Console.WriteLine("Files pending for removal: {0}", cleanList.Count());
                Console.WriteLine("Files left after removal: {0}", filepaths.Count() - cleanList.Count());

                //Debug
                foreach (var music in cleanList)
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
        private async static Task<List<string>> GetSong(string[] playlists)
        {
            var songList = new List<string>();
            foreach (var playlist in playlists)
            {
                songList.AddRange(await Task.Run(() => PlaylistBLL.GetSongFromList(playlist)));
            }
            return songList;
        }
    }
}
