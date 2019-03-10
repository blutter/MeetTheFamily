using FamilyTree.Model;
using FluentAssertions;
using SpecsFor.StructureMap;
using Xunit;

namespace FamilyTreeTests.Model
{
    public class PersonEntityCreationTests : SpecsFor<Person>
    {
        private Person _person;

        public PersonEntityCreationTests()
        {
            When();
        }

        protected override void When()
        {
            _person = Person.Create("John Doe", Gender.Male);
        }

        [Fact]
        void ThenThePersonHasTheCorrectName()
        {
            _person.Name.Should().Be("John Doe");
        }

        [Fact]
        void ThenThePersonHasTheCorrectGender()
        {
            _person.Gender.Should().Be(Gender.Male);
        }
    }
}
