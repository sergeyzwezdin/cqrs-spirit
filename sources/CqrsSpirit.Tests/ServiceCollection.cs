using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsSpirit.Tests
{
    public class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}