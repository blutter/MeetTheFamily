using System.Collections.Generic;
using FamilyTree.Model;

namespace FamilyTree.Services
{
    public interface IModelProcessor
    {
        void AddChild(string motherName, string childName, Gender gender);
        IEnumerable<Person> GetRelationsForPerson(string person, Relationship relationship);
    }
}
