using System.Collections.Generic;

namespace Programemein.Data.Entities
{
    public class Source
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public string OriginalUrl { get; set; }

        public virtual ICollection<Meme> Memes { get; set; }
            = new HashSet<Meme>();
    }
}
