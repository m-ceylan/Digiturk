using Digiturk.Entity.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Repository.Definition
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(string connectionString, string database, string collection) : base(connectionString, database, collection)
        {
        }
    }
}
