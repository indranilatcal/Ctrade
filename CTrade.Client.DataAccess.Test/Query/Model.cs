using Newtonsoft.Json.Linq;
using System.Linq;

namespace CTrade.Client.DataAccess.Test.Query
{
    public class Artist
    {
        public string Id { get; set; }
        public string Rev { get; set; }

        public string Name { get; set; }

        public Album[] Albums { get; set; }

        internal JObject AsJObject()
        {
            dynamic doc = new JObject();
            doc._id = Id;
            if (!string.IsNullOrWhiteSpace(Rev))
                doc._rev = Rev;
            doc.name = Name;
            doc.Add("$doctype", "artist");

            if (Albums != null & Albums.Any())
            {
                JArray albums = new JArray(Albums.Select(a => a.AsJObject()).ToArray());
                doc.albums = albums;
            }

            return doc as JObject;
        }
    }

    public class Album
    {
        public string Name { get; set; }
        internal JObject AsJObject()
        {
            dynamic doc = new JObject();
            doc.name = Name;

            return doc;
        }
    }
}
