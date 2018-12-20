using System.IO;

namespace Tester
{
    public class ChecksumTestData : TestData
    {
        public static void CreateData()
        {
            CreateFolderWithJpegNoCs();
            CreateFolderNoJpegNoCs();
            CreateFolderWithJpegIncompCs();
            CreateFolderWithJpegCs();
        }

        public static void DeleteData()
        {
            if (Directory.Exists(ChecksumTestRoot))
            {
                Directory.Delete(ChecksumTestRoot, true);
            }
        }

        public static void ResetData()
        {
            DeleteData();
            CreateData();
        }

        private static void CreateFolderWithJpegNoCs()
        {
            Directory.CreateDirectory(FolderWithJpegNoCs);
            CopyAllImagesTo(FolderWithJpegNoCs);
        }

        private static void CreateFolderNoJpegNoCs()
        {
            Directory.CreateDirectory(FolderNoJpegNoCs);
        }

        private static void CreateFolderWithJpegIncompCs()
        {
            Directory.CreateDirectory(FolderWithJpegIncompCs);
            CopyAllImagesTo(FolderWithJpegIncompCs);
            CopyIncompleteChecksumFileTo(FolderWithJpegIncompCs);
        }

        private static void CreateFolderWithJpegCs()
        {
            Directory.CreateDirectory(FolderWithJpegCs);
            CopyAllImagesTo(FolderWithJpegCs);
            CopyCompleteChecksumFileTo(FolderWithJpegCs);
        }

        public static string ChecksumTestRoot => Path.Combine(ExeDirectory, "Checksum");

        public static string FolderNoJpegNoCs => Path.Combine(ChecksumTestRoot, "NoJpegNoCs");

        public static string FolderWithJpegNoCs => Path.Combine(ChecksumTestRoot, "WithJpegNoCs");

        public static string FolderWithJpegIncompCs => Path.Combine(ChecksumTestRoot, "WithJpegIncompCs");

        public static string FolderWithJpegCs => Path.Combine(ChecksumTestRoot, "WithJpegCs");

        public static string CsIncompleteFile => Path.Combine(FolderWithJpegIncompCs, "incomplete.lwr");
    }
}
