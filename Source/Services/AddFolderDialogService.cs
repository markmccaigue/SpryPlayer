using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GoodPlayer.Services
{
    public class AddFolderDialogService : IDialogService
    {
        private const string MP3_FILTER = "*.mp3";

        public IEnumerable<string> ShowDialog()
        {
            var dialog = new FolderBrowserDialog();

            return dialog.ShowDialog() == DialogResult.OK ?
                Directory.GetFiles(dialog.SelectedPath, MP3_FILTER, SearchOption.AllDirectories) :
                Enumerable.Empty<string>();
        }
    }
}
