using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoChurras.Models;

namespace ProjetoChurras.Controllers
{
    [ApiController]
    [Route("api/person/invite")]
    public class PersonInviteController : ControllerBase
    {
        [HttpGet]
        public ActionResult Find()
        {
            return Ok(new InviteResponse { id = "" });
        }
    }
}