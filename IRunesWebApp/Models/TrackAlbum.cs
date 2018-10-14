namespace IRunesWebApp.Models
{
    public class TrackAlbum : BaseEntity<int>
    {
        public virtual Track Track { get; set; }

        public string TrackId { get; set; }

        public virtual Album Album { get; set; }

        public string  AlbumId { get; set; }
    }
}