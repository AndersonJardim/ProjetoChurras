using Microsoft.Azure.Cosmos;
using ProjetoChurras.Models;

namespace ProjetoChurras.Repository
{
    public class ChurrasRepository
    {
        private readonly IConfiguration configuration;

        public ChurrasRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private Container Connection()
        {
            string databaseId = "Churras";
            string containerId = "Bbq";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);
            return container;
        }

        public async Task<ItemResponse<ChurrasModel>> Create(ChurrasModel churras)
        {
            var container = Connection();
            churras.Id = Guid.NewGuid().ToString();
            var response = await container.CreateItemAsync(churras);
            return response;
        }

        public async Task<List<ChurrasModel>> FindAll()
        {
            var container = Connection();

            var sqlQueryText = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<ChurrasModel>(queryDefinition);
            var result = new List<ChurrasModel>();

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

        public async Task<bool> Update(ChurrasModel invite)
        {
            var container = Connection();

            // Verificar a existência do documento
            var partKey = new PartitionKey(invite.PartitionKey);
            var existingDocument = await container.ReadItemAsync<ChurrasModel>(invite.Id, partKey);

            if (existingDocument == null)
            {
                return false;
            }

            // Atualizar o documento
            await container.ReplaceItemAsync(invite, invite.Id);

            return true;
        }
    }
}