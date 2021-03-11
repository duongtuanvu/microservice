using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentValidation;
using UserService.Commands;
using UserService.Models;

namespace UserService.Validators
{
    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
