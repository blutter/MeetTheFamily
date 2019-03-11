using System;
using FamilyTree.Model;
using FamilyTree.Services;
using Should;
using Xunit;

namespace FamilyTreeTests.Services.ModelProcessorTests
{
    public class AddChildToUnknownPersonThrowsException
    {
        private readonly ModelProcessor _modelProcessor;
        private Exception _exception;

        public AddChildToUnknownPersonThrowsException()
        {
            _modelProcessor = GivenASampleTree();
            try
            {
                WhenAChildIsAddedToAnUnknownPerson();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        private void WhenAChildIsAddedToAnUnknownPerson()
        {
            _modelProcessor.AddChild("UnknownPerson", "Kid", Gender.Female);
        }

        private ModelProcessor GivenASampleTree()
        {
            return new ModelProcessor(new PersonLookupCache(), new RelationshipResolver(),
                "FamilyTreeTests.Services.ModelProcessorTests.sample-tree.txt",
                typeof(UnknownPersonQueryThrowsException));
        }

        [Fact]
        public void ThenAnExceptionIsThrown()
        {
            _exception.ShouldBeType<PersonUnknownException>();
        }
    }
}