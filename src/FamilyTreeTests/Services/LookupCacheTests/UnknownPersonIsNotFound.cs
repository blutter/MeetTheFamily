using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.LookupCacheTests
{
    public class UnknownPersonIsNotFound
    {
        private readonly IPersonLookupCache _personLookupCache;
        private readonly Person _foundPerson;

        public UnknownPersonIsNotFound()
        {
            _personLookupCache = GivenTheCacheHasAPerson();
            _foundPerson = WhenAnUnknownPersonIsLookedup();
        }

        private IPersonLookupCache GivenTheCacheHasAPerson()
        {
            var cache = new PersonLookupCache();

            var person = Person.Create("Jane", Gender.Female);
            cache.AddPerson(person);

            return cache;
        }

        private Person WhenAnUnknownPersonIsLookedup()
        {
            return _personLookupCache.GetPerson("John");
        }

        [Fact]
        public void ThenThePersonIsNotFoundInTheCache()
        {
            _foundPerson.Should().BeNull();
        }
    }
}
