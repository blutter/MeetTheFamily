using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests.AuntsAndUncles
{
    public class MaternalUnclesAreReturned
    {
        private readonly Person _person;
        private readonly List<Person> _maternalUncles;
        private IRelationshipResolver _relationshipResolver;

        public MaternalUnclesAreReturned()
        {
            _person = GivenAPersonWithAuntsAndUncles();
            _maternalUncles = WhenTheMaternalUnclesAreQueried();
        }

        private Person GivenAPersonWithAuntsAndUncles()
        {
            var grandma = Person.Create("Jane", Gender.Female);
            grandma.SetSpouse(Person.Create("John", Gender.Male));

            var mother = Person.CreateChild(grandma, "Alice the first", Gender.Female);
            var father = Person.Create("Aaron", Gender.Male);
            mother.SetSpouse(father);

            var uncle1 = Person.CreateChild(grandma, "Bob", Gender.Male);
            var aunt1 = Person.CreateChild(grandma, "Carol", Gender.Female);
            var uncle2 = Person.CreateChild(grandma, "Doug", Gender.Male);
            var aunt2 = Person.CreateChild(grandma, "Ellie", Gender.Female);

            var person = Person.CreateChild(mother, "Alice the second", Gender.Female);

            return person;
        }

        private List<Person> WhenTheMaternalUnclesAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_person, Relationship.MaternalUncle).ToList();
        }

        [Fact]
        public void ThenAllTheMaternalUnclesAreReturned()
        {
            _maternalUncles.Count.Should().Be(2);
            _maternalUncles.All(person => person.IsMale).Should().BeTrue();
        }

        [Fact]
        public void ThenTheMaternalUnclesAreInTheOrderTheyWereAdded()
        {
            _maternalUncles.Select(person => person.Name).Should().ContainInOrder("Bob", "Doug");
        }
    }
}
