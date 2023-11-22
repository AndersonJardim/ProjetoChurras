using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoChurras.Models
{
    public class InviteResponse
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsAttending { get; set; }
    }
}