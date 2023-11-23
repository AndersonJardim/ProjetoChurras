using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using ProjetoChurras.Models;

namespace ProjetoChurras.Controllers
{
    [ApiController]
    [Route("api/person/invite")]
    public class PersonInviteController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PersonInviteController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<InviteResponse>>> Find()
        {
            string databaseId = "Churras";
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);

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
                string databaseId = "Churras";
                string containerId = "Invite";

                string endpointUri = configuration["CosmosDb:EndpointUri"]!;
                string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

                var cosmosClient = new CosmosClient(endpointUri, primaryKey);
                var database = cosmosClient.GetDatabase(databaseId);
                var container = database.GetContainer(containerId);

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
            string databaseId = "Churras";
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);

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
                string databaseId = "Churras";
                string containerId = "Invite";

                string endpointUri = configuration["CosmosDb:EndpointUri"]!;
                string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

                var cosmosClient = new CosmosClient(endpointUri, primaryKey);
                var database = cosmosClient.GetDatabase(databaseId);
                var container = database.GetContainer(containerId);

                // Verificar a existência do documento
                var partKey = new PartitionKey(invite.PartitionKey);
                var existingDocument = await container.ReadItemAsync<InviteResponse>(invite.Id, partKey);

                // var sqlQueryText = $"SELECT * FROM Invite c where c.id = '{id}' ";
                // var queryDefinition = new QueryDefinition(sqlQueryText);
                // var queryResultSetIterator = container.GetItemQueryIterator<InviteResponse>(queryDefinition);
                // var result = new InviteResponse();

                // while (queryResultSetIterator.HasMoreResults)
                // {
                //     var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                //     foreach (var document in currentResultSet)
                //     {
                //         result = document;
                //         //result.Add(document);
                //     }
                // }

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
            string databaseId = "Churras";
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);

            var partKey = new PartitionKey(partitionKey);
            await container.DeleteItemAsync<InviteResponse>(id, partKey);

            return NoContent();
        }
    }
}