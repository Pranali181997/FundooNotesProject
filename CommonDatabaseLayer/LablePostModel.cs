using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDatabaseLayer
{
    public class LablePostModel
    {
        [Required]
        public string LableName { get; set; }
    }
}
