using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using FamilyTree.Services.Relationships;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class DaughtersAreReturned
    {
        private readonly Person _parent;
        private readonly List<Person> _daughters;
        private IRelationshipResolver _relationshipResolver;

        public DaughtersAreReturned()
        {
            _parent = GivenAParentWithChildren();
            _daughters = WhenTheDaughtersAreQueried();
        }

        private Person GivenAParentWithChildren()
        {
            var parent = Person.Create("Jane", Gender.Female);
            parent.SetSpouse(Person.Create("John", Gender.Male));

            Person.CreateChild(parent, "Alice", Gender.Female);
            Person.CreateChild(parent, "Bob", Gender.Male);
            Person.CreateChild(parent, "Christopher", Gender.Male);
            Person.CreateChild(parent, "Diane", Gender.Female);

            return parent;
        }

        private List<Person> WhenTheDaughtersAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();

            return _relationshipResolver.GetRelations(_parent, Relationship.Daughter).ToList();
        }

        [Fact]
        public void ThenAllTheDaughtersAreReturned()
        {
            _daughters.Count.Should().Be(2);
        }

        [Fact]
        public void ThenOnlyFemalesAreReturned()
        {
            _daughters.Should().OnlyContain(person => person.Gender == Gender.Female);
        }

        [Fact]
        public void ThenTheDaughtersAreReturnedInTheOrderTheyWereAdded()
        {
            _daughters.First().Should().Match<Person>(person => person.Name == "Alice");
            _daughters.Last().Should().Match<Person>(person => person.Name == "Diane");
        }
    }
}
