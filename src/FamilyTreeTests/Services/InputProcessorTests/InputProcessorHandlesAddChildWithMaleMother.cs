using System;
using System.IO;
using System.Text;
using FamilyTree.Model;
using FamilyTree.Services.InputHandling;
using FamilyTree.Services.ModelProcessing;
using Moq;
using Xunit;

namespace FamilyTreeTests.Services.InputProcessorTests
{
    [Collection("Sequential")]
    public class InputProcessorHandlesAddChildWithMaleMother
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;

        public InputProcessorHandlesAddChildWithMaleMother()
        {
            _inputProcessor = GivenTheInputProcessorQueryingWithAnUnknownPerson();
            WhenTheInputIsProcessed();
        }

        private IInputProcessor GivenTheInputProcessorQueryingWithAnUnknownPerson()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("ADD_CHILD Dad Kiddo Female"));
            Func<string, StreamReader> mockStreamReaderFactory = (string filename) => new StreamReader(stream);

            _mockModelProcessor
                .Setup(modelProcessor => modelProcessor.AddChild(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Gender>()))
                .Throws(new ChildAdditonFailedException("oh oh"));

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
            _mockModelProcessor.Verify(modelProcessor => modelProcessor.AddChild("Dad", "Kiddo", Gender.Female),
                Times.Once);
        }

        [Fact]
        public void ThenPersonNotFoundAppearsInTheOutput()
        {
            _mockTextWriter.Verify(textWriter => textWriter.WriteLine(It.Is<string>(str => str == "CHILD_ADDITION_FAILED")),
                Times.Once);
        }
    }
}
