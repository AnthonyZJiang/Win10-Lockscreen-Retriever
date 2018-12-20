using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Tester
{
    public abstract class TestData
    {
        public static string ExeDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string TestDataRoot
        {
            get
            {
                var pathItems = ExeDirectory.Split(Path.DirectorySeparatorChar);
                var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
                var projectPath = String.Join(Path.DirectorySeparatorChar.ToString(),
                    pathItems.Take(pathItems.Length - pos - 1));
                return Path.Combine(projectPath, "TestData");
            }
        }

        public static void CopyAllImagesTo(string destinationDirectory)
        {
            for (var i = 1; i <= 3; i++)
            {
                CopyImageTo(i, destinationDirectory);
            }
        }

        public static void CopyImageTo(int imageIndex, string destinationDirectory)
        {
            if (imageIndex < 1 || imageIndex > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(imageIndex), "Index must be 1, 2 or 3.");
            }

            var fileName = $"{imageIndex}.jpg";
            CopyFileFromDataFolderTo(fileName,destinationDirectory);
        }

        public static void CopyCompleteChecksumFileTo(string destinationDirectory)
        {
            CopyFileFromDataFolderTo("complete.lwr", destinationDirectory, ".lwr");
        }

        public static void CopyIncompleteChecksumFileTo(string destinationDirectory)
        {
            CopyFileFromDataFolderTo("incomplete.lwr", destinationDirectory, ".lwr");
        }

        public static void CopyFileFromDataFolderTo(string dataFileName, string destinationDirectory)
        {
            var source = Path.Combine(TestDataRoot, dataFileName);
            var destination = Path.Combine(destinationDirectory, dataFileName);
            File.Copy(source, destination, true);
        }

        public static void CopyFileFromDataFolderTo(string dataFileName, string destinationDirectory, string newFileName)
        {
            var source = Path.Combine(TestDataRoot, dataFileName);
            var destination = Path.Combine(destinationDirectory, newFileName);
            File.Copy(source, destination, true);
        }
    }
}
