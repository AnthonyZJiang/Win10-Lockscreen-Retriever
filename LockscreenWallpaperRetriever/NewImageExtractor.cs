using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockscreenWallpaperRetriever
{
    class NewImageExtractor
    {
        private readonly string[] _existingChecksums;
        private List<string> _newChecksums;
        private List<FileInfo> _newFileInfo;
        public NewImageExtractor(string[] existingChecksums)
        {
            _existingChecksums = existingChecksums;
        }

        public void Extract(List<FileInfo> infos)
        {
            var chksums = GetChecksumsFrom(infos);
            var isNew = IsNewWallpaper(chksums);

            _newChecksums = new List<string>();
            _newFileInfo = new List<FileInfo>();
            for (var i = 0; i<isNew.Length; i++)
            {
                if (isNew[i])
                {
                    _newChecksums.Add(chksums[i]);
                    _newFileInfo.Add(infos[i]);
                }
            }
        }

        public string[] GetChecksums()
        {
            return _newChecksums.ToArray();
        }

        public FileInfo[] GetFileInfos()
        {
            return _newFileInfo.ToArray();
        }

        private string[] GetChecksumsFrom(List<FileInfo> infos)
        {
           return infos.Select(x => Checksum.Checksum.Generate(x.FullName)).ToArray();
        }

        private bool[] IsNewWallpaper(string[] chksums)
        {
            var isNew = new bool[chksums.Length];
            for (var i = 0; i < chksums.Length; i++)
            {
                if (!_existingChecksums.Contains(chksums[i]))
                    isNew[i] = true;
            }
            return isNew;
        }
    }
}
