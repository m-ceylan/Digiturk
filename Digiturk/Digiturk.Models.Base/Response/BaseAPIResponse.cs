using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Models.Base.Response
{
    public class BaseAPIResponse<T>
    {
        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();

        public void AddValidationError(string Key, string Value)
        {
            this.ValidationErrors.Add(new ValidationError { Key = Key, Value = Value });
        }

        public void SetMessage(string Message)
        {
            if (this.ValidationErrors.Any())
            {
                throw new Exception("Set message cant be use with validation errors");
            }

            this.HasError = false;
            this.Message = Message;
        }

        public void SetErrorMessage(string Message2)
        {
            this.HasError = true;
            this.Message = Message2;
        }

        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
