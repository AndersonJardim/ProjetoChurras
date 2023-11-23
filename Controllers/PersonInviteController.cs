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
            var container = cosmosDB.Connection();

            var sqlQueryText = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<InviteResponse>(queryDefinition);
            var result = new List<InviteResponse>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var document in currentResultSet)
                {
                    result.Add(document);
                }
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InviteResponse>> FindOne(string id, string partitionKey)
        {
            try
            {
                var container = cosmosDB.Connection();

                var partKey = new PartitionKey(partitionKey);
                var response = await container.ReadItemAsync<InviteResponse>(id, partKey);

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
            var container = cosmosDB.Connection();

            // Gere um novo GUID para o ID
            invite.Id = Guid.NewGuid().ToString();

            var response = await container.CreateItemAsync(invite);

            return Ok(response.Resource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InviteResponse>> Update([FromBody] InviteResponse invite)
        {
            try
            {
                var container = cosmosDB.Connection();

                // Verificar a existência do documento
                var partKey = new PartitionKey(invite.PartitionKey);
                var existingDocument = await container.ReadItemAsync<InviteResponse>(invite.Id, partKey);

                if (existingDocument == null)
                {
                    return NotFound(); // ou outro código de status adequado
                }

                // Atualizar o documento
                var response = await container.ReplaceItemAsync(invite, invite.Id);

                return Ok(response.Resource);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id, string partitionKey)
        {
            var container = cosmosDB.Connection();

            var partKey = new PartitionKey(partitionKey);
            await container.DeleteItemAsync<InviteResponse>(id, partKey);

            return NoContent();
        }
    }
}