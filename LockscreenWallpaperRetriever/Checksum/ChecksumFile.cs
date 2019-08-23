using System;
using System.IO;
using System.Linq;

namespace LockscreenWallpaperRetriever.Checksum
{
    public class ChecksumFile
    {
        private readonly string _filename;
        private string[] _checksums;

        public ChecksumFile(string filename)
        {
            _filename = filename;

            if (Exists)
                ReadChecksums();
            else
                _checksums = new string[0];
        }

        public bool Exists => File.Exists(_filename);

        public string[] GetChecksums()
        {
            return (string[])_checksums.Clone();
        }

        public void Append(string[] checksums)
        {
            var cks = _checksums.ToList();
            cks.AddRange(checksums);
            _checksums = cks.ToArray();

            if (!Exists)
            {
                File.WriteAllLines(_filename, checksums);
                SetHidden();
                return;
            }
            File.AppendAllLines(_filename, checksums);
        }

        public void Overwrite(string[] checksums)
        {
            _checksums = checksums;

            File.WriteAllLines(_filename, checksums);
            SetHidden();
        }

        private void ReadChecksums()
        {
            _checksums = File.ReadAllLines(_filename);
        }

        private void SetHidden()
        {
            File.SetAttributes(_filename, FileAttributes.Hidden);
        }
    }
}
