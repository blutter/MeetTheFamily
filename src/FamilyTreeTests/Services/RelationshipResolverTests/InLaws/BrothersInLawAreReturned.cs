using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FamilyTree.Services.Relationships;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests.InLaws
{
    public class BrothersInLawAreReturned
    {
        private readonly Person _child;
        private readonly List<Person> _brotherInLaws;
        private IRelationshipResolver _relationshipResolver;

        public BrothersInLawAreReturned()
        {
            _child = GivenAChildWithMarriedSiblings();
            _brotherInLaws = WhenTheBrotherInLawsAreQueried();
        }

        private Person GivenAChildWithMarriedSiblings()
        {
            var parent = Person.Create("Eve", Gender.Female);
            parent.SetSpouse(Person.Create("Adam", Gender.Male));

            var marriedSister = Person.CreateChild(parent, "Alice", Gender.Female);
            var brotherInLaw1 = Person.Create("Anthony", Gender.Male);
            marriedSister.SetSpouse(brotherInLaw1);

            var child = Person.CreateChild(parent, "Bob", Gender.Male);

            var marriedBrother = Person.CreateChild(parent, "Christopher", Gender.Male);
            var sisterInLaw = Person.Create("Carol", Gender.Female);
            marriedBrother.SetSpouse(sisterInLaw);

            var marriedSister2 = Person.CreateChild(parent, "Diane", Gender.Female);
            var brotherInLaw2 = Person.Create("Doug", Gender.Male);
            marriedSister2.SetSpouse(brotherInLaw2);

            return child;
        }

        private List<Person> WhenTheBrotherInLawsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_child, Relationship.BrotherInLaw).ToList();
        }

        [Fact]
        public void ThenAllTheBrotherInLawsAreReturned()
        {
            _brotherInLaws.Count.Should().Be(2);
            _brotherInLaws.Should().OnlyContain(person => person.Gender == Gender.Male);
        }

        [Fact]
        public void ThenTheBrotherInLawsAreReturnedInTheOrderTheirSpousesWereAdded()
        {
            _brotherInLaws.First().Should().Match<Person>(person => person.Name == "Anthony");
            _brotherInLaws.Last().Should().Match<Person>(person => person.Name == "Doug");
        }

    }
}
