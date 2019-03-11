using FamilyTree.Services;
using FamilyTree.Services.InputHandling;
using Microsoft.Extensions.DependencyInjection;

namespace MeetTheFamily
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                var serviceProvider = Startup.Initialize(args[0]);

                var inputProcessor = serviceProvider.GetService<IInputProcessor>();

                inputProcessor.ProcessInput();
            }
        }
    }
}
