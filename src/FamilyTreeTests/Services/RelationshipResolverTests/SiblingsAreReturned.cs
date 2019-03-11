using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class MaternalAuntsAreReturned
    {
        private readonly Person _child;
        private readonly List<Person> _siblings;
        private IRelationshipResolver _relationshipResolver;

        public MaternalAuntsAreReturned()
        {
            _child = GivenAChildWithSiblings();
            _siblings = WhenTheSiblingsOfAChildAreQueried();
        }

        private Person GivenAChildWithSiblings()
        {
            var parent = Person.Create("Jane", Gender.Female);
            parent.SetSpouse(Person.Create("John", Gender.Male));

            Person.CreateChild(parent, "Alice", Gender.Female);
            var child = Person.CreateChild(parent, "Bob", Gender.Male);
            Person.CreateChild(parent, "Christopher", Gender.Male);
            Person.CreateChild(parent, "Diane", Gender.Female);

            return child;
        }

        private List<Person> WhenTheSiblingsOfAChildAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_child, Relationship.Siblings).ToList();
        }

        [Fact]
        public void ThenAllTheSiblingsAreReturned()
        {
            _siblings.Count.Should().Be(3);
        }

        [Fact]
        public void ThenTheSiblingsAreReturnedInTheOrderTheyWereAdded()
        {
            _siblings.First().Should()
                .Match<Person>(person => person.Name == "Alice" && person.Gender == Gender.Female);
            _siblings.Skip(1).First().Should()
                .Match<Person>(person => person.Name == "Christopher" && person.Gender == Gender.Male);
            _siblings.Skip(2).First().Should()
                .Match<Person>(person => person.Name == "Diane" && person.Gender == Gender.Female);
        }
    }
}
