using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digiturk.Repository.Definition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digiturk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository repo;

        public CategoryController(
            CategoryRepository repo
            )
        {
            this.repo = repo;
        }







    }
}
