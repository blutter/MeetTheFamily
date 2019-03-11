using System;
using System.IO;
using System.Text;
using FamilyTree.Model;
using FamilyTree.Services;
using Moq;
using Xunit;

namespace FamilyTreeTests.Infrastructure.InputProcessorTests
{
    public class InputProcessorHandlesAddChildWithUnknownMother
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;

        public InputProcessorHandlesAddChildWithUnknownMother()
        {
            _inputProcessor = GivenTheInputProcessorQueryingWithAnUnknownPerson();
            WhenTheInputIsProcessed();
        }

        private IInputProcessor GivenTheInputProcessorQueryingWithAnUnknownPerson()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("ADD_CHILD UnknownMum Kiddo Female"));
            Func<string, StreamReader> mockStreamReaderFactory = (string filename) => new StreamReader(stream);

            _mockModelProcessor
                .Setup(modelProcessor => modelProcessor.AddChild(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Gender>()))
                .Throws(new PersonUnknownException("can't find person"));

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
        public void ThenTheChildIsAdded()
        {
            _mockModelProcessor.Verify(modelProcessor => modelProcessor.AddChild("UnknownMum", "Kiddo", Gender.Female),
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
