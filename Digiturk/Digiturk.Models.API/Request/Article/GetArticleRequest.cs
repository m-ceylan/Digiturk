using Digiturk.Models.Base.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Models.API.Request.Article
{
    public class GetArticleRequest
    {

        [CustomRequired]
        [CustomRequiredID]
        public string Id { get; set; }

    }
}
