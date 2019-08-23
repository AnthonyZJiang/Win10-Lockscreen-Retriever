using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LockscreenWallpaperRetriever
{
    public class ImageCopier
    {
        private readonly IEnumerable<FileInfo> _imageToCopy;
        private readonly string _destinationFolder;

        public ImageCopier(IEnumerable<FileInfo> imageToCopy, string destination)
        {
            _destinationFolder = destination;
            _imageToCopy = imageToCopy;
        }

        public void CopyMissingImages()
        {
            foreach (var image in _imageToCopy)
            {
                var filename = GetCopyToFileName(image);
                image.CopyTo(filename, true);
            }
        }

        private string GetCopyToFileName(FileSystemInfo fileInfo)
        {
            return Path.Combine(_destinationFolder, fileInfo.Name + ".jpg");
        }
    }
}
