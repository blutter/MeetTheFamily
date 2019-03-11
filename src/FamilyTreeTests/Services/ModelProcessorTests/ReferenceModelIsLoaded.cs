using System.Linq;
using FamilyTree.Model;
using FamilyTree.Services;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Services.ModelProcessorTests
{
    public class ReferenceModelIsLoaded
    {
        private readonly ModelProcessor _modelProcessor;

        public ReferenceModelIsLoaded()
        {
            _modelProcessor = new ModelProcessor(new PersonLookupCache(), new RelationshipResolver(),
                "FamilyTree.ReferenceModel.arthur-clan.txt");
        }

        [Fact]
        public void GivenTheReferenceModel_WhenHarrysSonAreQueries_ThenJamesAndAlbusAreReturned()
        {
            // ARRANGE

            // ACT
            var harrysSons = _modelProcessor.GetRelationsForPerson("Harry", Relationship.Sons);

            // ASSERT
            harrysSons.First().Should().Match<Person>(person => person.Name == "James");
            harrysSons.Last().Should().Match<Person>(person => person.Name == "Albus");

            harrysSons.All(son => son.IsMale).Should().BeTrue();
            harrysSons.Count().Should().Be(2);
        }
    }
}
