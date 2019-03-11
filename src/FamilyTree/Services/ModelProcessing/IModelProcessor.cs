using System.Collections.Generic;
using FamilyTree.Model;
using FamilyTree.Services.Relationships;

namespace FamilyTree.Services.ModelProcessing
{
    public interface IModelProcessor
    {
        void AddChild(string motherName, string childName, Gender gender);
        IEnumerable<Person> GetRelationsForPerson(string person, Relationship relationship);
    }
}
