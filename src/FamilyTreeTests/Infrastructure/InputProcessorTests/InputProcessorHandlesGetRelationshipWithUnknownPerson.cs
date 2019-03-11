using System;
using System.IO;
using System.Text;
using FamilyTree.Services;
using Moq;
using Xunit;

namespace FamilyTreeTests.Infrastructure.InputProcessorTests
{
    [Collection("Sequential")]
    public class InputProcessorHandlesGetRelationshipWithUnknownPerson
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;

        public InputProcessorHandlesGetRelationshipWithUnknownPerson()
        {
            _inputProcessor = GivenTheInputProcessorQueryingWithAnUnknownPerson();
            WhenTheInputIsProcessed();
        }

        private IInputProcessor GivenTheInputProcessorQueryingWithAnUnknownPerson()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("GET_RELATIONSHIP Kid Siblings"));
            Func<string, StreamReader> mockStreamReaderFactory = (string filename) => new StreamReader(stream);

            _mockModelProcessor
                .Setup(modelProcessor => modelProcessor.GetRelationsForPerson(It.IsAny<string>(),
                    It.IsAny<Relationship>()))
                .Throws(new PersonUnknownException("oops"));

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
        public void ThenPersonNotFoundAppearsInTheOutput()
        {
            _mockTextWriter.Verify(textWriter => textWriter.WriteLine(It.Is<string>(str => str == "PERSON_NOT_FOUND")),
                Times.Once);
        }
    }
}
