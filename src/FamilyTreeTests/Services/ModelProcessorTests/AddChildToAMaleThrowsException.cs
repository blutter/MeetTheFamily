using System;
using FamilyTree.Model;
using FamilyTree.Services;
using Should;
using Xunit;

namespace FamilyTreeTests.Services.ModelProcessorTests
{
    public class AddChildToAMaleThrowsException
    {
        private readonly ModelProcessor _modelProcessor;
        private Exception _exception;

        public AddChildToAMaleThrowsException()
        {
            _modelProcessor = GivenASampleTree();
            try
            {
                WhenAChildIsAddedToAMale();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        private void WhenAChildIsAddedToAMale()
        {
            _modelProcessor.AddChild("Patriarch", "Kid", Gender.Female);
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
            _exception.ShouldBeType<ChildAdditonFailedException>();
        }
    }
}