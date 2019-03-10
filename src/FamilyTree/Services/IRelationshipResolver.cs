using FamilyTree.Model;
using System.Collections.Generic;

namespace FamilyTree.Services
{
    public interface IRelationshipResolver
    {
        IEnumerable<Person> GetRelations(Person person, Relationship relationship);
    }
}
