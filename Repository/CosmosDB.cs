using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

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
            string containerId = "Invite";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            var cosmosClient = new CosmosClient(endpointUri, primaryKey);
            var database = cosmosClient.GetDatabase(databaseId);
            var container = database.GetContainer(containerId);
            return container;
        }
    }
}