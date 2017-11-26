using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FreeMyWalkman
{
    /// <summary>
    /// Logic that cleans file according to playlist
    /// </summary>
    public class PlaylistBLL
    {
        /// <summary>
        /// Return all music in the playlist
        /// </summary>
        /// <param name="playListFile"></param>
        /// <returns></returns>
        public static async Task<List<string>> GetSongFromList(string playListFile)
        {
            Console.Write("Playlist: {0} - ", Path.GetFileNameWithoutExtension(playListFile));
            var songList = await Task.Run(() => ReadPlaylist(playListFile));
            return songList;
        }

        /// <summary>
        /// Read file stream
        /// </summary>
        /// <param name="playListFile"></param>
        /// <returns></returns>
        private static List<string> ReadPlaylist(string playListFile)
        {
            var songList = new List<string>();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(playListFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.StartsWith("#"))
                        {
                            songList.Add(line.Replace('/', Path.DirectorySeparatorChar)); //Replace walkman separator to System directory separator
                        }
                    }
                }
                Console.WriteLine("{0} Songs", songList.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} could not be read. May be corrupted or not a M3U8 file.", playListFile);
                Console.WriteLine(e.Message);
            }
            return songList;
        }
    }
}
