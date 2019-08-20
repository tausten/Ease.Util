//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ease.Util.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns a Stream of the embedded resource matching the passed <paramref name="resourceNameSuffix"/>. If there is no case-sensitive 
        /// match, then case-insensitive match (if any) will be used. If there are multiple matches, the resource with the shortest
        /// `ManifestResourceName` will be used. If there are no matches, `null` is returned.
        /// </summary>
        /// <param name="assembly">The `Assembly` to search for matching embedded resource.</param>
        /// <param name="resourceNameSuffix">The suffix of the `ManifestResourceName` to look for.</param>
        /// <returns>A `Stream` of the matching resource, or `null` if not found.</returns>
        public static Stream GetResourceBySuffixAsStream(this Assembly assembly, string resourceNameSuffix)
        {
            Stream result = null;
            if (!string.IsNullOrWhiteSpace(resourceNameSuffix))
            {
                var matchingResourceNames = assembly.GetManifestResourceNames()
                    .Where(x => x.EndsWith(resourceNameSuffix, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x)
                    .ToList();

                var matchingName = matchingResourceNames.FirstOrDefault(x => x.EndsWith(resourceNameSuffix)) ?? matchingResourceNames.FirstOrDefault();

                if (null != matchingName)
                {
                    result = assembly.GetManifestResourceStream(matchingName);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the contents of the embedded resource matching the passed <paramref name="resourceNameSuffix"/> as a string (read as the 
        /// specified <paramref name="encoding"/>). If there is no case-sensitive match, then case-insensitive match (if any) will be used. If 
        /// there are multiple matches, the resource with the shortest `ManifestResourceName` will be used. If there are no matches, `null` is returned.
        /// </summary>
        /// <param name="assembly">The `Assembly` to search for matching embedded resource.</param>
        /// <param name="resourceNameSuffix">The suffix of the `ManifestResourceName` to look for.</param>
        /// <param name="encoding">[optional]The `System.Text.Encoding` to use when reading the resource.</param>
        /// <returns>A `string` of the contents of the matching resource, or `null` if not found.</returns>
        public static string GetResourceBySuffixAsString(this Assembly assembly, string resourceNameSuffix, System.Text.Encoding encoding = null)
        {
            string result = null;
            using (var stream = assembly.GetResourceBySuffixAsStream(resourceNameSuffix))
            {
                if (null != stream)
                {
                    using (var reader = null != encoding ? new StreamReader(stream, encoding) : new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            return result;
        }
    }
}
