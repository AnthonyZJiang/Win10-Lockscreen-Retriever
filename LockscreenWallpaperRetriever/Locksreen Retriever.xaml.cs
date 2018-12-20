using System;
using System.IO;
using System.Reflection;
using System.Windows;
namespace LockscreenWallpaperRetriever
{
    /// <summary>
    /// Interaction logic for Locksreen_Retriever.xaml
    /// </summary>
    public partial class Locksreen_Retriever : Window
    {
        private bool _isSilent;
        public Locksreen_Retriever()
        {
            RegisterStartup();
            ParseCmdArgs();
            if (_isSilent)
            {
                SilentUpdate();
                ShutdownApp();
                return;
            }
            InitializeComponent();
        }

        private void SilentUpdate()
        {
            if (!IsImageFolderValid())
            {
                return;
            }

            var imageFinder = new ImageFinder();
            var imageFileInfos = imageFinder.Find();
            var imageCopier = new ImageCopier(Properties.Settings.Default.ImageFolder, imageFileInfos);
            imageCopier.CopyMissingImages();
        }

        private void ParseCmdArgs()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                foreach (var arg in args)
                {
                    if (arg == "--silent")
                    {
                        _isSilent = true;
                    }
                }
            }
        }

        private bool IsImageFolderValid()
        {
            return Directory.Exists(Properties.Settings.Default.ImageFolder);
        }

        private void ShutdownApp()
        {
            Application.Current.Shutdown();
        }

        private void RegisterStartup()
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("Locksreen Auto-retrievers", $"\"{GetAppExeFile()}\" --silent");
        }

        private string GetAppExeFile()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetFullPath(path);
        }
    }
}
