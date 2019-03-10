using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class SonsAreReturned
    {
        private readonly Person _parent;
        private readonly List<Person> _sons;
        private IRelationshipResolver _relationshipResolver;

        public SonsAreReturned()
        {
            _parent = GivenAParentWithChildren();
            _sons = WhenTheSonsAreQueried();
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

        private List<Person> WhenTheSonsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();

            return _relationshipResolver.GetRelations(_parent, Relationship.Sons).ToList();
        }

        [Fact]
        public void ThenAllTheSonsAreReturned()
        {
            _sons.Count.Should().Be(2);
        }

        [Fact]
        public void ThenOnlyMalesAreReturned()
        {
            _sons.Should().OnlyContain(person => person.Gender == Gender.Male);
        }

        [Fact]
        public void ThenTheSonsAreReturnedInTheOrderTheyWereAdded()
        {
            _sons.First().Should().Match<Person>(person => person.Name == "Bob");
            _sons.Last().Should().Match<Person>(person => person.Name == "Christopher");
        }
    }
}
