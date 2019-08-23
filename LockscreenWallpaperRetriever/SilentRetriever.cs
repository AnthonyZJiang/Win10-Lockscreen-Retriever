using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockscreenWallpaperRetriever.Checksum;

namespace LockscreenWallpaperRetriever
{
    public class SilentRetriever
    {
        private string _imageFolderPath;
        public SilentRetriever(string destinationImageFolderPath)
        {
            _imageFolderPath = destinationImageFolderPath;
        }

        public void Retrieve()
        {
            var folderManager = new WallpaperFolderManager(_imageFolderPath);
            folderManager.CombineDuplicates();

            var bingImageFinder = new BingImageFinder();
            var imgInfos = bingImageFinder.GetImageInfos();
            folderManager.CopyNewWallpaper(imgInfos);
        }
    }
}
