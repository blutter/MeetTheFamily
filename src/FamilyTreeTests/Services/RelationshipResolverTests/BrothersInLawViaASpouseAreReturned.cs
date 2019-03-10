using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests
{
    public class BrothersInLawViaASpouseAreReturned
    {
        private readonly Person _child;
        private readonly List<Person> _brotherInLaws;
        private IRelationshipResolver _relationshipResolver;

        public BrothersInLawViaASpouseAreReturned()
        {
            _child = GivenAChildWithMarriedSiblingsAndASpouseWithBrothersInLaw();
            _brotherInLaws = WhenTheBrotherInLawsAreQueried();
        }

        private Person GivenAChildWithMarriedSiblingsAndASpouseWithBrothersInLaw()
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

            AddSpouseWithBrothersInLaw(child);

            return child;
        }

        private void AddSpouseWithBrothersInLaw(Person person)
        {
            var spousesMother = Person.Create("Barbara's Mum", Gender.Female);
            var spousesFather = Person.Create("Barbara's Dad", Gender.Male);
            spousesMother.SetSpouse(spousesFather);

            var spouse = Person.CreateChild(spousesMother, "Barbara", Gender.Female);
            var spousesSister = Person.CreateChild(spousesMother, "Barbara's sister", Gender.Female);
            var spousesBrotherInLaw = Person.Create("Barbara's brother in law", Gender.Male);
            spousesSister.SetSpouse(spousesBrotherInLaw);

            var spousesBrother = Person.CreateChild(spousesMother, "Barbara's brother", Gender.Male);
            var spousesSisterInLaw = Person.Create("Barbara's sister in law", Gender.Female);
            spousesBrother.SetSpouse(spousesSisterInLaw);

            person.SetSpouse(spouse);
        }

        private List<Person> WhenTheBrotherInLawsAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_child, Relationship.BrotherInLaws).ToList();
        }

        [Fact]
        public void ThenAllTheBrotherInLawsAreReturned()
        {
            _brotherInLaws.Count.Should().Be(3);
            _brotherInLaws.Should().OnlyContain(person => person.Gender == Gender.Male);
        }

        [Fact]
        public void ThenTheBrotherInLawsAreReturnedInTheOrderTheirSpousesWereAdded()
        {
            _brotherInLaws.First().Should().Match<Person>(person => person.Name == "Anthony");
            _brotherInLaws.Skip(1).First().Should().Match<Person>(person => person.Name == "Doug");
            _brotherInLaws.Skip(2).First().Should().Match<Person>(person => person.Name == "Barbara's brother");
        }

    }
}
