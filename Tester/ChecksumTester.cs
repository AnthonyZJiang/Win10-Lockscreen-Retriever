using System.IO;
using LockscreenWallpaperRetriever;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tester
{
    [TestClass]
    public class ChecksumTester
    {
        private Checksum _checksum;
        private ImageCopier _imageCopier;
        private ImageFinder _imageFinder;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            ChecksumTestData.CreateData();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            ChecksumTestData.DeleteData();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            ChecksumTestData.CreateData();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            ChecksumTestData.DeleteData();
        }

        #region private helpers
        private void Initialise(string folder)
        {
            _imageFinder = new ImageFinder();
            var fileInfos = _imageFinder.Find();
            _imageCopier = new ImageCopier(folder, fileInfos);
            _checksum = new Checksum(_imageCopier);
        }

        private string[] GetRealChecksums()
        {
            return File.ReadAllLines(_checksum.ChecksumFilename);
        }

        private bool IsChecksumFileExist()
        {
            return File.Exists(_checksum.ChecksumFilename);
        }


        private bool IsChecksumFileHidden()
        {
            var attributes = File.GetAttributes(_checksum.ChecksumFilename);
            return (attributes & FileAttributes.Hidden) != 0;
        }

        private string[] GetTestJpegChecksums()
        {
            return new[]
            {
                "9ac5dd3ac91170e89fb5cbd2b20b28d3",
                "37aba0b59b22b54cbe4f907fcfb491ac",
                "3dff2e9113007d23e84870b20a5ae8fd"
            };
        }
        #endregion

        #region testers
        [TestMethod]
        // r1: gets an empty array
        // r2: does not create a checksum file
        public void WriteChecksumFile_NoJpeg()
        {
            Initialise(ChecksumTestData.FolderNoJpegNoCs);

            Assert.IsFalse(IsChecksumFileExist());
            var checksum = _checksum.GetCurrentImageChecksums();
            Assert.AreEqual(checksum.Length, 0);
            Assert.IsFalse(IsChecksumFileExist());
        }

        [TestMethod]
        // r1: gets a checksum array that matches those returned by GetTestJpegChecksums()
        // r2: checksums written to the file matches those returned by GetTestJpegChecksums()
        // r3: checksum file should be hidden
        public void WriteChecksumFile_WithJpegNoCs()
        {
            Initialise(ChecksumTestData.FolderWithJpegNoCs);

            Assert.IsFalse(IsChecksumFileExist());
            var checksum = _checksum.GetCurrentImageChecksums();
            CollectionAssert.AreEqual(checksum, GetTestJpegChecksums());
            CollectionAssert.AreEqual(GetRealChecksums(), GetTestJpegChecksums());
            Assert.IsTrue(IsChecksumFileHidden());
        }

        [TestMethod]
        // r1: gets a checksum array that matches those returned by GetTestJpegChecksums()
        // r2: checksums written to the file matches those returned by GetTestJpegChecksums()
        // r3: checksum file should be visible (not created by Checksum class)
        public void WriteChecksumFile_WithJpegCs()
        {
            Initialise(ChecksumTestData.FolderWithJpegCs);

            Assert.IsTrue(IsChecksumFileExist());
            var checksum = _checksum.GetCurrentImageChecksums();
            CollectionAssert.AreEqual(checksum, GetTestJpegChecksums());
            CollectionAssert.AreEqual(GetRealChecksums(), GetTestJpegChecksums());
            Assert.IsFalse(IsChecksumFileHidden());
        }

        [TestMethod]
        // r1: gets a checksum array that matches the first two strings returned by GetTestJpegChecksums()
        // r2: checksums written to the file matches the first two strings returned by GetTestJpegChecksums()
        // r3: checksum file should be visible (not created by Checksum class)
        public void WriteChecksumFile_WithJpegIncompCs()
        {
            Initialise(ChecksumTestData.FolderWithJpegIncompCs);

            Assert.IsTrue(IsChecksumFileExist());
            var checksum = _checksum.GetCurrentImageChecksums();
            var testChecksum = Utility.SubArray(GetTestJpegChecksums(), 0, 2);
            CollectionAssert.AreEqual(checksum, testChecksum);
            CollectionAssert.AreEqual(GetRealChecksums(), testChecksum);
            Assert.IsFalse(IsChecksumFileHidden());
        }

        [TestMethod]
        // r1: gets a checksum array that matches those returned by GetTestJpegChecksums()
        // r2: checksums written to the file matches those returned by GetTestJpegChecksums()
        // r3: checksum file should be hidden
        public void WriteChecksumFile_WithJpegIncompCsRegenerateCs()
        {
            Initialise(ChecksumTestData.FolderWithJpegIncompCs);

            Assert.IsTrue(IsChecksumFileExist());
            var checksum = _checksum.RegenerateChecksumFile();
            CollectionAssert.AreEqual(checksum, GetTestJpegChecksums());
            CollectionAssert.AreEqual(GetRealChecksums(), GetTestJpegChecksums());
            Assert.IsTrue(IsChecksumFileHidden());
        }
        #endregion
    }
}
