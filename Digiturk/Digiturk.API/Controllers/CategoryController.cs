using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digiturk.API.Code;
using Digiturk.Repository.Definition;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Digiturk.API.Controllers
{
    public class CategoryController : BaseController<CategoryController>
    {
        private readonly CategoryRepository repo;

        public CategoryController(
            CategoryRepository repo
            )
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<Entity.Definition.Category>> Load()
        {
            var response = await repo.GetBy(x=>true).ToListAsync();

            return Ok(response);
        }
   }
}
