using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Models;

namespace UserService.Commands
{
    public class UserCreateCommand : IRequest<bool>
    {
        public string Name { get; set; }
    }

    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, bool>
    {
        public async Task<bool> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
