using Digiturk.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Entity.ProjectUser
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

    }


    public class UserMini
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }

    }

}
