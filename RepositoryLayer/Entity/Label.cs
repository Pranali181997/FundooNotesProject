using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class Label
    {
        public int? UserId { get; set; }
        public int? NoteId { get; set; }
        public virtual User User { get; set; }
        public virtual Note Note { get; set; }
        public string LableName { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LableId { get; set; }
    }
}
