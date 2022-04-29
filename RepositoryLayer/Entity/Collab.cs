using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class Collab
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollabId{ get; set; }
        public string CollabEmail { get; set; }

        [ForeignKey("User")]
        public int? userId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Note")]
        public int? NoteId { get; set; }
        public virtual Note Note { get; set; }

    }
}