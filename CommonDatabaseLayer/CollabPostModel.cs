using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDatabaseLayer
{
   public  class CollabPostModel
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[A-Za-z0-9]{3,}([.][A-Za-z0-9]{3,})?[@][A-Za-z]{2,}[.][A-Za-z]{2,}([.][a-zA-Z]{2})?$", ErrorMessage = "EmailId is not valid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
