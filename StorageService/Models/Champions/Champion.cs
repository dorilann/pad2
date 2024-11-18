using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StorageService.Models.Champions
{
    public class Champion
    {
        [BsonId]
        public long Key { get; set; }
        public string Version { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public List<string> Tags { get; set; }
        public string Partype { get; set; }
    }
}
