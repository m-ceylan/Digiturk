using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Digiturk.Models.Base.Attributes
{
   public class CustomRequiredID:RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                try
                {
                    new ObjectId(value.ToString());
                }
                catch
                {
                    this.ErrorMessage = "Geçersiz parametre: ID";
                    return false;
                }

            }
                return true;
        }

    }
}
