using Newtonsoft.Json;
using System;

namespace GoodPlayer
{
    public class TrackProxy
    {
        private TagLib.Tag _properties;
        private string _title;
        private string _artist;
        private string _albumName;
        private string _genre;
        private string _composer;
        private uint _year;
        private byte[] _picture;

        public Uri Location { get; set; }

        private TagLib.Tag Properties
        {
            get { return _properties ?? (_properties = TagLib.File.Create(Location.OriginalString).Tag); }
        }

        public string Title
        {
            get { return _title ?? (_title = Properties.Title); }
            set { _title = value; }
        }

        [JsonIgnore]
        public string Artist
        {
            get { return _artist ?? (_artist = Properties.FirstPerformer); }
        }

        [JsonIgnore]
        public string AlbumName
        {
            get { return _albumName ?? (_albumName = Properties.Album); }
        }

        [JsonIgnore]
        public uint Year
        {
            get { return _year == 0 ? (_year = Properties.Year) : _year; }
        }

        [JsonIgnore]
        public string Composer
        {
            get { return _composer ?? (_composer = Properties.FirstComposer); }
        }

        [JsonIgnore]
        public string Genre
        {
            get { return _genre ?? (_genre = Properties.FirstGenre); }
        }

        [JsonIgnore]
        public byte[] Picture
        {
            get { return _picture ?? (_picture = Properties.Pictures.Length > 0 ? Properties.Pictures[0].Data.Data : null); }
        }

        public TrackProxy(Uri location)
        {
            Location = location;
        }
    }
}
