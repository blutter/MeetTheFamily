using FamilyTree.Model;

namespace FamilyTree.Services
{
    public interface IPersonLookupCache
    {
        void AddPerson(Person person);
        Person GetPerson(string name);
    }
}
