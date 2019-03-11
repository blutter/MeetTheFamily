using System;

namespace FamilyTree.Services.ModelProcessing
{
    public class PersonUnknownException : Exception
    {
        public PersonUnknownException(string message) : base(message)
        {
        }
    }
}
