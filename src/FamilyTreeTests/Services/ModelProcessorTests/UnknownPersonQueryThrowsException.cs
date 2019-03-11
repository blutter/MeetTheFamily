using System;
using FamilyTree.Services;
using FamilyTree.Services.ModelProcessing;
using FamilyTree.Services.Relationships;
using Should;
using Xunit;

namespace FamilyTreeTests.Services.ModelProcessorTests
{
    public class UnknownPersonQueryThrowsException
    {
        private readonly ModelProcessor _modelProcessor;
        private Exception _exception;

        public UnknownPersonQueryThrowsException()
        {
            _modelProcessor = GivenASampleTree();
            try
            {
                WhenARelationshipForAnUnknownPersonIsQueried();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        private void WhenARelationshipForAnUnknownPersonIsQueried()
        {
            _modelProcessor.GetRelationsForPerson("Incognito", Relationship.Daughter);
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