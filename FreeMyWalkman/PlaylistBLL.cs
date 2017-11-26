﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        public static List<string> GetSongList(string playListFile)
        {
            var songList = new List<string>();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(playListFile))
                {
                    string line;
                    while((line = sr.ReadLine()) != null)
                    {
                        if (!line.StartsWith("#"))
                        {
                            songList.Add(line);
                        }
                    }
                }
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
