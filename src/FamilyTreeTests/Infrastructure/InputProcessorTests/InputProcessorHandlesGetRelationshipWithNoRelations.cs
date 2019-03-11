using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FamilyTree.Model;
using FamilyTree.Services;
using FamilyTree.Services.InputHandling;
using FamilyTree.Services.ModelProcessing;
using FamilyTree.Services.Relationships;
using Moq;
using Xunit;

namespace FamilyTreeTests.Infrastructure.InputProcessorTests
{
    [Collection("Sequential")]
    public class InputProcessorHandlesGetRelationshipWithNoRelations
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;

        public InputProcessorHandlesGetRelationshipWithNoRelations()
        {
            _inputProcessor = GivenTheInputProcessorUsingAFileWithAGetRelationshipCommand();
            WhenTheInputIsProcessed();
        }

        private IInputProcessor GivenTheInputProcessorUsingAFileWithAGetRelationshipCommand()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("GET_RELATIONSHIP Kid Siblings"));
            Func<string, StreamReader> mockStreamReaderFactory = (string filename) => new StreamReader(stream);

            _mockModelProcessor
                .Setup(modelProcessor => modelProcessor.GetRelationsForPerson(It.Is<string>(str => str == "Kid"),
                    It.Is<Relationship>(relationship => relationship == Relationship.Siblings)))
                .Returns(new List<Person>());

            SetupTextWriterToCaptureConsoleOutput();

            return new InputProcessor(_mockModelProcessor.Object, "input-file.txt", mockStreamReaderFactory);
        }

        private void SetupTextWriterToCaptureConsoleOutput()
        {
            _mockTextWriter = new Mock<TextWriter>();
            Console.SetOut(_mockTextWriter.Object);
        }

        private void WhenTheInputIsProcessed()
        {
            _inputProcessor.ProcessInput();
        }

        [Fact]
        public void ThenTheRelationshipIsQueried()
        {
            _mockModelProcessor.Verify(modelProcessor => modelProcessor.GetRelationsForPerson("Kid", Relationship.Siblings),
                Times.Once);
        }

        [Fact]
        public void ThenTheRelationsAppearInTheOutput()
        {
            _mockTextWriter.Verify(textWriter => textWriter.WriteLine(It.Is<string>(str => str == "NONE")),
                Times.Once);
        }
    }
}
