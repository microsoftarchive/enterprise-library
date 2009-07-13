//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// 
    /// </summary>
    public class RollingFlatFilePurger
    {
        private readonly string directory;
        private readonly string baseFileName;
        private readonly int cap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="baseFileName"></param>
        /// <param name="cap"></param>
        public RollingFlatFilePurger(string directory, string baseFileName, int cap)
        {
            // TODO check params - null, consistency (cap at least one)
            if (directory == null) throw new ArgumentNullException("directory");
            if (baseFileName == null) throw new ArgumentNullException("baseFileName");
            if (cap < 1) throw new ArgumentOutOfRangeException("cap");

            this.directory = directory;
            this.baseFileName = baseFileName;
            this.cap = cap;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Purge()
        {
            var extension = Path.GetExtension(this.baseFileName);
            var searchPattern = Path.GetFileNameWithoutExtension(this.baseFileName) + "*" + extension;

            string[] matchingFiles = new string[0];
            try
            {
                matchingFiles = Directory.GetFiles(this.directory, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }

            if (matchingFiles.Length < this.cap)
            {
                // bail out early if possible
                return;
            }

            // Getting files for a file name pattern with a three chars extension would retrieve files with longer
            // extensions - these files need to be ignored.
            var sortedMatchingFilesWithTimestamps = matchingFiles
                .Where(matchingFile => Path.GetExtension(matchingFile).Equals(extension))
                .Select(matchingFile =>
                    new
                    {
                        Path = matchingFile,
                        CreationTime = GetCreationTime(matchingFile)
                    })
                .OrderBy(matchingFile => matchingFile.CreationTime).ToArray();

            var numberOfFilesToPurge = sortedMatchingFilesWithTimestamps.Length - this.cap;
            for (int i = 0; i < numberOfFilesToPurge; i++)
            {
                try
                {
                    File.Delete(sortedMatchingFilesWithTimestamps[i].Path);
                }
                catch (UnauthorizedAccessException)
                {
                    // cannot delete the file because of a permissions issue - just skip it
                }
                catch (IOException)
                {
                    // cannot delete the file, most likely because it is already opened - just skip it
                }
            }
        }

        private static DateTime GetCreationTime(string path)
        {
            try
            {
                return File.GetCreationTimeUtc(path);
            }
            catch (UnauthorizedAccessException)
            {
                // will cause file be among the first files when sorting, 
                // and its deletion will likely fail causing it to be skipped
                return DateTime.MinValue;
            }
        }
    }
}
