using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;

namespace FamilyTree.Services
{
    public class RelationshipResolver : IRelationshipResolver
    {
        public IEnumerable<Person> GetRelations(Person person, Relationship relationship)
        {
            switch (relationship)
            {
                case Relationship.Sons:
                    return person.Children.Where(child => child.IsMale);
                case Relationship.Daughters:
                    return person.Children.Where(child => child.IsFemale);
                case Relationship.Siblings:
                    return person.Mother.Children.Where(sibling => !sibling.Equals(person));
                case Relationship.BrotherInLaws:
                    return GetRelations(person, Relationship.Siblings)
                        .Where(sibling => (sibling.IsFemale && sibling.Spouse != null))
                        .Select(sister => sister.Spouse)
                        .Concat((person.Spouse == null || person.Spouse.Mother == null) ?
                            new List<Person>() :
                            GetRelations(person.Spouse, Relationship.Siblings).Where(spouseSibling => spouseSibling.IsMale));
                default:
                    throw new NotImplementedException($"Unsupported relationship {relationship}");
                    break;
            }
        }
    }
}
