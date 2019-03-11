using System;
using System.IO;
using System.Text;
using FamilyTree.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeetTheFamily
{
    class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceProvider Initialize(string inputFile)
        {
            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);
            ConfigureInputFromFile(serviceCollection, inputFile);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            return ServiceProvider;
        }

        private static void Configure(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IModelProcessor, ModelProcessor>();
            serviceCollection.AddSingleton<IRelationshipResolver, RelationshipResolver>();
            serviceCollection.AddSingleton<IPersonLookupCache, PersonLookupCache>();
            serviceCollection.AddTransient<Func<string, StreamReader>>(provider => filename => new StreamReader(filename));
        }

        private static void ConfigureInputFromFile(ServiceCollection serviceCollection, string inputFile)
        {
            serviceCollection.AddSingleton<IInputProcessor, InputProcessor>(provider =>
                new InputProcessor(provider.GetService<IModelProcessor>(), inputFile,
                    provider.GetService<Func<string, StreamReader>>()));
        }
    }
}
