using Digiturk.Entity.ProjectUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Repository.ProjectUser
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(string connectionString, string database, string collection) : base(connectionString, database, collection)
        {
        }
    }
}
