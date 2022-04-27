using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDatabaseLayer
{
    public class NotePostModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BGColor { get; set; }

        [Required]
        public bool IsArchive { get; set; }

        [Required]
        public bool IsReminder { get; set; }

        [Required]
        public bool IsPin { get; set; }

        [Required]
        public bool IsTrash { get; set; }

        [Required]
        public DateTime RegisterdDate { get; set; }

    }
}
