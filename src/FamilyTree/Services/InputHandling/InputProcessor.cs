using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FamilyTree.Model;
using FamilyTree.Services.ModelProcessing;
using FamilyTree.Services.Relationships;

namespace FamilyTree.Services.InputHandling
{
    public class InputProcessor : IInputProcessor
    {
        private readonly IModelProcessor _modelProcessor;
        private readonly string _fileName;
        private readonly Func<string, StreamReader> _streamReaderFactory;

        private const string ADD_CHLID_COMMAND = "ADD_CHILD";
        private const string GET_RELATIONSHIP_COMMAND = "GET_RELATIONSHIP";

        private enum Command
        {
            AddChild,
            GetRelationship,
            Invalid
        }

        public InputProcessor(IModelProcessor modelProcessor, string fileName, Func<string, StreamReader> streamReaderFactory)
        {
            _modelProcessor = modelProcessor;
            _fileName = fileName;
            _streamReaderFactory = streamReaderFactory;
        }

        public void ProcessInput()
        {
            using (var streamReader = _streamReaderFactory(_fileName))
            {
                var line = streamReader.ReadLine();
                while (line != null)
                {
                    var words = SplitWordsInLine(line);
                    ProcessCommand(words);
                    line = streamReader.ReadLine();
                }
            }
        }

        private List<string> SplitWordsInLine(string line)
        {
            return Regex
                .Matches(line, @"(?<match>[\w-]+)|\""(?<match>[\w\s-]*)""")
                .Select(m => m.Groups["match"].Value)
                .ToList();
        }

        private void ProcessCommand(List<string> commandWords)
        {
            var commandWord = commandWords[0];
            var commandOperands = commandWords.Skip(1).ToList();
            var parsedCommand = ParseCommand(commandWord);
            ProcessCommand(parsedCommand, commandOperands);
        }

        private Command ParseCommand(string commandWord)
        {
            var command = Command.Invalid;
            switch (commandWord)
            {
                case ADD_CHLID_COMMAND:
                    command = Command.AddChild;
                    break;
                case GET_RELATIONSHIP_COMMAND:
                    command = Command.GetRelationship;
                    break;
                default:
                    break;
            }

            return command;
        }

        private void ProcessCommand(Command command, List<string> operands)
        {
            try
            {
                switch (command)
                {
                    case Command.AddChild:
                        HandleAddChildOperation(operands);
                        break;
                    case Command.GetRelationship:
                        HandleGetRelationshipOperation(operands);
                        break;
                    default:
                        ThrowInvalidCommand(command, operands);
                        break;
                }

            }
            catch (ChildAdditonFailedException)
            {
                Console.WriteLine("CHILD_ADDITION_FAILED");
            }
            catch (PersonUnknownException)
            {
                Console.WriteLine("PERSON_NOT_FOUND");
            }
            catch (Exception e)
            {
                ThrowInvalidCommand(command, operands, e);
            }
        }

        private void HandleAddChildOperation(List<string> operands)
        {
            if (operands.Count == 3)
            {
                var motherName = operands[0];
                var childName = operands[1];
                if (Enum.TryParse<Gender>(operands[2], out var gender))
                {
                    _modelProcessor.AddChild(motherName, childName, gender);
                }

                Console.WriteLine("CHILD_ADDED");
            }
        }

        private void HandleGetRelationshipOperation(List<string> operands)
        {
            if (operands.Count == 2)
            {
                var name = operands[0];
                var relationshipStr = operands[1];
                if (!String.IsNullOrWhiteSpace(relationshipStr) &&
                    Enum.TryParse<Relationship>(relationshipStr.Replace("-", ""), out var relationship))
                {
                    var relations = _modelProcessor.GetRelationsForPerson(name, relationship).ToList();

                    Console.WriteLine(relations.Count > 0
                        ? String.Join(" ", relations.Select(person => person.Name))
                        : "NONE");
                }
            }
        }

        private void ThrowInvalidCommand(Command command, List<string> operands, Exception innerException = null)
        {
            throw new Exception($"Invalid command. Type {command} with operands {string.Join(", ", operands)}", innerException);
        }
    }
}
