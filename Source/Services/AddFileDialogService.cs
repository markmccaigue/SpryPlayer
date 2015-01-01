using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace GoodPlayer
{
    public class AddFileDialogService : IDialogService
    {
        private const string MP3_EXTENSION = ".mp3";
        private const string MP3_FILTER = "MP3 Files (*.mp3)|*.mp3";

        public IEnumerable<string> ShowDialog()
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = MP3_EXTENSION,
                Filter = MP3_FILTER,
                ValidateNames = true,
                Multiselect = true
            };

            return dialog.ShowDialog() == true ?
                dialog.FileNames :
                Enumerable.Empty<string>();
        }
    }
}
