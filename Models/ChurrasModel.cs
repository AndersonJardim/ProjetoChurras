using Newtonsoft.Json;

namespace ProjetoChurras.Models
{
    public class ChurrasModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("partitionKey")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string PartitionKey { get; set; } = "ChurrasPartition";

        public string? Reason { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateBbq { get; set; }
        public bool IsTrincaPay { get; set; }
        public bool GonnaHappen { get; set; }
    }
}