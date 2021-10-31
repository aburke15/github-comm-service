using AutoMapper;
using GitHubApiClient.Models;
using GitHubCommunicationService.Responses;

namespace GitHubCommunicationService.MappingProfiles
{
    public class GitHubProfile : Profile
    {
        public GitHubProfile()
        {
            CreateMap<Repository, GitHubUserRepositoryResponse>();
        }
    }
}