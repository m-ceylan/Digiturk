using Digiturk.Entity.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Repository.Application
{
    public class ArticleRepository : BaseRepository<Article>
    {
        public ArticleRepository(string connectionString, string database, string collection) : base(connectionString, database, collection)
        {
        }
    }
}
