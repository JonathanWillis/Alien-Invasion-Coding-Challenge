using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AlienInvasion.Server.REST.Models
{
    public class CreateTeamRequest
    {
        [Required]
        public string Name { get; set; }
    }
}