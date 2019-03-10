using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class BrothersInLawForAFemaleAreReturned
    {
        private readonly Person _female;
        private readonly List<Person> _brotherInLaws;
        private IRelationshipResolver _relationshipResolver;

        public BrothersInLawForAFemaleAreReturned()
        {
            _female = GivenAFemaleWithMarriedSiblings();
            _brotherInLaws = WhenTheBrotherInLawsAreQueried();
        }

        private Person GivenAFemaleWithMarriedSiblings()
        {
            var parent = Person.Create("Eve", Gender.Female);
            parent.SetSpouse(Person.Create("Adam", Gender.Male));

            var marriedSister = Person.CreateChild(parent, "Alice", Gender.Female);
            var brotherInLaw1 = Person.Create("Anthony", Gender.Male);
            marriedSister.SetSpouse(brotherInLaw1);

            var female = Person.CreateChild(parent, "Barbara", Gender.Female);
            var spouse = Person.Create("Bob", Gender.Male);
            female.SetSpouse(spouse);

            var marriedBrother = Person.CreateChild(parent, "Christopher", Gender.Male);
            var sisterInLaw = Person.Create("Carol", Gender.Female);
            marriedBrother.SetSpouse(sisterInLaw);

            var marriedSister2 = Person.CreateChild(parent, "Diane", Gender.Female);
            var brotherInLaw2 = Person.Create("Doug", Gender.Male);
            marriedSister2.SetSpouse(brotherInLaw2);

            return female;
        }

        private List<Person> WhenTheBrotherInLawsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_female, Relationship.BrotherInLaws).ToList();
        }

        [Fact]
        public void ThenAllTheBrotherInLawsAreReturned()
        {
            _brotherInLaws.Count.Should().Be(2);
            _brotherInLaws.Should().OnlyContain(person => person.Gender == Gender.Male);
        }

        [Fact]
        public void ThenTheFemalesSpouseIsNotABrotherInLaw()
        {
            _brotherInLaws.Where(brother => brother.Name == "Bob").Should().BeEmpty();
        }

        [Fact]
        public void ThenTheBrotherInLawsAreReturnedInTheOrderTheirSpousesWereAdded()
        {
            _brotherInLaws.First().Should().Match<Person>(person => person.Name == "Anthony");
            _brotherInLaws.Last().Should().Match<Person>(person => person.Name == "Doug");
        }
    }
}
