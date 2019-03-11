using System;

namespace FamilyTree.Services
{
    public class PersonUnknownException : Exception
    {
        public PersonUnknownException(string message) : base(message)
        {
        }
    }
}
