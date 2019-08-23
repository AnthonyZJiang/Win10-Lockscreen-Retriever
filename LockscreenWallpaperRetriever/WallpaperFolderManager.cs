using System.Collections.Generic;
using System.IO;
using System.Linq;
using LockscreenWallpaperRetriever.Checksum;

namespace LockscreenWallpaperRetriever
{
    public class WallpaperFolderManager
    {
        private readonly ChecksumFile _checksumFile;
        private readonly string _folderPath;
        private List<FileInfo> _jpgFiles;
        private List<FileInfo> _lwrFiles;
        private string[] _masterChecksums;

        public WallpaperFolderManager(string folderPath)
        {
            _folderPath = folderPath;
            _checksumFile = new ChecksumFile(Path.Combine(_folderPath, ".lwr"));
            _masterChecksums = _checksumFile.GetChecksums();
            CreateFolderIfNotExists();
            GenerateFileLists();
        }

        public string[] GetChecksums()
        {
            return _checksumFile.Exists
                ? _checksumFile.GetChecksums()
                : GenerateThenWriteChecksumFile();
        }

        public void CombineDuplicates()
        {
            foreach (var lwr in _lwrFiles)
            {
                if (lwr.Name != ".lwr")
                {
                    var duplicate = (new ChecksumFile(lwr.FullName));
                    _masterChecksums = Checksum.Checksum.Merge(_masterChecksums, duplicate.GetChecksums());
                }
            }

            _checksumFile.Append(_checksumFile.GetChecksums().Except(_masterChecksums).ToArray());
        }

        public void CopyNewWallpaper(List<FileInfo> imageInfos)
        {
            var extractor = new NewImageExtractor(_masterChecksums);
            extractor.Extract(imageInfos);
            var copier = new ImageCopier(extractor.GetFileInfos(), _folderPath);
            copier.CopyMissingImages();
            _checksumFile.Append(extractor.GetChecksums());

        }

        private void CreateFolderIfNotExists()
        {
            if (File.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        private void GenerateFileLists()
        {
            _lwrFiles = new List<FileInfo>();
            _jpgFiles = new List<FileInfo>();

            var files = Directory.GetFiles(_folderPath);
            foreach (var file in files)
            {
                var fi = new FileInfo(file);
                if (fi.Extension == ".lwr")
                {
                    _lwrFiles.Add(fi);
                }
                else if(fi.Extension == ".jpg")
                {
                    _jpgFiles.Add(fi);
                }
            }
        }

        private string[] GenerateThenWriteChecksumFile()
        {
            var checksums = GenerateChecksumForImages();
            if (checksums.Length != 0)
            {
                _checksumFile.Append(checksums);
            }

            return checksums;
        }

        private string[] GenerateChecksumForImages()
        {
            var existingChecksums = new string[_jpgFiles.Count];

            for (var i = 0; i < _jpgFiles.Count; i++)
            {
                existingChecksums[i] = Checksum.Checksum.Generate(_jpgFiles[i].FullName);
            }

            return existingChecksums;
        }
    }
}