using Newtonsoft.Json;

namespace ABU.GitHubCommunicationService.Data.Models
{
    public record RepositoryPermissions
    {
        [JsonProperty("admin")] public bool Admin { get; init; }

        [JsonProperty("maintain")] public bool Maintain { get; init; }

        [JsonProperty("push")] public bool Push { get; init; }

        [JsonProperty("triage")] public bool Triage { get; init; }

        [JsonProperty("pull")] public bool Pull { get; init; }
    }
}