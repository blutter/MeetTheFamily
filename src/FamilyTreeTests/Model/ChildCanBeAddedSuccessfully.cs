using FamilyTree.Model;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Model
{
    public class ChildCanBeAddedSuccessfully
    {
        private Person _mother;
        private Person _father;
        private Person _child;

        public ChildCanBeAddedSuccessfully()
        {
            WhenAFemaleWithASpouseCreatesAChild();
        }

        void WhenAFemaleWithASpouseCreatesAChild()
        {
            _mother = Person.Create("Jane Doe", Gender.Female);
            _father = Person.Create("John Doe", Gender.Male);
            _mother.SetSpouse(_father);
            _child = Person.CreateChild(_mother, "Baby John", Gender.Male);
        }

        [Fact]
        void ThenTheMotherHasTheNewChild()
        {
            _mother.Children.Should().OnlyContain(child => child.Name == "Baby John" && child.Gender == Gender.Male);
        }

        [Fact]
        void ThenTheFatherHasTheNewChild()
        {
            _father.Children.Should().OnlyContain(child => child.Name == "Baby John" && child.Gender == Gender.Male);
        }

        [Fact]
        void ThenTheChildHasParents()
        {
            _child.Mother.Should().BeSameAs(_mother);
            _child.Father.Should().BeSameAs(_father);
        }
    }
}
