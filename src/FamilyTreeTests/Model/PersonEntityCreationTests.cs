using FamilyTree.Model;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Model
{
    public class PersonEntityCreationTests
    {
        private Person _person;

        public PersonEntityCreationTests()
        {
            WhenAPersonIsCreated();
        }

        void WhenAPersonIsCreated()
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
