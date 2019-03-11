using FamilyTree.Model;
using FamilyTree.Services;
using FamilyTree.Services.ModelProcessing;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.LookupCacheTests
{
    public class NewPersonCanBeAdded
    {
        private readonly IPersonLookupCache _personLookupCache;
        private readonly Person _foundPerson;

        public NewPersonCanBeAdded()
        {
            _personLookupCache = GivenTheCacheHasAPerson();
            _foundPerson = WhenAPersonInTheCacheIsLookedup();
        }

        private IPersonLookupCache GivenTheCacheHasAPerson()
        {
            var cache = new PersonLookupCache();

            var person = Person.Create("Jane", Gender.Female);
            cache.AddPerson(person);

            return cache;
        }

        private Person WhenAPersonInTheCacheIsLookedup()
        {
            return _personLookupCache.GetPerson("Jane");
        }

        [Fact]
        public void ThenThePersonIsFoundInTheCache()
        {
            _foundPerson.Should().Match<Person>(person => person.Name == "Jane" && person.IsFemale);
        }
    }
}
