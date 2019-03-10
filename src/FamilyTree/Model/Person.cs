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
            if (mother.IsFemale())
            {
                var child = new Person(name, gender, mother, mother.Spouse);
                mother.AddChild(child);

                var father = mother.Spouse;
                father.AddChild(child);
                return child;
            }
            else
            {
                throw new InvalidOperationException($"Cannot create child for male person {mother.Name}");
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

        bool IsMale() => Gender == Gender.Male;
        bool IsFemale() => !IsMale();

        private void AddChild(Person child)
        {
            Children.Add(child);
        }

        public void SetSpouse(Person spouse)
        {
            Spouse = spouse;
        }
    }
}
