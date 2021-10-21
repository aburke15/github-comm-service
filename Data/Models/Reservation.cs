namespace GitHubCommunicationService.Data.Models
{
    public record Reservation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public byte PartySize { get; set; }
    }
}