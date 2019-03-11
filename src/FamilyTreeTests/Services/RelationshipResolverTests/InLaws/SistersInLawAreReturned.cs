using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests.InLaws
{
    public class SistersInLawAreReturned
    {
        private readonly Person _child;
        private readonly List<Person> _sistersInLaw;
        private IRelationshipResolver _relationshipResolver;

        public SistersInLawAreReturned()
        {
            _child = GivenAChildWithMarriedSiblings();
            _sistersInLaw = WhenTheSisterInLawsAreQueried();
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

        private List<Person> WhenTheSisterInLawsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_child, Relationship.SisterInLaw).ToList();
        }

        [Fact]
        public void ThenTheSpousesSistersAreReturned()
        {
            _sistersInLaw.Count.Should().Be(1);
            _sistersInLaw.Should().OnlyContain(person => person.Gender == Gender.Female);
        }

        [Fact]
        public void ThenTheBrotherInLawsAreReturnedInTheOrderTheirSpousesWereAdded()
        {
            _sistersInLaw.Select(inLaw => inLaw.Name).First().Should().Contain("Carol");
        }

    }
}
