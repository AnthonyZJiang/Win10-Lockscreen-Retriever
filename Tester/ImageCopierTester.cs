using LockscreenWallpaperRetriever;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tester
{
    [TestClass]
    public class ImageCopierTester
    {
        [TestMethod]
        public void TestMethod1()
        {
            var imageFinder = new ImageFinder();
            var imageFileInfos = imageFinder.Find(false);
            var imageCopier = new ImageCopier(@"G:\OneDrive\Documents\WallPapers\BingWallpaper", imageFileInfos);
            imageCopier.CopyMissingImages();
        }
    }
}
