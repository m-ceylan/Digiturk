using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Models.API.Response.Article
{
public    class LoadArticleResponse
    {
        public List<Digiturk.Entity.Application.Article> Items { get; set; } = new List<Entity.Application.Article>();

    }
}
