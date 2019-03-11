using System;
using System.IO;
using System.Text;
using FamilyTree.Model;
using FamilyTree.Services.InputHandling;
using FamilyTree.Services.ModelProcessing;
using Moq;
using Should;
using Xunit;

namespace FamilyTreeTests.Services.InputProcessorTests
{
    [Collection("Sequential")]
    public class InputProcessorHandlesInvalidCommand
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;
        private Exception _exception;

        public InputProcessorHandlesInvalidCommand()
        {
            _inputProcessor = GivenTheInputProcessorUsingAFileWithAnInvalidCommand();
            try
            {
                WhenTheInputIsProcessed();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        private IInputProcessor GivenTheInputProcessorUsingAFileWithAnInvalidCommand()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("ADD_GRANDPARENT Person Grandparent Female"));
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
        public void ThenAnExceptionIsThrown()
        {
            _exception.ShouldBeType<InvalidOperationException>();
        }

        [Fact]
        public void ThenNothingIsWritenToTheOutput()
        {
            _mockTextWriter.Verify(textWriter => textWriter.WriteLine(It.IsAny<string>()), Times.Never);
        }
    }
}
