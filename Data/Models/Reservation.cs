using JetBrains.Annotations;

namespace GitHubCommunicationService.Data.Models
{
    [UsedImplicitly]
    public record Reservation
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public byte PartySize { get; set; }
    }
}