using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace LockscreenWallpaperRetriever.Checksum
{
    public class Checksum
    {
        public static string Generate(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string[] Merge(params string[][] checksums)
        {
            var list = checksums[0];
            for (var i = 1; i < checksums.Length; i++)
            {
                list = list.Union(checksums[i]).ToArray();
            }
            return list;
        }
    }
}