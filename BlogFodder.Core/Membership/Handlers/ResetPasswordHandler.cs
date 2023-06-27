﻿using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Membership.Handlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, AuthenticationResult>
    {
        private readonly IServiceProvider _serviceProvider;

        public ResetPasswordHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<AuthenticationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthenticationResult();
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var resetResult = await userManager.ResetPasswordAsync(user, request.Code, request.Password);
                if (resetResult.Succeeded == false)
                {
                    result.Success = false;
                    foreach (var error in resetResult.Errors)
                    {
                        result.AddMessage(error.Description, ResultMessageType.Error);
                    }
                    return result;
                }
            }

            result.Success = true;
            result.AddMessage($"Your password has been reset, <a class=\"underline\" href=\"{Constants.Urls.Account.Login}\">please login</a>", ResultMessageType.Success);
            return result;
        }
    }
}