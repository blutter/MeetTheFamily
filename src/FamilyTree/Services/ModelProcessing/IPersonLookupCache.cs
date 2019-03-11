using FamilyTree.Model;

namespace FamilyTree.Services.ModelProcessing
{
    public interface IPersonLookupCache
    {
        void AddPerson(Person person);
        Person GetPerson(string name);
    }
}
