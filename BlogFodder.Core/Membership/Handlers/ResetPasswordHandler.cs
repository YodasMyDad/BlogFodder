using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogFodder.Core.Membership.Handlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, AuthenticationResult>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthenticationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthenticationResult();

            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                var resetResult = await _userManager.ResetPasswordAsync(user, request.Code, request.Password).ConfigureAwait(false);
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