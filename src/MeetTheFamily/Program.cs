using System;
using FamilyTree.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeetTheFamily
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Startup.Initialize(args[0]);

            var inputProcessor = serviceProvider.GetService<IInputProcessor>();

            inputProcessor.ProcessInput();
        }
    }
}
