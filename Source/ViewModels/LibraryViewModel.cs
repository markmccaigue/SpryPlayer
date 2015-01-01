using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoodPlayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GoodPlayer.ViewModel
{
    public class LibraryViewModel : ViewModelBase
    {
        private ObservableCollection<TrackProxy> _tracks;
        private TrackProxy _selectedTrack;
        private IDialogService _fileDialogService;
        private IDialogService _folderDialogService;
        private IPersistenceService<IEnumerable<TrackProxy>, object> _persistenceService;

        private Random _random = new Random();

        public ICommand AddFileCommand { get; private set; }
        public ICommand AddFolderCommand { get; private set; }
        public ICommand RemoveFileCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand SaveLibraryCommand { get; private set; }

        public LibraryViewModel(AddFileDialogService fileDialogService, IPersistenceService<IEnumerable<TrackProxy>, object> persistenceService, AddFolderDialogService folderDialogService)
        {
            _fileDialogService = fileDialogService;
            _persistenceService = persistenceService;
            _folderDialogService = folderDialogService;

            AddFolderCommand = new RelayCommand(() => AddFiles(_folderDialogService));
            AddFileCommand = new RelayCommand(() => AddFiles(_fileDialogService));
            RemoveFileCommand = new RelayCommand(RemoveFile);
            NextCommand = new RelayCommand(Next);
            SaveLibraryCommand = new RelayCommand(SaveLibrary);

            MessengerInstance.Register<SpryMessage>(this, HandleMessage);
        }

        public ObservableCollection<TrackProxy> Tracks
        {
            get
            {
                return _tracks ?? (_tracks = new ObservableCollection<TrackProxy>(_persistenceService.Load(null)));
            }
        }

        public TrackProxy SelectedTrack
        {
            get
            {
                return _selectedTrack;
            }
            set
            {
                Set(() => SelectedTrack, ref _selectedTrack, value, true);
            }
        }

        private void HandleMessage(SpryMessage message)
        {
            if (message == SpryMessage.MoveNext)
            {
                Next();
            }
        }

        private void Next()
        {
            var position = _random.Next(Tracks.Count);
            SelectedTrack = Tracks[position];
        }

        private void AddFiles(IDialogService dialogService)
        {
            var fileNames = dialogService.ShowDialog();
            fileNames.ToList().ForEach(f => Tracks.Add(new TrackProxy(new Uri(f))));
        }

        private void RemoveFile()
        {
            if (SelectedTrack != null)
            {
                var position = Tracks.IndexOf(SelectedTrack);
                Tracks.Remove(SelectedTrack);
                SelectedTrack = Tracks.Count > 0 ? Tracks[Math.Min(Tracks.Count - 1, position)] : null;
            }
        }

        private void SaveLibrary()
        {
            _persistenceService.Save(Tracks);
        }
    }
}
