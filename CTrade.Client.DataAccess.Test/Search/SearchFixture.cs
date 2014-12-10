using CTrade.Client.DataAccess.Requests;
using Newtonsoft.Json.Linq;

namespace CTrade.Client.DataAccess.Test.Search
{
    internal class SearchFixture
    {
        private const string _views101 =
                "{" +
                    "\"_id\": \"_design/views101\"," +
                    "\"language\": \"javascript\"," +
                    "\"views\": {" +
                        "\"latin_name_jssum\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"," +
                            "\"reduce\": \"function (key, values, rereduce){\\n  return sum(values);\\n}\"" +
                        "}," +
                        "\"latin_name\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"" +
                        "}," +
                        "\"diet_sum\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet){\\n    emit(doc.diet, 1);\\n  }\\n}\"," +
                            "\"reduce\": \"_sum\"" +
                        "}," +
                        "\"diet_count\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet && doc.latinName){\\n    emit(doc.diet, doc.latinName);\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"complex_count\": {" +
                            "\"map\": \"function(doc){\\n  if(doc.class && doc.diet){\\n    emit([doc.class, doc.diet], 1);\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"diet\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet){\\n    emit(doc.diet, 1);\\n  }\\n}\"" +
                        "}," +
                        "\"complex_latin_name_count\": {" +
                            "\"map\": \"function(doc){\\n  if(doc.latinName){\\n    emit([doc.class, doc.diet, doc.latinName], doc.latinName.length)\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"diet_jscount\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet){\\n    emit(doc.diet, 1);\\n  }\\n}\"," +
                            "\"reduce\": \"function (key, values, rereduce){\\n  return values.length;\\n}\"" +
                        "}," +
                        "\"latin_name_count\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"latin_name_sum\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"," +
                            "\"reduce\": \"_sum\"" +
                        "}" +
                    "}," +
                    "\"indexes\": {" +
                        "\"animals\": {" +
                            "\"index\": \"function(doc){\\n" +
                            "  index('default', doc._id);\\n" +
                            "  if (doc.minLength){\\n    index('minLength', doc.minLength, {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.maxLength){\\n    index('maxLength', doc.maxLength, {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.diet){\\n    index('diet', doc.diet, {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.latinName){\\n    index('latinName', doc.latinName, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc['class']){\\n    index('class', doc['class'], {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n}\"" +
                        "}" +
                    "}" +
                "}";

        internal JObject[] Init(string connectionKey)
        {
            TestRuntime.EnsureCleanup(connectionKey);

            //Create documents
            var repository = TestRuntime.CreateRepository(connectionKey);
            JObject[] animals = CreateAllAnimals();
            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(animals);
            var createResult = repository.BulkAsync(bulkRequest).Result;

            //Create the view with search index
            var indexResult = repository.CreateAsync(JObject.Parse(_views101)).Result;

            return animals;
        }

        private JObject[] CreateAllAnimals()
        {
            return new JObject[]
            {
                CreatePanda(), CreateZebra(),
                CreateSnipe(), CreateLlama(),
                CreateLemur(), CreateKookaBurra(),
                CreateGiraffe(), CreateElephant(),
                CreateBadger(), CreateAardvark()
            };
        }

        private JObject CreateAardvark()
        {
            dynamic animal = new JObject();

            animal._id = "aardvark";
            animal.@class = "mammal";
            animal.latinName = "Orycteropus afer";
            animal.diet = "omnivore";
            animal.minLength = 1;
            animal.maxLength = 2.2;
            animal.minWeight = 40;
            animal.maxWeight = 65;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Aardvark";

            return animal;
        }

        private JObject CreateBadger()
        {
            dynamic animal = new JObject();

            animal._id = "badger";
            animal.@class = "mammal";
            animal.latinName = "Meles meles";
            animal.diet = "omnivore";
            animal.minLength = 0.6;
            animal.maxLength = 0.9;
            animal.minWeight = 7;
            animal.maxWeight = 30;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Badger";

            return animal;
        }

        private JObject CreateElephant()
        {
            dynamic animal = new JObject();

            animal._id = "elephant";
            animal.@class = "mammal";
            animal.diet = "herbivore";
            animal.minLength = 3.2;
            animal.maxLength = 4;
            animal.minWeight = 4700;
            animal.maxWeight = 6050;
            animal.wikiPage = "http://en.wikipedia.org/wiki/African_elephant";

            return animal;
        }

        private JObject CreateGiraffe()
        {
            dynamic animal = new JObject();

            animal._id = "giraffe";
            animal.@class = "mammal";
            animal.diet = "herbivore";
            animal.minLength = 5;
            animal.maxLength = 6;
            animal.minWeight = 830;
            animal.maxWeight = 1600;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Giraffe";

            return animal;
        }

        private JObject CreateKookaBurra()
        {
            dynamic animal = new JObject();

            animal._id = "kookaburra";
            animal.@class = "bird";
            animal.latinName = "Dacelo novaeguineae";
            animal.diet = "carnivore";
            animal.minLength = 0.28;
            animal.maxLength = 0.42;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Kookaburra";

            return animal;
        }

        private JObject CreateLemur()
        {
            dynamic animal = new JObject();

            animal._id = "lemur";
            animal.@class = "mammal";
            animal.diet = "omnivore";
            animal.minLength = 0.95;
            animal.maxLength = 1.1;
            animal.minWeight = 2.2;
            animal.maxWeight = 2.2;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Ring-tailed_lemur";

            return animal;
        }

        private JObject CreateLlama()
        {
            dynamic animal = new JObject();

            animal._id = "llama";
            animal.@class = "mammal";
            animal.latinName = "Lama glama";
            animal.diet = "herbivore";
            animal.minLength = 1.7;
            animal.maxLength = 1.8;
            animal.minWeight = 130;
            animal.maxWeight = 200;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Llama";

            return animal;
        }

        private JObject CreateSnipe()
        {
            dynamic animal = new JObject();

            animal._id = "snipe";
            animal.@class = "bird";
            animal.latinName = "Gallinago gallinago";
            animal.diet = "omnivore";
            animal.minLength = 0.25;
            animal.maxLength = 0.27;
            animal.minWeight = 0.08;
            animal.maxWeight = 0.14;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Common_Snipe";

            return animal;
        }

        public static JObject CreatePanda()
        {
            dynamic animal = new JObject();

            animal._id = "panda";
            animal.@class = "mammal";
            animal.diet = "carnivore";
            animal.minLength = 1.2;
            animal.maxLength = 1.8;
            animal.minWeight = 75;
            animal.maxWeight = 115;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Panda";

            return animal;
        }

        private JObject CreateZebra()
        {
            dynamic animal = new JObject();

            animal._id = "zebra";
            animal.@class = "mammal";
            animal.diet = "herbivore";
            animal.minLength = 2;
            animal.maxLength = 2.5;
            animal.minWeight = 175;
            animal.maxWeight = 387;
            animal.wikiPage = "http://en.wikipedia.org/wiki/Plains_zebra";

            return animal;
        }
    }
}
