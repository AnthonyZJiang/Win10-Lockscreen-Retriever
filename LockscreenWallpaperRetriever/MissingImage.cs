using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LockscreenWallpaperRetriever
{
    public class MissingImage
    {
        private readonly Checksum _checksum;
        private readonly IReadOnlyList<FileInfo> _allFileInfos;
        private readonly IReadOnlyList<string> _allFileChecksums;

        public MissingImage(Checksum checksum, IReadOnlyList<FileInfo> allFileInfos)
        {
            _checksum = checksum;
            _allFileInfos = allFileInfos;
            _allFileChecksums = GetChecksumForAllFileInfos();
            GetMissingImages();
        }

        public IReadOnlyCollection<string> MissingImageChecksum { get; private set; }
        public IReadOnlyCollection<FileInfo> MissingImageFileInfos { get; private set; }

        private void GetMissingImages()
        {
            var existingImageChecksums = _checksum.GetCurrentImageChecksums();

            if (existingImageChecksums.Length == 0)
            {
                SetAllFileMissing();
            }
            else
            {
                SelectAndSetMissingImages(existingImageChecksums);
            }
        }

        private void SetAllFileMissing()
        {
            MissingImageFileInfos = _allFileInfos;
            MissingImageChecksum = _allFileChecksums;
        }

        private void SelectAndSetMissingImages(string[] existingImageChecksums)
        {
            var missingImageFileInfos = new List<FileInfo>();
            var missingImageChecksum = new List<string>();
            for (var i = 0; i < _allFileChecksums.Count; i++)
            {
                if (existingImageChecksums.Contains(_allFileChecksums[i]))
                {
                    continue;
                }
                missingImageFileInfos.Add(_allFileInfos[i]);
                missingImageChecksum.Add(_allFileChecksums[i]);
            }

            MissingImageFileInfos = missingImageFileInfos;
            MissingImageChecksum = missingImageChecksum;
        }

        private string[] GetChecksumForAllFileInfos()
        {
            return _allFileInfos.Select(x => Checksum.GetChecksum(x.FullName)).ToArray();
        }
    }
}
