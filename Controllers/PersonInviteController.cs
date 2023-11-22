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

        [HttpGet]
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
        public async Task<ActionResult<InviteResponse>> FindOne(string id)
        {
            string databaseId = "Churras";
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);

            var partitionKey = new PartitionKey(id);
            var response = await container.ReadItemAsync<InviteResponse>(id, partitionKey);

            return Ok(response.Resource);
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

            var response = await container.CreateItemAsync(invite);

            return Ok(response.Resource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InviteResponse>> Update(string id, [FromBody] InviteResponse invite)
        {
            string databaseId = "Churras";
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);

            var partitionKey = new PartitionKey(id);
            var response = await container.ReplaceItemAsync(invite, id, partitionKey);

            return Ok(response.Resource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            string databaseId = "Churras";
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);

            var partitionKey = new PartitionKey(id);
            await container.DeleteItemAsync<InviteResponse>(id, partitionKey);

            return NoContent();
        }
    }
}