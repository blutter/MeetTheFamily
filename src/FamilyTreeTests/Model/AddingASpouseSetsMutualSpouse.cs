using FamilyTree.Model;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Model
{
    public class AddingASpouseSetsMutualSpouse
    {
        private Person _person;
        private Person _spouse;

        public AddingASpouseSetsMutualSpouse()
        {
            GivenAFemaleWithoutASpouse();
            WhenASpouseIsAdded();
        }

        private void GivenAFemaleWithoutASpouse()
        {
            _person = Person.Create("Jane Doe", Gender.Female);
        }

        void WhenASpouseIsAdded()
        {
            _spouse = Person.Create("John Doe", Gender.Male);
            _person.SetSpouse(_spouse);
        }

        [Fact]
        void ThenTheFemaleHasASpouse()
        {
            _person.Spouse.Should().Be(_spouse);
        }

        [Fact]
        void ThenTheMaleHasASpouse()
        {
            _spouse.Spouse.Should().Be(_person);
        }
    }
}
