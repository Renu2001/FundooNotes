using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class NoteLabelEntity
    {
        //public int Id { get; set; }
        public int? NoteId { get; set; }
        public NoteEntity? Notes { get; set; }
        public int? LabelId { get; set; }
        public LabelEntity? Labels { get; set; }
    }
}
