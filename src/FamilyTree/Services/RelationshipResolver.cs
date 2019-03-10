using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;

namespace FamilyTree.Services
{
    public class RelationshipResolver : IRelationshipResolver
    {
        IEnumerable<Person> IRelationshipResolver.GetRelations(Person person, Relationship relationship)
        {
            switch (relationship)
            {
                case Relationship.Sons:
                    return person.Children.Where(child => child.IsMale);
                case Relationship.Daughters:
                    return person.Children.Where(child => child.IsFemale);
                case Relationship.Siblings:
                    return person.Mother.Children.Where(mothersChild => !mothersChild.Equals(person));
                case Relationship.BrotherInLaws:
                    return person.Mother.Children
                        .Where(mothersChild => (mothersChild.IsFemale && mothersChild.Spouse != null))
                        .Select(sister => sister.Spouse);
                default:
                    throw new NotImplementedException($"Unsupported relationship {relationship}");
                    break;
            }
        }
    }
}
