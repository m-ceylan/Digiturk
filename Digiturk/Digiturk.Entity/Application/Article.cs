using Digiturk.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digiturk.Entity.Application
{
  public  class Article:BaseEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public DateTime? PublisDate { get; set; }
        public string CategoryID { get; set; }
        public string OwnerID { get; set; }
        public Digiturk.Entity.Definition.Category Category { get; set; } = new Definition.Category();
        public Digiturk.Entity.ProjectUser.User Owner { get; set; } = new ProjectUser.User();

    }
}
