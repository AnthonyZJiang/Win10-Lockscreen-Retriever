using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace LockscreenWallpaperRetriever
{
    public class Checksum
    {
        public const string ChecksumFileName = ".lwr";
        private readonly ImageCopier _imageCopier;

        public Checksum(ImageCopier imageCopier)
        {
            _imageCopier = imageCopier;
        }

        public string ChecksumFilename => Path.Combine(_imageCopier.ImageFolder, ChecksumFileName);

        public string[] GetCurrentImageChecksums()
        {
            return File.Exists(ChecksumFilename)
                ? File.ReadAllLines(ChecksumFilename)
                : GenerateAndWriteChecksumForExistingImages();
        }

        public string[] RegenerateChecksumFile()
        {
            return GenerateAndWriteChecksumForExistingImages();
        }

        public void AppendChecksumToFile(string[] checksums)
        {
            if (!File.Exists(ChecksumFilename))
            {
                File.WriteAllLines(ChecksumFilename, checksums);
                return;
            }
            File.AppendAllLines(ChecksumFilename, checksums);
        }

        private string[] GenerateAndWriteChecksumForExistingImages()
        {
            var checksums = GetChecksumForExistingImages();
            if (checksums.Length != 0)
            {
                File.WriteAllLines(ChecksumFilename, checksums);
                File.SetAttributes(ChecksumFilename, FileAttributes.Hidden);
            }

            return checksums;
        }

        private string[] GetChecksumForExistingImages()
        {
            var images = GetAllImagesInImageFolder();
            var existingChecksums = new string[images.Count];

            for (var i = 0; i < images.Count; i++)
            {
                existingChecksums[i] = GetChecksum(images[i].FullName);
            }

            return existingChecksums;
        }

        private List<FileInfo> GetAllImagesInImageFolder()
        {
            var images = new List<FileInfo>();
            var files = Directory.GetFiles(_imageCopier.ImageFolder);

            foreach (var file in files)
            {
                var fi = new FileInfo(file);
                if (fi.Extension == ".jpg")
                {
                    images.Add(fi);
                }
            }

            return images;
        }

        public static string GetChecksum(string filename)
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
    }
}
