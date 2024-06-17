using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId {  get; set; }

        [Required]
        public string LabelName { get; set; }

        [JsonIgnore]
        public ICollection<NoteLabelEntity> NoteLabel { get; set; } = new List<NoteLabelEntity>();
    }
}
