using CTrade.Client.DataAccess.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CTrade.Client.DataAccess.Test.Search
{
    [TestClass]
    public class SearchTests : TestBase
    {
        private JObject[] _animals;
        
        [TestInitialize]
        public void Setup()
        {
            _animals = new SearchFixture().Init(ConnectionKey);
        }

        [TestMethod]
        public void TestBasicSearch()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore"));

            var searchResp = Repository.SearchAsync(searchReq).Result;

            Assert.IsFalse(searchResp.IsEmpty);
            var fieldList = searchResp.Rows.Select(r => new { Class = (string)r.Fields["class"], Diet = (string)r.Fields["diet"] });
            Assert.IsTrue(fieldList.All(f => f.Class == "bird" && f.Diet == "carnivore"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptySearchExpressionShouldReportError()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression(string.Empty));
        }

        [TestMethod]
        public void TestBasicSearchWithIncludedDoc()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore")
                .IncludeDocs(true)
                );

            var searchResp = Repository.SearchAsync(searchReq).Result;

            Assert.IsFalse(searchResp.IsEmpty);
            var fieldList = searchResp.Rows.Select(r => new { Class = (string)r.Fields["class"],
                Diet = (string)r.Fields["diet"], Doc = r.IncludedDoc });
            Assert.IsTrue(fieldList.All(f => f.Class == "bird" && f.Diet == "carnivore" && f.Doc != null));
        }

        [TestMethod]
        public void TestLimitReturnsRequestedNumberOfRows()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore")
                .Limit(2)
                );

            var searchResp = Repository.SearchAsync(searchReq).Result;

            Assert.IsTrue(searchResp.RowCount <= 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCountsCannotBeAnEmptyListWhenSpecified()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore")
                .Counts()
                );
        }

        [TestMethod]
        public void TestSearchWithCounts()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore")
                .Counts("diet", "class")
                );

            var searchResp = Repository.SearchAsync(searchReq).Result;
            dynamic counts = searchResp.Counts;

            Assert.IsNotNull(counts);
            Assert.IsNotNull(counts.@class);
            Assert.AreEqual(1.0, (double)counts.@class.bird);
            Assert.AreEqual(1.0, (double)counts.diet.carnivore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRangesCannotBeNullWhenSpecified()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore")
                .Ranges(null)
                );
        }

        [TestMethod]
        public void TestSearchWithRanges()
        {
            var rangeoptions = new
            {
                minLength = new { minlight = "[0 TO 100]", minheavy = "{101 TO Infinity}" },
                maxLength = new { maxlight = "[0 TO 100]", maxheavy = "{101 TO Infinity}" }
            };

            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird AND diet:carnivore")
                .Ranges(rangeoptions)
                );

            var searchResp = Repository.SearchAsync(searchReq).Result;
            dynamic ranges = searchResp.Ranges;

            Assert.IsNotNull(ranges);
            Assert.IsNotNull(ranges.maxLength);
            Assert.IsNotNull(ranges.maxLength.maxlight);
            Assert.IsNotNull(ranges.maxLength.maxheavy);
            Assert.IsNotNull(ranges.minLength);
            Assert.IsNotNull(ranges.minLength.minlight);
            Assert.IsNotNull(ranges.minLength.minheavy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGroupFieldCannotBeEmptyWhenSpecified()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird")
                .GroupField(string.Empty)
                );
        }

        [TestMethod]
        public void TestSearchWithGroups()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird")
                .GroupField("diet")
                );

            var searchResp = Repository.SearchAsync(searchReq).Result;

            Assert.IsTrue(searchResp.IsEmpty);
            Assert.IsFalse(searchResp.IsGroupsEmpty);
            Assert.AreEqual(2, searchResp.GroupCount);
            var firstGroup = searchResp.Groups[0];
            Assert.AreEqual("carnivore", firstGroup.By);
            Assert.IsFalse(firstGroup.IsEmpty);
            Assert.AreEqual(1, firstGroup.RowCount);
            var row = firstGroup.Rows.First();
            Assert.IsFalse(string.IsNullOrWhiteSpace(row.Id));
            Assert.AreEqual("carnivore", (string)row.Fields["diet"]);
            Assert.AreEqual("bird", (string)row.Fields["class"]);
            var secondGroup = searchResp.Groups[1];
            Assert.AreEqual("omnivore", secondGroup.By);
            Assert.IsFalse(secondGroup.IsEmpty);
            Assert.AreEqual(1, secondGroup.RowCount);
            row = secondGroup.Rows.First();
            Assert.IsFalse(string.IsNullOrWhiteSpace(row.Id));
            Assert.AreEqual("omnivore", (string)row.Fields["diet"]);
            Assert.AreEqual("bird", (string)row.Fields["class"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBookMarkCannotBeEmptyWhenSpecified()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird")
                .Bookmark(string.Empty)
                );
        }

        [TestMethod]
        public void TestLimitAndBookMark()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:bird")
                .Limit(1)
                );

            var firstSearchResp = Repository.SearchAsync(searchReq).Result;

            searchReq.Configure(q => q.Bookmark(firstSearchResp.Bookmark));
            var secondSearchResp = Repository.SearchAsync(searchReq).Result;
            
            //Expect one more
            Assert.AreEqual(1, secondSearchResp.RowCount);

            searchReq.Configure(q => q.Bookmark(secondSearchResp.Bookmark));
            var thirdSearchResp = Repository.SearchAsync(searchReq).Result;

            //There aren't anymore, so same bookmark gets returned.
            Assert.IsTrue(thirdSearchResp.IsEmpty);
            Assert.AreEqual(secondSearchResp.Bookmark, thirdSearchResp.Bookmark);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSortCannotBeAnEmptyListWhenSpecified()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:mammal AND diet:omnivore")
                .Sort()
                );
        }

        [TestMethod]
        public void TestSortSingleNumericField()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("class:mammal AND diet:omnivore")
                .Sort("-maxLength")
                );

            var sortResp = Repository.SearchAsync(searchReq).Result;

            var fieldList = sortResp.Rows.Select(r => new { MaxLength = r.Fields["maxLength"] });
            var sortedFieldList = fieldList.OrderByDescending(f => f.MaxLength);
            Assert.IsTrue(sortedFieldList.SequenceEqual(fieldList));
        }

        [TestMethod]
        public void TestSortWithStringFieldThenByNumericDescending()
        {
            var searchReq = SearchRequestFactory.Create("views101", "animals");
            searchReq.Configure(q => q.Expression("diet:omnivore")
                .Sort("class", "-maxLength")
                );

            var sortResp = Repository.SearchAsync(searchReq).Result;

            var fieldList = sortResp.Rows.Select(r => new { Class = r.Fields["class"], MaxLength = r.Fields["maxLength"] });
            var sortedFieldList = fieldList.OrderBy(f => f.Class).ThenByDescending(f => f.MaxLength);
            Assert.IsTrue(sortedFieldList.SequenceEqual(fieldList));
        }
    }
}
