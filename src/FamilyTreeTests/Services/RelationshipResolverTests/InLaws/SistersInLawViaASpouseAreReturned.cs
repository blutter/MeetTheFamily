using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.RelationshipResolverTests.InLaws
{
    public class SistersInLawViaASpouseAreReturned
    {
        private readonly Person _person;
        private readonly List<Person> _sistersInLaw;
        private IRelationshipResolver _relationshipResolver;

        public SistersInLawViaASpouseAreReturned()
        {
            _person = GivenAPersonWithASpouseWithSistersInLaw();
            _sistersInLaw = WhenTheSistersInLawAreQueried();
        }

        private Person GivenAPersonWithASpouseWithSistersInLaw()
        {
            var parent = Person.Create("Eve", Gender.Female);
            parent.SetSpouse(Person.Create("Adam", Gender.Male));

            var sisterInLaw1 = Person.CreateChild(parent, "Alice", Gender.Female);
            var brotherInLaw1 = Person.Create("Anthony", Gender.Male);
            sisterInLaw1.SetSpouse(brotherInLaw1);

            var spouse = Person.CreateChild(parent, "Bob", Gender.Male);
            var person = Person.Create("Barbara", Gender.Female);
            person.SetSpouse(spouse);
            
            var brotherInLaw2 = Person.CreateChild(parent, "Christopher", Gender.Male);
            var sisterInLaw2 = Person.Create("Carol", Gender.Female);
            brotherInLaw2.SetSpouse(sisterInLaw2);

            var sisterInLaw3 = Person.CreateChild(parent, "Diane", Gender.Female);
            var brotherInLaw3 = Person.Create("Doug", Gender.Male);
            sisterInLaw3.SetSpouse(brotherInLaw3);

            return person;
        }

        private List<Person> WhenTheSistersInLawAreQueried()
        {
            _relationshipResolver = new RelationshipResolver();
            return _relationshipResolver.GetRelations(_person, Relationship.SisterInLaw).ToList();
        }

        [Fact]
        public void ThenTheSpousesSistersAreReturned()
        {
            _sistersInLaw.Count.Should().Be(2);
            _sistersInLaw.Should().OnlyContain(person => person.IsFemale);
        }

        [Fact]
        public void ThenTheSistersInLawAreReturnedInTheOrderTheirSpousesWereAdded()
        {
            _sistersInLaw.Select(inlaw => inlaw.Name).First()
                .Should().Be("Alice");
            _sistersInLaw.Select(inlaw => inlaw.Name).Skip(1).First()
                .Should().Be("Diane");
        }

    }
}
