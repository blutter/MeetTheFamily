using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests.AuntsAndUncles
{
    public class MaternalAuntsAreReturned
    {
        private readonly Person _person;
        private readonly List<Person> _maternalAunts;
        private IRelationshipResolver _relationshipResolver;

        public MaternalAuntsAreReturned()
        {
            _person = GivenAPersonWithAuntsAndUncles();
            _maternalAunts = WhenTheMaternalAuntsAreQueried();
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

        private List<Person> WhenTheMaternalAuntsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_person, Relationship.MaternalAunt).ToList();
        }

        [Fact]
        public void ThenAllTheMaternalAuntsAreReturned()
        {
            _maternalAunts.Count.Should().Be(2);
            _maternalAunts.All(person => person.IsFemale).Should().BeTrue();
        }

        [Fact]
        public void ThenTheMaternalAuntsAreInTheOrderTheyWereAdded()
        {
            _maternalAunts.Select(person => person.Name).Should().ContainInOrder("Carol", "Ellie");
        }
    }
}
