using JetBrains.Annotations;

namespace GitHubCommunicationService.Config
{
    [UsedImplicitly]
    public record GitHubOptions
    {
        public string? AuthToken { get; set; }
        public string BaseUri { get; set; } = null!;
        public string? Username { get; set; }
    }
}