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
                    return GetSons(person);
                case Relationship.Daughter:
                    return GetDaughters(person);
                case Relationship.Siblings:
                    return GetSiblings(person);
                case Relationship.BrotherInLaw:
                    return GetBrothersInLaw(person);
                case Relationship.SisterInLaw:
                    return GetSistersInLaw(person);
                case Relationship.MaternalAunt:
                    return GetMaternalAunts(person);
                case Relationship.PaternalAunt:
                    return GetPaternalAunts(person);
                case Relationship.MaternalUncle:
                    return GetMaternalUncles(person);
                case Relationship.PaternalUncle:
                    return GetPaternalUncles(person);
                default:
                    throw new NotImplementedException($"Unsupported relationship {relationship}");
            }
        }

        private static IEnumerable<Person> GetSons(Person person)
        {
            return person.Children.Where(child => child.IsMale);
        }

        private static IEnumerable<Person> GetDaughters(Person person)
        {
            return person.Children.Where(child => child.IsFemale);
        }

        private static IEnumerable<Person> GetSiblings(Person person)
        {
            return (person.Mother != null)
                ? person.Mother.Children.Where(sibling => !sibling.Equals(person))
                : new List<Person>();
        }

        private IEnumerable<Person> GetBrothersInLaw(Person person)
        {
            Func<Person, bool> sisterHasSpouse()
            {
                return sibling => (sibling.IsFemale && sibling.HasSpouse);
            }

            return GetSiblings(person)
                .Where(sisterHasSpouse())
                .Select(sister => sister.Spouse)
                .Concat((person.Spouse?.Mother == null)
                    ? new List<Person>()
                    : GetSiblings(person.Spouse).Where(spouseSibling => spouseSibling.IsMale));
        }

        private IEnumerable<Person> GetSistersInLaw(Person person)
        {
            Func<Person, bool> brotherHasSpouse()
            {
                return sibling => (sibling.IsMale && sibling.HasSpouse);
            }

            return GetSiblings(person)
                .Where(brotherHasSpouse())
                .Select(sibling => sibling.Spouse)
                .Concat(person.Spouse?.Mother == null
                    ? new List<Person>()
                    : GetSiblings(person.Spouse).Where(spouseSibling => spouseSibling.IsFemale));
        }

        private IEnumerable<Person> GetPaternalUncles(Person person)
        {
            return GetSiblings(person.Father)
                .Where(sibling => sibling.IsMale);
        }

        private IEnumerable<Person> GetMaternalAunts(Person person)
        {
            return GetSiblings(person.Mother)
                .Where(sibling => sibling.IsFemale);
        }

        
        private IEnumerable<Person> GetPaternalAunts(Person person)
        {
            return GetSiblings(person.Father)
                .Where(sibling => sibling.IsFemale);
        }

        private IEnumerable<Person> GetMaternalUncles(Person person)
        {
            return GetSiblings(person.Mother)
                .Where(sibling => sibling.IsMale);
        }
    }
}
