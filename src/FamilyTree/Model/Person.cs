using System;
using System.Collections.Generic;

namespace FamilyTree.Model
{
    public class Person
    {
        private readonly string _name;
        private readonly Gender _gender;

        public Person Mother { get; private set; }
        public Person Father { get; private set; }

        public Person Spouse { get; private set; }
        public List<Person> Children { get; private set; } = new List<Person>();

        public static Person Create(string name, Gender gender)
        {
            return new Person(name, gender);
        }

        public static Person CreateChild(Person mother, string name, Gender gender)
        {
            if (mother.IsFemale && mother.HasSpouse)
            {
                var child = new Person(name, gender, mother, mother.Spouse);
                mother.AddChild(child);

                var father = mother.Spouse;
                father.AddChild(child);
                return child;
            }
            else
            {
                var message = !mother.HasSpouse ?
                    $"Cannot create child for person {mother.Name} without a spouse" :
                    $"Cannot create child for male person {mother.Name}";
                throw new InvalidOperationException(message);
            }
        }

        private Person(string name, Gender gender)
        {
            _name = name;
            _gender = gender;
        }

        private Person(string name, Gender gender, Person mother, Person father) : this(name, gender)
        {
            Mother = mother;
            Father = father;
        }

        public string Name => _name;
        public Gender Gender => _gender;

        public bool IsMale => Gender == Gender.Male;
        public bool IsFemale => !IsMale;

        bool HasSpouse => Spouse != null;

        private void AddChild(Person child)
        {
            Children.Add(child);
        }

        public void SetSpouse(Person spouse)
        {
            Spouse = spouse;
            if (!spouse.HasSpouse)
            {
                spouse.SetSpouse(this);
            }
        }

        public bool Equals(Person person) => person.Name == Name;
    }
}
