using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static RepositoryLayer.Entity.User;

namespace RepositoryLayer.Entity
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
        public class BaseEntity
        {
            public DateTime RegisterdDate { get; set; }
            public DateTime ModifiedDate { get; set; }
        }
    }