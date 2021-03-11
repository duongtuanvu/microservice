using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using microservice.Data;
using Microsoft.Extensions.Logging;

namespace microservice.Behaviour
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly AppDbContext _db;

        public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetType().Name;
            await using var transaction = _db.Database.BeginTransaction();
            try
            {
                _logger.LogInformation($"Start transaction Id {transaction.TransactionId} for {typeName}");
                var response = await next();
                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation($"End transaction Id {transaction.TransactionId} for {typeName}");
                return response;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError($"Transaction Id {transaction.TransactionId} for {typeName} with Error: {e.Message}");
                throw;
            }
        }
    }
}
