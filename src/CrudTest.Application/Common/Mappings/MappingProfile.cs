using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace CrudTest.Application.Common.Mappings
{
    /**
     * This mapping profile searches the executing assembly for implementations
     * of IMapFrom{T} and uses the Activator to instanciate them and call their
     * Mapping method. Care must be taken that implementations have a public
     * parameterless constructor otherwise this class' usage of Activator
     * will throw an exception. Also in case startup times were ever
     * an issue, this class must be checked.
     */
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });

            }
        }
    }
}