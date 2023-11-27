using Newtonsoft.Json;

namespace ProjetoChurras.Models
{
    public class InvitesModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("partitionKey")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string PartitionKey { get; set; } = "InvitesPartition";

        public string Name { get; set; }
        public string? Reason { get; set; }
        public bool IsVeg { get; set; }
        public DateTime DateCreated { get; set; }
        public bool WillBeThere { get; set; }
        public DateTime DateBbq { get; set; }
    }
}