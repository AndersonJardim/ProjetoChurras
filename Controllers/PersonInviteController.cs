using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using ProjetoChurras.Models;
using ProjetoChurras.Repository;

namespace ProjetoChurras.Controllers
{
    [ApiController]
    [Route("api/person/invites")]
    public class PersonInvitesController : ControllerBase
    {
        private readonly CosmosDB cosmosDB;

        public PersonInvitesController(CosmosDB cosmosDB)
        {
            this.cosmosDB = cosmosDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvitesResponse>>> Find()
        {
            try
            {
                var result = await cosmosDB.FindAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvitesResponse>> FindOne(string id, string partitionKey)
        {
            try
            {
                var response = await cosmosDB.FindOne(id, partitionKey);
                return Ok(response.Resource);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<InvitesResponse>> Create([FromBody] InvitesResponse invite)
        {
            try
            {
                var response = await cosmosDB.Create(invite);
                return Ok(response.Resource);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<InvitesResponse>> Update([FromBody] InvitesResponse invite)
        {
            try
            {
                var response = await cosmosDB.Update(invite);
                if (response)
                    return Ok(response);
                else
                    return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id, string partitionKey)
        {
            try
            {
                await cosmosDB.Delete(id, partitionKey);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}