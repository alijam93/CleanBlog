
using CleanBlog.Shared.Identity.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Api.Controllers.Base
{
    [Authorize(Policy = Policies.IsAdmin)]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminControllerBase : ControllerBase
    {
        protected const string Id = "{id}";
    }
}
