using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjetoChurras.Models
{
    public class InviteResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; } = "InvitePartition";
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsAttending { get; set; }
    }
}