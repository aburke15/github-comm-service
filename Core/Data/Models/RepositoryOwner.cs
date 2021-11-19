using Newtonsoft.Json;

namespace ABU.GitHubCommunicationService.Core.Data.Models
{
    public record RepositoryOwner
    {
        [JsonProperty("login")] public string? Login { get; init; }

        [JsonProperty("id")] public long Id { get; init; }

        [JsonProperty("node_id")] public string? NodeId { get; init; }

        [JsonProperty("avatar_url")] public Uri? AvatarUrl { get; init; }

        [JsonProperty("gravatar_id")] public string? GravatarId { get; init; }

        [JsonProperty("url")] public Uri? Url { get; init; }

        [JsonProperty("html_url")] public Uri? HtmlUrl { get; init; }

        [JsonProperty("followers_url")] public Uri? FollowersUrl { get; init; }

        [JsonProperty("following_url")] public string? FollowingUrl { get; init; }

        [JsonProperty("gists_url")] public string? GistsUrl { get; init; }

        [JsonProperty("starred_url")] public string? StarredUrl { get; init; }

        [JsonProperty("subscriptions_url")] public Uri? SubscriptionsUrl { get; init; }

        [JsonProperty("organizations_url")] public Uri? OrganizationsUrl { get; init; }

        [JsonProperty("repos_url")] public Uri? ReposUrl { get; init; }

        [JsonProperty("events_url")] public string? EventsUrl { get; init; }

        [JsonProperty("received_events_url")] public Uri? ReceivedEventsUrl { get; init; }

        [JsonProperty("type")] public string? Type { get; init; }

        [JsonProperty("site_admin")] public bool SiteAdmin { get; init; }
    }
}