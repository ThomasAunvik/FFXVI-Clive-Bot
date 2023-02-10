using System.ComponentModel.DataAnnotations.Schema;

namespace CliveBot.Database.Models
{
    public class BotModerator
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ConnectionSource { get; set; }
        public required string ConnectionId { get; set; }
        
        public int? PermissionsId { get; set; }

        public BotModeratorPermissions? Permissions { get; set; }
    }
}
