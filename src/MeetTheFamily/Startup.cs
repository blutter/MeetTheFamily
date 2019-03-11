using System;
using FamilyTree.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeetTheFamily
{
    class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceProvider Initialize()
        {
            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            return ServiceProvider;
        }

        private static void Configure(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IModelProcessor, ModelProcessor>();
            serviceCollection.AddSingleton<IRelationshipResolver, RelationshipResolver>();
            serviceCollection.AddSingleton<IPersonLookupCache, PersonLookupCache>();
        }
    }
}
