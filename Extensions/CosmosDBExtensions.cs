using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace ProjetoChurras.Extensions
{
    public static class CosmosDBExtensions
    {
        internal static async Task AddDataBase(this ConfigurationManager configuration)
        {
            string databaseId = "Churras";

            string endpointUri = configuration["CosmosDb:EndpointUri"]!;
            string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

            // Configuração do cliente Cosmos
            var cosmosClient = new CosmosClient(endpointUri, primaryKey);

            // Obtém o banco de dados e o contêiner
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            AddDataBaseContainer(database, "Invites");
            AddDataBaseContainer(database, "Person");
        }

        private static async void AddDataBaseContainer(Database database, string containerId)
        {
            Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
        }
    }
}