using System;
using FamilyTree.Model;
using FluentAssertions;
using Xunit;

namespace FamilyTreeTests.Model
{
    public class MotherWithoutASpouseCannotHaveAChild
    {
        private Person _mother;
        private Person _child;
        private Exception _exception;

        public MotherWithoutASpouseCannotHaveAChild()
        {
            GivenAFemaleWithoutASpouse();
            try
            {
                WhenTheFemaleHasAChild();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        private void GivenAFemaleWithoutASpouse()
        {
            _mother = Person.Create("Jane Doe", Gender.Female);
        }

        void WhenTheFemaleHasAChild()
        {
            _child = Person.CreateChild(_mother, "Baby John", Gender.Male);
        }

        [Fact]
        void ThenTheChildIsNotCreated()
        {
            _child.Should().BeNull();
        }

        [Fact]
        void ThenTheMotherHasNoChild()
        {
            _mother.Children.Should().BeEmpty();
        }

        [Fact]
        void ThenAnExceptionWasThrown()
        {
            _exception.Should().BeOfType<InvalidOperationException>();
            _exception.Message.Should().Contain("without a spouse");
        }
    }
}
