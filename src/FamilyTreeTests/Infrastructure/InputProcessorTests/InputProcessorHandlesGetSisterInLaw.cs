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
    public class InputProcessorHandlesGetSisterInLaw
    {
        private readonly IInputProcessor _inputProcessor;
        private Mock<TextWriter> _mockTextWriter;
        private Mock<IModelProcessor> _mockModelProcessor;

        public InputProcessorHandlesGetSisterInLaw()
        {
            _inputProcessor = GivenTheInputProcessorUsingAFileWithAGetSisterInLawCommand();
            WhenTheInputIsProcessed();
        }

        private IInputProcessor GivenTheInputProcessorUsingAFileWithAGetSisterInLawCommand()
        {
            _mockModelProcessor = new Mock<IModelProcessor>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("GET_RELATIONSHIP Person Sister-In-Law"));
            Func<string, StreamReader> mockStreamReaderFactory = (string filename) => new StreamReader(stream);

            _mockModelProcessor
                .Setup(modelProcessor => modelProcessor.GetRelationsForPerson(It.Is<string>(str => str == "Person"),
                    It.Is<Relationship>(relationship => relationship == Relationship.SisterInLaw)))
                .Returns(new List<Person> { Person.Create("SisterInLaw1", Gender.Female), Person.Create("SisterInLaw2", Gender.Female) });

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
            _mockModelProcessor.Verify(modelProcessor => modelProcessor.GetRelationsForPerson("Person", Relationship.SisterInLaw),
                Times.Once);
        }

        [Fact]
        public void ThenChildAddedAppearsInTheOutput()
        {
            _mockTextWriter.Verify(textWriter => textWriter.WriteLine(It.Is<string>(str => str == "SisterInLaw1 SisterInLaw2")),
                Times.Once);
        }
    }
}
