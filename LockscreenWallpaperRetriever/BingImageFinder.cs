using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LockscreenWallpaperRetriever
{
    public class BingImageFinder
    {
        private List<FileInfo> _currentQualifiedFiles;
        private BackgroundWorker _imageSizeCheckBgWorker;

        public int FileSizeGate { get; set; } = 102400;
        public int ImageHeightMin { get; set; } = 1080;
        public int ImageWidthMin { get; set; } = 1920;
        
        public List<FileInfo> GetImageInfos(bool isGetLatest = true)
        {
            GetAllFilesInAssetFolder();
            FilterFilesBySize();
            if (isGetLatest)
            {
                FilterFilesByDate();
            }

            FilterFilesByImageSize();

            return _currentQualifiedFiles;
        }

        private void GetAllFilesInAssetFolder()
        {
            _currentQualifiedFiles = new List<FileInfo>();
            var files = Directory.GetFiles(GetAssetsFolder());
            
            foreach (var file in files)
            {
                _currentQualifiedFiles.Add(new FileInfo(file));
            }
        }

        private static string GetAssetsFolder()
        {
            var appData = Environment.ExpandEnvironmentVariables("%LocalAppData%");
            return
                appData + @"\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";
        }

        private void FilterFilesBySize()
        {
            if (_currentQualifiedFiles.Count == 0)
            {
                return;
            }
            
            foreach (var qualifiedFiles in _currentQualifiedFiles.ToList())
            {
                if (qualifiedFiles.Length < FileSizeGate)
                {
                    _currentQualifiedFiles.Remove(qualifiedFiles);
                }
            }
        }

        private void FilterFilesByImageSize(object sender = null, DoWorkEventArgs e = null)
        {
            if (_currentQualifiedFiles.Count == 0)
            {
                return;
            }

            foreach (var qualifiedFile in _currentQualifiedFiles.ToList())
            {
                var size = GetImageSize(qualifiedFile.FullName);
                if (!IsSizeValid(size))
                {
                    _currentQualifiedFiles.Remove(qualifiedFile);
                }
            }
        }

        private static Size GetImageSize(string filename)
        {
            return Image.FromFile(filename).Size;
        }

        private bool IsSizeValid(Size size)
        {
            return size.Height >= ImageHeightMin && size.Width >= ImageWidthMin;
        }

        private void FilterFilesByDate()
        {
            if (_currentQualifiedFiles.Count == 0)
            {
                return;
            }

            foreach (var qualifiedFile in _currentQualifiedFiles.ToList())
            {
                var date = qualifiedFile.CreationTime;
                if (!IsDateWithinAMonth(date))
                {
                    _currentQualifiedFiles.Remove(qualifiedFile);
                }
            }
        }

        private static bool IsDateWithinAMonth(DateTime date)
        {
            var aMonthAgo = DateTime.Now.AddMonths(-1);
            return date > aMonthAgo;
        }
    }
}
