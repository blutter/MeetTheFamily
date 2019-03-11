using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class SpouseOutsideTheDirectFamilyHasNoSiblings
    {
        private readonly Person _spouse;
        private readonly List<Person> _siblings;
        private IRelationshipResolver _relationshipResolver;

        public SpouseOutsideTheDirectFamilyHasNoSiblings()
        {
            _spouse = GivenASpouseWhoIsOutsideOfTheDirectFamilyTree();
            _siblings = WhenTheSiblingsAreQueried();
        }

        private Person GivenASpouseWhoIsOutsideOfTheDirectFamilyTree()
        {
            var personInFamily = Person.Create("Jane", Gender.Female);
            var spouse = Person.Create("John", Gender.Male);
            personInFamily.SetSpouse(spouse);

            return spouse;
        }

        private List<Person> WhenTheSiblingsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_spouse, Relationship.Siblings).ToList();
        }

        [Fact]
        public void ThenAllNoSiblingsAreReturned()
        {
            _siblings.Should().BeEmpty();
        }
    }
}
