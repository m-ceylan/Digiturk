using Digiturk.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Entity.Definition
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public int ArticleCount { get; set; }
        public int OrderNo { get; set; }

    }
}
