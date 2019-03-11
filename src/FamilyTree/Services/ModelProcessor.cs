using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FamilyTree.Model;

namespace FamilyTree.Services
{
    public class ModelProcessor : IModelProcessor
    {
        private readonly IPersonLookupCache _personLookupCache;
        private readonly IRelationshipResolver _relationshipResolver;

        private const string ADD_ADAM_COMMAND = "ADD_ADAM";
        private const string ADD_SPOUSE_COMMAND = "ADD_SPOUSE";
        private const string ADD_CHLID_COMMAND = "ADD_CHILD";

        private enum Command
        {
            AddAdam,
            AddSpouse,
            AddChild,
            Invalid
        }

        public ModelProcessor(IPersonLookupCache personLookupCache, IRelationshipResolver relationshipResolver, string referenceResourceName)
        {
            _personLookupCache = personLookupCache;
            _relationshipResolver = relationshipResolver;

            InitializeModelFromResource(referenceResourceName);
        }

        private void InitializeModelFromResource(string referenceResourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(referenceResourceName))
            using (var reader = new StreamReader(stream))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var words = SplitWordsInLine(line);
                    ProcessCommand(words);
                    line = reader.ReadLine();
                }
            }
        }

        private List<string> SplitWordsInLine(string line)
        {
            return Regex
                .Matches(line, @"(?<match>\w+)|\""(?<match>[\w\s]*)""")
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
                case ADD_ADAM_COMMAND:
                    command = Command.AddAdam;
                    break;
                case ADD_SPOUSE_COMMAND:
                    command = Command.AddSpouse;
                    break;
                case ADD_CHLID_COMMAND:
                    command = Command.AddChild;
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
                    case Command.AddAdam:
                        if (operands.Count == 1)
                        {
                            var name = operands[0];
                            if (!String.IsNullOrWhiteSpace(name))
                            {
                                AddAdam(name);
                            }
                        }
                        break;
                    case Command.AddSpouse:
                        if (operands.Count == 2)
                        {
                            var name = operands[0];
                            var spouse = operands[1];
                            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(spouse))
                            {
                                AddSpouse(name, spouse);
                            }
                        }
                        break;
                    case Command.AddChild:
                        if (operands.Count == 3)
                        {
                            var parentName = operands[0];
                            var childName = operands[1];
                            if (Enum.TryParse<Gender>(operands[2], out var gender))
                            {
                                AddChild(parentName, childName, gender);
                            }
                        }
                        break;
                    default:
                        ThrowInvalidCommand(command, operands);
                        break;
                }

            }
            catch (Exception e)
            {
                ThrowInvalidCommand(command, operands, e);
            }
        }

        private void ThrowInvalidCommand(Command command, List<string> operands, Exception innerException = null)
        {
            throw new Exception($"Invalid command. Type {command} with operands {string.Join(", ", operands)}", innerException);
        }

        private void AddSpouse(string name, string spouseName)
        {
            var spousesPartner = _personLookupCache.GetPerson(name);
            if (spousesPartner != null)
            {
                var genderOfSpouse = spousesPartner.IsMale ? Gender.Female : Gender.Male;
                var spouse = Person.Create(spouseName, genderOfSpouse);
                _personLookupCache.AddPerson(spouse);

                spousesPartner.SetSpouse(spouse);
            }
            else
            {
                throw new InvalidOperationException($"Person named {name} is unknown");
            }
        }

        private void AddAdam(string name)
        {
            var adam = Person.Create(name, Gender.Male);
            _personLookupCache.AddPerson(adam);
        }

        public void AddChild(string motherName, string childName, Gender gender)
        {
            var mother = _personLookupCache.GetPerson(motherName);
            if (mother != null && mother.IsFemale)
            {
                var child = Person.CreateChild(mother, childName, gender);
                _personLookupCache.AddPerson(child);
            }
            else
            {
                throw new InvalidOperationException($"Mother named {motherName} is unknown");
            }
        }

        public IEnumerable<Person> GetRelationsForPerson(string person, Relationship relationship)
        {
            throw new NotImplementedException();
        }
    }
}
