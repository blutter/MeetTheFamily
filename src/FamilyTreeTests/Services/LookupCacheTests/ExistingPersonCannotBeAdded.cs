using System;
using System.Data;
using FamilyTree.Model;
using FamilyTree.Services;
using FamilyTree.Services.ModelProcessing;
using Should;
using Xunit;

namespace FamilyTreeTests.Services.LookupCacheTests
{
    public class ExistingPersonCannotBeAdded
    {
        private readonly IPersonLookupCache _personLookupCache;
        private readonly Exception exception;

        public ExistingPersonCannotBeAdded()
        {
            _personLookupCache = GivenTheCacheHasAPerson();
            try
            {
                WhenAPersonWithTheSameNameIsAdded();
            }
            catch (Exception e)
            {
                exception = e;
            }
        }

        private IPersonLookupCache GivenTheCacheHasAPerson()
        {
            var cache = new PersonLookupCache();

            var person = Person.Create("Jane", Gender.Female);
            cache.AddPerson(person);

            return cache;
        }

        private void WhenAPersonWithTheSameNameIsAdded()
        {
            var doppelganger = Person.Create("Jane", Gender.Female);
            _personLookupCache.AddPerson(doppelganger);
        }

        [Fact]
        public void ThenAnExceptionIsThrown()
        {
            exception.ShouldBeType<DuplicateNameException>();
        }
    }
}
