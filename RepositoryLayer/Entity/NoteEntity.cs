using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class NoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public bool IsTrashed { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        [JsonIgnore]
        public ICollection<CollaboratorEntity>? Collabarators { get; set; }
        [JsonIgnore]
        public ICollection<NoteLabelEntity> NoteLabel { get; set; } 

    }
}
