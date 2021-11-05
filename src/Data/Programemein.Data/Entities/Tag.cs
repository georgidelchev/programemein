using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Programemein.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Meme> Memes { get; set; }
            = new HashSet<Meme>();
    }
}
