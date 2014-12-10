
namespace CTrade.Client.Services.Entities
{
    public  abstract class EntityBase
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string DocType { get; set; }
    }
}
