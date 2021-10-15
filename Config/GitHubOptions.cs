namespace GitHubCommunicationService.Config
{
    public record GitHubOptions
    {
        public string AuthToken { get; set; }
        public string BaseUri { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        
    }
}