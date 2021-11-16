using Newtonsoft.Json;

namespace GitHubCommunicationService.Data.Models;

public record Test
{
    [JsonProperty("id")] public string? Id { get; init; }
    [JsonProperty("name")] public string? Name { get; init; }
    [JsonProperty("age")] public int Age { get; init; }
    [JsonProperty("favorite_color")] public string? FavoriteColor { get; init; }
}