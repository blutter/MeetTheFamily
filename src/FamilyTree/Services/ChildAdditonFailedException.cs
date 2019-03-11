using System;

namespace FamilyTree.Services
{
    public class ChildAdditonFailedException : Exception
    {
        public ChildAdditonFailedException(string message) : base(message)
        {
        }
    }
}
