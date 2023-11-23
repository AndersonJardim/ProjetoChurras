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
    [Route("api/person/invite")]
    public class PersonInviteController : ControllerBase
    {
        private readonly CosmosDB cosmosDB;

        public PersonInviteController(CosmosDB cosmosDB)
        {
            this.cosmosDB = cosmosDB;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<InviteResponse>>> Find()
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
        public async Task<ActionResult<InviteResponse>> FindOne(string id, string partitionKey)
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
        public async Task<ActionResult<InviteResponse>> Create([FromBody] InviteResponse invite)
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
        public async Task<ActionResult<InviteResponse>> Update([FromBody] InviteResponse invite)
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