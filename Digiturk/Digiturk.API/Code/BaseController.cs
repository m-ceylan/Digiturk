using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digiturk.API.Code
{

    
    [ApiController]
    [Route("[controller]/[action]")]
    
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        public string CurrentUserID
        {
            get
            {
                return User.Claims.FirstOrDefault(x => x.Type == "userID").Value.ToString();
            }
        }

    }
}
