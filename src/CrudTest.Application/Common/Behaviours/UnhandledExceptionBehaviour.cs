using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrudTest.Application.Common.Behaviours
{
    /**
     * This pipeline behaviour catches any exception thrown by inner handlers and
     * logs it's surrounding pipeline behaviours, logs it and rethrow. therefore
     * it's recommend to use this class as the outermost behaviour by
     * registering it in the container as the first pipeline
     * behaviour.
     */
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "CrudTest Request: Unhandled Exception for Request {Name} {@Request}", requestName,
                    request);

                throw;
            }
        }
    }
}