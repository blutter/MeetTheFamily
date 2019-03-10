using FamilyTree.Model;
using FluentAssertions;
using System;
using Xunit;

namespace FamilyTreeTests.Model
{
    public class ChildCannotBeAddedToAFather
    {
        private Person _father;
        private Person _child;
        private Exception _exception;

        public ChildCannotBeAddedToAFather()
        {
            try
            {
                WhenAMaleWithASpouseCreatesAChild();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        void WhenAMaleWithASpouseCreatesAChild()
        {
            _father = Person.Create("John Doe", Gender.Male);
            var mother = Person.Create("Jane Doe", Gender.Female);
            _father.SetSpouse(mother);
            _child = Person.CreateChild(_father, "Baby John", Gender.Male);
        }

        [Fact]
        void ThenTheChildIsNotCreated()
        {
            _child.Should().BeNull();
        }

        [Fact]
        void ThenTheFatherHasNoChild()
        {
            _father.Children.Should().BeEmpty();
        }

        [Fact]
        void ThenAnExceptionIsThrown()
        {
            _exception.Should().BeOfType<InvalidOperationException>();
            _exception.Message.Should().Contain("Cannot create child for male");
        }
    }
}
