using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Programemein.Data.Entities
{
    public class Meme
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        public int SourceId { get; set; }

        public Source Source { get; set; }

        public virtual ImageData ImageData { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
            = new List<Tag>();
    }
}
