using System.Collections.Generic;
using System.Data;
using FamilyTree.Model;

namespace FamilyTree.Services
{
    public class PersonLookupCache : IPersonLookupCache
    {
        private readonly Dictionary<string, Person> _persons = new Dictionary<string, Person>();

        public void AddPerson(Person person)
        {
            if (!_persons.ContainsKey(person.Name))
            {
                _persons.Add(person.Name, person);
            }
            else
            {
                throw new DuplicateNameException($"Person named {person.Name} already exists");
            }
        }

        public Person GetPerson(string name)
        {
            Person foundPerson = null;
            if (_persons.ContainsKey(name))
            {
                foundPerson = _persons[name];
            }

            return foundPerson;
        }
    }
}
