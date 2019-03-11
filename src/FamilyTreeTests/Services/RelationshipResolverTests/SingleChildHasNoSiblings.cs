using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using FamilyTree.Services.Relationships;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class SingleChildHasNoSiblings
    {
        private readonly Person _child;
        private readonly List<Person> _siblings;
        private IRelationshipResolver _relationshipResolver;

        public SingleChildHasNoSiblings()
        {
            _child = GivenAChildWithNoSiblings();
            _siblings = WhenTheSiblingsAreQueried();
        }

        private Person GivenAChildWithNoSiblings()
        {
            var parent = Person.Create("Jane", Gender.Female);
            parent.SetSpouse(Person.Create("John", Gender.Male));

            var child = Person.CreateChild(parent, "Bob", Gender.Male);
            return child;
        }

        private List<Person> WhenTheSiblingsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_child, Relationship.Siblings).ToList();
        }

        [Fact]
        public void ThenAllNoSiblingsAreReturned()
        {
            _siblings.Should().BeEmpty();
        }
    }
}
