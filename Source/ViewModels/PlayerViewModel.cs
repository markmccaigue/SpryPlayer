using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Windows.Media;

namespace GoodPlayer.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        private TrackProxy _currentTrack;
        private TrackProxy _selectedTrack;

        public bool CanPlay
        {
            get
            {
                return _canPlay;
            }
            set
            {
                Set(() => CanPlay, ref _canPlay, value);
            }
        }

        public ICommand VolumeCommand { get; set; }
        public ICommand PlayPauseCommand { get; set; }
        public ICommand PlayNextCommand { get; set; }
        public TrackProxy CurrentTrack
        {
            get
            {
                return _currentTrack;
            }
            set
            {
                Set(() => CurrentTrack, ref _currentTrack, value);
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
                Set(() => SelectedTrack, ref _selectedTrack, value);
            }
        }

        private MediaPlayer _player;
        private bool _canPlay;
        public PlayerViewModel(MediaPlayer player)
        {
            _player = player;

            PlayNextCommand = new RelayCommand(PlayNext);
            PlayPauseCommand = new RelayCommand(PlayPause);
            VolumeCommand = new RelayCommand<double>(Volume);

            MessengerInstance.Register<PropertyChangedMessage<TrackProxy>>(this, HandleTrackSelection);

            _player.MediaEnded += _player_MediaEnded;
        }

        void _player_MediaEnded(object sender, System.EventArgs e)
        {
            PlayNext();
        }

        private void HandleTrackSelection(PropertyChangedMessage<TrackProxy> message)
        {
            SelectedTrack = message.NewValue;
            CanPlay = (message.OldValue == null || SelectedTrack != CurrentTrack) && SelectedTrack != null;
        }

        private void Play()
        {
            if (!CanPlay)
            {
                return;
            }

            if (CurrentTrack != SelectedTrack)
            {
                CurrentTrack = SelectedTrack;
                _player.Open(CurrentTrack.Location);
            }

            CanPlay = false;
            _player.Play();
        }

        private void Volume(double volume)
        {
            // Arbitrary rounding to make the slider a little easier to pull to min
            if (volume < 0.03)
            {
                volume = 0;
            }

            _player.Volume = volume;
        }

        private void Pause()
        {
            if (CanPlay)
            {
                return;
            }

            CanPlay = true;
            _player.Pause();
        }

        private void PlayNext()
        {
            Messenger.Default.Send(SpryMessage.MoveNext);
            Play();
        }
        private void PlayPause()
        {
            if (CanPlay)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }
}
