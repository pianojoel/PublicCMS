using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.Data;
using Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly PublicContext _ctx;
        public ProjectsController(PublicContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _ctx.Projects.ToListAsync();
        }


    }
}
