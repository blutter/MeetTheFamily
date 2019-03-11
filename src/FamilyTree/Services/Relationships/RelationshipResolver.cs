using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTree.Model;

namespace FamilyTree.Services.Relationships
{
    public class RelationshipResolver : IRelationshipResolver
    {
        public IEnumerable<Person> GetRelations(Person person, Relationship relationship)
        {
            switch (relationship)
            {
                case Relationship.Son:
                    return person.Children.Where(child => child.IsMale);
                case Relationship.Daughter:
                    return person.Children.Where(child => child.IsFemale);
                case Relationship.Siblings:
                    return (person.Mother != null) ? 
                        person.Mother.Children.Where(sibling => !sibling.Equals(person)) :
                        new List<Person>();
                case Relationship.BrotherInLaw:
                    return GetRelations(person, Relationship.Siblings)
                        .Where(sibling => (sibling.IsFemale && sibling.Spouse != null))
                        .Select(sister => sister.Spouse)
                        .Concat((person.Spouse?.Mother == null) ?
                            new List<Person>() :
                            GetRelations(person.Spouse, Relationship.Siblings).Where(spouseSibling => spouseSibling.IsMale));
                case Relationship.SisterInLaw:
                    return GetRelations(person, Relationship.Siblings)
                        .Where(sibling => (sibling.IsMale && sibling.Spouse != null))
                        .Select(sibling => sibling.Spouse)
                        .Concat(person.Spouse?.Mother == null ?
                            new List<Person>() : 
                            GetRelations(person.Spouse, Relationship.Siblings).Where(spouseSibling => spouseSibling.IsFemale));
                case Relationship.MaternalAunt:
                    return GetRelations(person.Mother, Relationship.Siblings)
                        .Where(sibling => sibling.IsFemale);
                case Relationship.PaternalAunt:
                    return GetRelations(person.Father, Relationship.Siblings)
                        .Where(sibling => sibling.IsFemale);
                case Relationship.MaternalUncle:
                    return GetRelations(person.Mother, Relationship.Siblings)
                        .Where(sibling => sibling.IsMale);
                case Relationship.PaternalUncle:
                    return GetRelations(person.Father, Relationship.Siblings)
                        .Where(sibling => sibling.IsMale);
                default:
                    throw new NotImplementedException($"Unsupported relationship {relationship}");
                    break;
            }
        }
    }
}
