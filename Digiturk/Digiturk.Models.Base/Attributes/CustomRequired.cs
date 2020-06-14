using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Digiturk.Models.Base.Attributes
{
    public class CustomRequired:RequiredAttribute
    {
        public override bool IsValid(object value)
        {

            this.ErrorMessage = "Lütfen bu alanı doldurun.";

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || (value != null && value.ToString() == "1.01.0001 00:00:00"))
                return false;

            return true;
           
        }
    }
}
