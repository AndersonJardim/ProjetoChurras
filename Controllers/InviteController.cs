using Microsoft.AspNetCore.Mvc;
using ProjetoChurras.Models;
using ProjetoChurras.Repository;

namespace ProjetoChurras.Controllers
{
    [ApiController]
    [Route("api/person/invites")]
    public class InvitesController : ControllerBase
    {
        private readonly InviteRepository inviteRepository;

        public InvitesController(InviteRepository inviteRepository)
        {
            this.inviteRepository = inviteRepository;
        }

        [HttpPost]
        public async Task<ActionResult<InvitesModel>> InviteAccept([FromBody] InvitesModel invitesModel)
        {
            try
            {
                var response = await inviteRepository.Create(invitesModel);
                return Ok(response.Resource);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message ?? e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<InvitesModel>> InviteUpdate([FromBody] InvitesModel invitesModel)
        {
            try
            {
                var response = await inviteRepository.Update(invitesModel);
                return Ok($"Update realizado. Status: {response}");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message ?? e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<InvitesModel>>> GetChurras()
        {
            try
            {
                var response = await inviteRepository.FindAll();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message ?? e.Message);
            }
        }
    }
}