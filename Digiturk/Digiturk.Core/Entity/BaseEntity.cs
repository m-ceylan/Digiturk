using System;
using System.Collections.Generic;
using System.Text;
using static Digiturk.Core.Enums.Enums;

namespace Digiturk.Core.Entity
{
   public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public RegistrationStatus  RegistrationStatus { get; set; }

    }
}
