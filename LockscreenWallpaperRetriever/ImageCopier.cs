using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LockscreenWallpaperRetriever
{
    public class ImageCopier
    {
        private readonly IReadOnlyList<FileInfo> _imageFileInfos;
        private readonly Checksum _checksum;

        public ImageCopier(string imageFolder, IReadOnlyList<FileInfo> imageFileInfos)
        {
            ImageFolder = imageFolder;
            _imageFileInfos = imageFileInfos;
            _checksum = new Checksum(this);
        }

        public string ImageFolder { get; }

        public void CopyMissingImages()
        {
            var missingImages = new MissingImage(_checksum, _imageFileInfos);

            foreach (var missingImage in missingImages.MissingImageFileInfos)
            {
                var filename = GetCopyToFileName(missingImage);
                missingImage.CopyTo(filename, true);
            }

            _checksum.AppendChecksumToFile(missingImages.MissingImageChecksum.ToArray());
        }

        private string GetCopyToFileName(FileSystemInfo fileInfo)
        {
            return Path.Combine(ImageFolder, fileInfo.Name + ".jpg");
        }
    }
}
