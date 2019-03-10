using System.Collections.Generic;

namespace FamilyTree.Model
{
    public class Person
    {
        private readonly string _name;
        private readonly Gender _gender;

        public Person Mother { get; protected set; }
        public Person Father { get; protected set; }

        public Person Spouse { get; set; }
        public List<Person> Children { get; set; }

        public Person(string name, Gender gender)
        {
            _name = name;
            _gender = gender;
        }

        public string Name => _name;
        public Gender Gender => _gender;

        bool IsMale() => Gender == Gender.Male;

        void AddChild(Person child)
        {
            child.Mother = this;
        }

        void SetPartner(Person partner)
        {

        }
    }
}
