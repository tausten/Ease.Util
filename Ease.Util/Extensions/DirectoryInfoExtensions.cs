//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.IO;

namespace Ease.Util.Extensions
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Creates a uniquely named zero-byte file on disk under the DirectoryInfo's directory and returns the 
        /// full path of that file. WARNING: The caller is responsible for cleaning up this file when done.
        /// </summary>
        /// <param name="directoryInfo">The DirectoryInfo for the directory in which to create the file.</param>
        /// <param name="maxAttempts">[optional] The maximum number of attempts to make to create the file. 
        /// Default is unlimited.</param>
        /// <returns>The path to the new zero-byte temporary file.</returns>
        public static string GetTempFileName(this DirectoryInfo directoryInfo, int maxAttempts = 0)
        {
            if (null == directoryInfo)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            IOException lastException = null;
            var attemptCount = 0;
            FileInfo result = null;
            while (null == result && (maxAttempts <= 0 || attemptCount < maxAttempts))
            {
                ++attemptCount;

                // Ensure the full path to the directory exists before trying to create files under it...
                Directory.CreateDirectory(directoryInfo.FullName);

                try
                {
                    var candidate = Path.Combine(directoryInfo.FullName, Path.GetRandomFileName());
                    // Intentionally using an explicit exists check the further narrow the race window 
                    // before attempting the actual create.
                    if (!File.Exists(candidate))
                    {
                        using (var fs = new FileStream(candidate, FileMode.CreateNew))
                        {
                            fs.Flush();
                        }
                        result = new FileInfo(candidate);
                    }
                }
                catch (IOException ex)
                {
                    // Try again with different file.
                    lastException = ex;
                }
            }

            return result?.FullName 
                ?? throw new IOException(
                    $"Unable to create a new temporary file under [{directoryInfo.FullName}] after [{attemptCount}] attempts.",
                    lastException);
        }
    }
}
