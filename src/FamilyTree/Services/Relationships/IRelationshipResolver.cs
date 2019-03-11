using System.Collections.Generic;
using FamilyTree.Model;

namespace FamilyTree.Services.Relationships
{
    public interface IRelationshipResolver
    {
        IEnumerable<Person> GetRelations(Person person, Relationship relationship);
    }
}
