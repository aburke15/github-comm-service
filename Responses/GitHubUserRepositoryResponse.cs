using System;
using Newtonsoft.Json;

namespace GitHubCommunicationService.Responses
{
    public record GitHubUserRepositoryResponse
    {
        [JsonProperty("id")]
        public long Id { get; init; }
        [JsonProperty("node_id")]
        public string? NodeId { get; init; }
        [JsonProperty("name")]
        public string? Name { get; init; }
        [JsonProperty("full_name")]
        public string? FullName { get; init; }
        [JsonProperty("owner")]
        public dynamic? Owner { get; init; }
        [JsonProperty("html_url")]
        public string? HtmlUrl { get; init; }
        [JsonProperty("description")]
        public string? Description { get; init; }
        [JsonProperty("fork")]
        public bool Fork { get; init; }
        [JsonProperty("url")]
        public string? Url { get; init; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; init; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; init; }
        [JsonProperty("clone_url")]
        public string? CloneUrl { get; init; }
        [JsonProperty("size")]
        public long Size { get; init; }
        [JsonProperty("watchers_count")]
        public int WatchersCount { get; init; }
        [JsonProperty("language")]
        public string? Language { get; init; }
        [JsonProperty("forks_count")]
        public int ForksCount { get; init; }
        [JsonProperty("archived")]
        public bool Archived { get; init; }
        [JsonProperty("disabled")]
        public bool Disabled { get; init; }
        [JsonProperty("license")]
        public string? License { get; init; }
        [JsonProperty("forks")]
        public int Forks { get; init; }
        [JsonProperty("watchers")]
        public int Watchers { get; init; }
        [JsonProperty("default_branch")]
        public string? DefaultBranch { get; init; }
        [JsonProperty("permissions")]
        public dynamic? Permissions { get; init; }
    }
}