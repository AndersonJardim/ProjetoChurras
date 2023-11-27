using Microsoft.Azure.Cosmos;
using ProjetoChurras.Models;

namespace ProjetoChurras.Repository
{
    public class InviteRepository
    {
        private readonly IConfiguration configuration;

        public InviteRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private Container Connection()
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

        public async Task<ItemResponse<InvitesModel>> Create(InvitesModel invite)
        {
            var container = Connection();
            invite.Id = Guid.NewGuid().ToString();

            var response = await container.CreateItemAsync(invite);
            return response;
        }

        public async Task<bool> Update(InvitesModel invitesModel)
        {
            var container = Connection();

            var partKey = new PartitionKey(invitesModel.PartitionKey);
            var existingDocument = await container.ReadItemAsync<InvitesModel>(invitesModel.Id, partKey);

            if (existingDocument == null)
            {
                return false;
            }

            await container.ReplaceItemAsync(invitesModel, invitesModel.Id);

            return true;
        }

        public async Task<List<InvitesModel>> FindAll()
        {
            var container = Connection();

            var sqlQueryText = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<InvitesModel>(queryDefinition);
            var result = new List<InvitesModel>();

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
    }
}