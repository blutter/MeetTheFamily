using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FamilyTree.Services.ModelProcessing;
using FamilyTree.Services.Relationships;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.ModelProcessorTests
{
    public class ReferenceModelTests
    {
        private readonly ModelProcessor _modelProcessor;

        public ReferenceModelTests()
        {
            _modelProcessor = new ModelProcessor(new PersonLookupCache(), new RelationshipResolver(),
                "FamilyTree.ReferenceModel.arthur-clan.txt");
        }

        [Fact]
        public void GivenTheReferenceModel_WhenHarrysSonAreQueried_ThenJamesAndAlbusAreReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var harrysSons = _modelProcessor.GetRelationsForPerson("Harry", Relationship.Son);

            // ASSERT
            harrysSons.First().Should().Match<Person>(person => person.Name == "James");
            harrysSons.Last().Should().Match<Person>(person => person.Name == "Albus");

            harrysSons.All(son => son.IsMale).Should().BeTrue();
            harrysSons.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenHelensDaughtersAreQueried_ThenRoseIsReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var helensDaughters = _modelProcessor.GetRelationsForPerson("Helen", Relationship.Daughter);

            // ASSERT
            helensDaughters.First().Should().Match<Person>(person => person.Name == "Rose");

            helensDaughters.All(son => son.IsFemale).Should().BeTrue();
            helensDaughters.Count().Should().Be(1);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenRonaldsSiblingsAreQueried_ThenHisFourSiblingsAreReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var ronaldsSiblings = _modelProcessor.GetRelationsForPerson("Ronald", Relationship.Siblings);

            // ASSERT
            ronaldsSiblings.First()
                .Should().Match<Person>(person => person.Name == "Bill" && person.IsMale);
            ronaldsSiblings.Skip(1).First()
                .Should().Match<Person>(person => person.Name == "Charlie" && person.IsMale);
            ronaldsSiblings.Skip(2).First()
                .Should().Match<Person>(person => person.Name == "Percy" && person.IsMale);
            ronaldsSiblings.Skip(3).First()
                .Should().Match<Person>(person => person.Name == "Ginerva" && person.IsFemale);

            ronaldsSiblings.Count().Should().Be(4);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenRonaldsSistersInLawAreQueried_ThenFloraAndAudreyAreReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var ronaldsSisterInLaws = _modelProcessor.GetRelationsForPerson("Ronald", Relationship.SisterInLaw);

            // ASSERT
            ronaldsSisterInLaws.Select(person => person.Name).Should().ContainInOrder("Flora", "Audrey");

            ronaldsSisterInLaws.All(son => son.IsFemale).Should().BeTrue();
            ronaldsSisterInLaws.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenHugosBrothersInLawAreQueried_ThenMalfoyIsReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var hugosBrothesInLaw = _modelProcessor.GetRelationsForPerson("Hugo", Relationship.BrotherInLaw);

            // ASSERT
            hugosBrothesInLaw.First()
                .Should().Match<Person>(person => person.Name == "Malfoy");

            hugosBrothesInLaw.All(son => son.IsMale).Should().BeTrue();
            hugosBrothesInLaw.Count().Should().Be(1);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenRemusMaternalAuntsAreQueried_ThenDominiqueIsReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var remusMaternalAunts = _modelProcessor.GetRelationsForPerson("Remus", Relationship.MaternalAunt);

            // ASSERT
            remusMaternalAunts.First()
                .Should().Match<Person>(person => person.Name == "Dominique");

            remusMaternalAunts.All(son => son.IsFemale).Should().BeTrue();
            remusMaternalAunts.Count().Should().Be(1);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenWilliamsPaternalAuntsAreQueried_ThenLilyIsReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var WilliamsPaternalAunts = _modelProcessor.GetRelationsForPerson("William", Relationship.PaternalAunt);

            // ASSERT
            WilliamsPaternalAunts.First()
                .Should().Match<Person>(person => person.Name == "Lily");

            WilliamsPaternalAunts.All(son => son.IsFemale).Should().BeTrue();
            WilliamsPaternalAunts.Count().Should().Be(1);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenJamesMaternalUnclesAreQueried_ThenGinervasFourBrothersAreReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var jamesMaternalUncles = _modelProcessor.GetRelationsForPerson("James", Relationship.MaternalUncle);

            // ASSERT
            jamesMaternalUncles.First()
                .Should().Match<Person>(person => person.Name == "Bill");
            jamesMaternalUncles.Skip(1).First()
                .Should().Match<Person>(person => person.Name == "Charlie");
            jamesMaternalUncles.Skip(2).First()
                .Should().Match<Person>(person => person.Name == "Percy");
            jamesMaternalUncles.Skip(3).First()
                .Should().Match<Person>(person => person.Name == "Ronald");

            jamesMaternalUncles.All(son => son.IsMale).Should().BeTrue();
            jamesMaternalUncles.Count().Should().Be(4);
        }

        [Fact]
        public void GivenTheReferenceModel_WhenHugosPaternalUnclesAreQueried_ThenRonaldsThreeBrothersAreReturned()
        {
            // ARRANGE
            // noop

            // ACT
            var williamsPaternalUncles = _modelProcessor.GetRelationsForPerson("Hugo", Relationship.PaternalUncle);

            // ASSERT
            williamsPaternalUncles.First()
                .Should().Match<Person>(person => person.Name == "Bill");
            williamsPaternalUncles.Skip(1).First()
                .Should().Match<Person>(person => person.Name == "Charlie");
            williamsPaternalUncles.Skip(2).First()
                .Should().Match<Person>(person => person.Name == "Percy");

            williamsPaternalUncles.All(son => son.IsMale).Should().BeTrue();
            williamsPaternalUncles.Count().Should().Be(3);
        }
    }
}