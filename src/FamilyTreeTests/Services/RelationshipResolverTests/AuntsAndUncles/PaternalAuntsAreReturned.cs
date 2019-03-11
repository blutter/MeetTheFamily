using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests.AuntsAndUncles
{
    public class PaternalAuntsAreReturned
    {
        private readonly Person _person;
        private readonly List<Person> _paternalAunts;
        private IRelationshipResolver _relationshipResolver;

        public PaternalAuntsAreReturned()
        {
            _person = GivenAPersonWithAuntsAndUncles();
            _paternalAunts = WhenThePaternalAuntsAreQueried();
        }

        private Person GivenAPersonWithAuntsAndUncles()
        {
            var grandma = Person.Create("Jane", Gender.Female);
            grandma.SetSpouse(Person.Create("John", Gender.Male));

            var father = Person.CreateChild(grandma, "Aaron", Gender.Male);
            var mother = Person.Create("Alice", Gender.Female);
            mother.SetSpouse(father);

            var uncle1 = Person.CreateChild(grandma, "Bob", Gender.Male);
            var aunt1 = Person.CreateChild(grandma, "Carol", Gender.Female);
            var uncle2 = Person.CreateChild(grandma, "Doug", Gender.Male);
            var aunt2 = Person.CreateChild(grandma, "Ellie", Gender.Female);

            var person = Person.CreateChild(mother, "Aaron the second", Gender.Male);

            return person;
        }

        private List<Person> WhenThePaternalAuntsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_person, Relationship.PaternalAunt).ToList();
        }

        [Fact]
        public void ThenAllThePaternalAuntsAreReturned()
        {
            _paternalAunts.Count.Should().Be(2);
            _paternalAunts.All(person => person.IsFemale).Should().BeTrue();
        }

        [Fact]
        public void ThenThePaternalAuntsAreInTheOrderTheyWereAdded()
        {
            _paternalAunts.Select(person => person.Name).Should().ContainInOrder("Carol", "Ellie");
        }
    }
}
