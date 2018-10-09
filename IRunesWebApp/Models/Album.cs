namespace IRunesWebApp.Models
{
    using System.Collections.Generic;

    public class Album : BaseEntity<string>
    {
        public Album()
        {
            this.Tracks = new HashSet<TrackAlbum>();
           
        }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get;}

        public virtual ICollection<TrackAlbum> Tracks { get; set; }
    }
}
