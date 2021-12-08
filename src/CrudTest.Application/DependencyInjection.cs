using System.Reflection;
using CrudTest.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CrudTest.Application
{
    public static class DependencyInjection
    {
        /**
         * Adds application services to the service container.
         */
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);
            services.AddMediatR(executingAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}