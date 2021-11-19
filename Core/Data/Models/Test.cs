using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ABU.GitHubCommunicationService.Core.Data.Models;

public record Test
{
    [BsonElement("_id")]
    public ObjectId? Id { get; init; }
    [BsonElement("name")]
    public string? Name { get; init; }
    [BsonElement("age")]
    public int Age { get; init; }
    [BsonElement("favorite_color")]
    public string? FavoriteColor { get; init; }
}