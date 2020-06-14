using Digiturk.Models.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Digiturk.Models.API.Request.Category
{
   public class CategoryCreateRequest
    {
        [CustomRequired]
        public string Title { get; set; }
        public string Image { get; set; }


        [Range(1,500,ErrorMessage ="Lütfen, 1-500 arası değer giriniz.")]
        public int OrderNo { get; set; }

    }
}
