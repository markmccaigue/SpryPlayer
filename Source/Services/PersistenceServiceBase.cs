using System;
using System.IO;

namespace GoodPlayer.Services
{
    public class PersistenceServiceBase
    {
        private const string APP_FOLDER_NAME = "SpryPlayer";

        private string _appFolderPath;
        public string AppFolderPath
        {
            get
            {
                return _appFolderPath ?? (_appFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), APP_FOLDER_NAME));
            }
        }

        protected void EnsurePathExists()
        {
            if (!Directory.Exists(AppFolderPath))
            {
                Directory.CreateDirectory(AppFolderPath);
            }
        }
    }
}
