using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GoodPlayer.Services;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Windows.Media;

namespace GoodPlayer.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic) { }
            else
            {
                // Dependencies
                SimpleIoc.Default.Register<MediaPlayer>(() => new MediaPlayer { Volume = 0.5 });
                SimpleIoc.Default.Register<AddFileDialogService>();
                SimpleIoc.Default.Register<AddFolderDialogService>();
                SimpleIoc.Default.Register<IPersistenceService<IEnumerable<TrackProxy>, object>, LibraryPersistenceService>();

                // Top-level objects
                SimpleIoc.Default.Register<LibraryViewModel>();
                SimpleIoc.Default.Register<PlayerViewModel>();
            }
        }

        public LibraryViewModel Library
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LibraryViewModel>();
            }
        }

        public PlayerViewModel Player
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayerViewModel>();
            }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
        }
    }
}