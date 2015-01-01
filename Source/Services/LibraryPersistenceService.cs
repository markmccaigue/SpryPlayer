using GoodPlayer.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoodPlayer
{
    public class LibraryPersistenceService : PersistenceServiceBase, IPersistenceService<IEnumerable<TrackProxy>, object>
    {
        private const string LIBRARY_FILE_NAME = "library.json";

        private string _libraryFilePath;
        public string LibraryFilePath
        {
            get
            {
                return _libraryFilePath ?? (_libraryFilePath = Path.Combine(AppFolderPath, LIBRARY_FILE_NAME));
            }
        }

        public void Save(IEnumerable<TrackProxy> items)
        {
            EnsurePathExists();

            var json = JsonConvert.SerializeObject(items);
            File.WriteAllText(LibraryFilePath, json);
        }

        public IEnumerable<TrackProxy> Load(object hint)
        {
            if (!File.Exists(LibraryFilePath))
            {
                return Enumerable.Empty<TrackProxy>();
            }

            var json = File.ReadAllText(LibraryFilePath);
            return JsonConvert.DeserializeObject<IEnumerable<TrackProxy>>(json);
        }
    }
}
