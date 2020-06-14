using Digiturk.Models.Base.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Models.API.Request.Article
{
  public class CreateArticleRequest
    {
        [CustomRequired]
        public string Title { get; set; }
        [CustomRequired]
        public string Summary { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public DateTime? PublisDate { get; set; }
        [CustomRequired]
        [CustomRequiredID]
        public string CategoryID { get; set; }


    }
}
