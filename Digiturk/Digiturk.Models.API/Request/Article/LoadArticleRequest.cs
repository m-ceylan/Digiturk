using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Digiturk.Models.API.Request.Article
{
    public class LoadArticleRequest
    {
        [Range(0, 100)]
        public int Skip { get; set; }
        [Range(1, 1000)]
        public int Take { get; set; }


        public string SearchTerm { get; set; }

    }
}
