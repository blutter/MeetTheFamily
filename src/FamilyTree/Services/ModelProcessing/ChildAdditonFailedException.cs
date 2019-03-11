using System;

namespace FamilyTree.Services.ModelProcessing
{
    public class ChildAdditonFailedException : Exception
    {
        public ChildAdditonFailedException(string message) : base(message)
        {
        }
    }
}
