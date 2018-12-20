using System.Threading.Tasks;
using LockscreenWallpaperRetriever;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tester
{
    [TestClass]
    public class ImageFinderTester
    {
        [TestMethod]
        public async Task FindAsync_FindAll()
        {
            var imageFinder = new ImageFinder();
            var files = await imageFinder.FindAsync(false);
            Assert.IsTrue(files.Count > 0);
        }

        [TestMethod]
        public void Find_FindAll()
        {
            var imageFinder = new ImageFinder();
            var files = imageFinder.Find(false);
            Assert.IsTrue(files.Count > 0);
        }

        [TestMethod]
        public async Task FindAsync_FindRecent()
        {
            var imageFinder = new ImageFinder();
            var files = await imageFinder.FindAsync();
            Assert.IsTrue(files.Count > 0);
        }

        [TestMethod]
        public void Find_FindRecent()
        {
            var imageFinder = new ImageFinder();
            var files = imageFinder.Find();
            Assert.IsTrue(files.Count > 0);
        }
    }
}
