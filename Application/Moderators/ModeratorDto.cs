using CliveBot.Database.Models;

namespace CliveBot.Application.Moderators
{
    public class ModeratorDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ConnectionSource { get; set; }
        public required string ConnectionId { get; set; }

        public ModeratorPermissionsDto? Permissions { get; set; }
    }

    public static class ModeratorDtoExtension
    {
        public static ModeratorDto ConvertDto(this BotModerator model)
        {
            return new ModeratorDto()
            {
                Id = model.Id,
                Name = model.Name,
                ConnectionSource = model.ConnectionSource,
                ConnectionId = model.ConnectionId,
                Permissions = model.Permissions?.ConvertDto(),
            };
        }

        public static IEnumerable<ModeratorDto> ConvertDto(this IEnumerable<BotModerator> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
