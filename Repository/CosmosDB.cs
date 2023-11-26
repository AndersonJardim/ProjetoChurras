using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ProjetoChurras.Models;

namespace ProjetoChurras.Repository
{
    public class CosmosDB
    {
        private readonly IConfiguration configuration;

        public CosmosDB(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Container Connection()
        {
            string databaseId = "Churras";
            string containerId = "Invites";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);
            return container;
        }

        public async Task<List<InvitesResponse>> FindAll()
        {
            var container = Connection();

            var sqlQueryText = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<InvitesResponse>(queryDefinition);
            var result = new List<InvitesResponse>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var document in currentResultSet)
                {
                    result.Add(document);
                }
            }

            return result;
        }

        public async Task<ItemResponse<InvitesResponse>> FindOne(string id, string partitionKey)
        {
            var container = Connection();

            var partKey = new PartitionKey(partitionKey);
            var response = await container.ReadItemAsync<InvitesResponse>(id, partKey);
            return response;
        }

        public async Task<ItemResponse<InvitesResponse>> Create(InvitesResponse invite)
        {
            var container = Connection();

            // Gere um novo GUID para o ID
            invite.Id = Guid.NewGuid().ToString();

            var response = await container.CreateItemAsync(invite);
            return response;
        }

        public async Task<bool> Update(InvitesResponse invite)
        {
            var container = Connection();

            // Verificar a existência do documento
            var partKey = new PartitionKey(invite.PartitionKey);
            var existingDocument = await container.ReadItemAsync<InvitesResponse>(invite.Id, partKey);

            if (existingDocument == null)
            {
                return false;
            }

            // Atualizar o documento
            await container.ReplaceItemAsync(invite, invite.Id);

            return true;
        }

        public async Task Delete(string id, string partitionKey)
        {
            var container = Connection();

            var partKey = new PartitionKey(partitionKey);
            await container.DeleteItemAsync<InvitesResponse>(id, partKey);
        }

    }
}