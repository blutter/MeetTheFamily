using System;
using System.IO;
using System.Text;
using FamilyTree.Model;
using FamilyTree.Services;
using Moq;
using Xunit;

namespace FamilyTreeTests.Infrastructure.InputProcessorTests
{
    public class InputProcessorHandlesGetChildCommand
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;

        public InputProcessorHandlesGetChildCommand()
        {
            _inputProcessor = GivenTheInputProcessorUsingAFileWithAGetChildCommand();
            WhenTheInputIsProcessed();
        }

        private IInputProcessor GivenTheInputProcessorUsingAFileWithAGetChildCommand()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("ADD_CHILD Mom Kid Female"));
            Func<string, StreamReader> mockStreamReaderFactory = (string filename) => new StreamReader(stream);

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
        public void ThenAChildIsAdded()
        {
            _mockModelProcessor.Verify(modelProcessor => modelProcessor.AddChild("Mom", "Kid", Gender.Female),
                Times.Once);
        }

        [Fact]
        public void ThenChildAddedAppearsInTheOutput()
        {
            _mockTextWriter.Verify(textWriter => textWriter.WriteLine(It.Is<string>(str => str == "CHILD_ADDED")),
                Times.Once);
        }
    }
}
