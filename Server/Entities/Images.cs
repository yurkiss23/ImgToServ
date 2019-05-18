using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    [Table("ITS_Images")]
    public class Images
    {
        [Key]
        public int Id { get; set; }
        [Required,StringLength(maximumLength: 125)]
        public string Name { get; set; }
        [Required]
        public string Base64 { get; set; }
    }
}
