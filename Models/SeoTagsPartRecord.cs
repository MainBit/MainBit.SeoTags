using Orchard.ContentManagement.Records;
using System.ComponentModel.DataAnnotations;

namespace MainBit.SeoTags.Models
{
    public class SeoTagsPartRecord : ContentPartRecord 
    {
        [StringLength(2048)]
        public virtual string Title { get; set; }
        public virtual string Keywords { get; set; }
        public virtual string Description { get; set; }
        public virtual string Canonical { get; set; }
    }
}