using Gab.Core.Membership.Models;
using Gab.Core.Membership.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using BlogFodder.Core.Membership.Commands;

namespace Gab.Core.Membership.Handlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, GabAuthenticationResult>
    {
        private readonly UserManager<GabUser> _userManager;

        public ResetPasswordHandler(UserManager<GabUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GabAuthenticationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new GabAuthenticationResult();

            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                var resetResult = await _userManager.ResetPasswordAsync(user, request.Code, request.Password).ConfigureAwait(false);
                if (resetResult.Succeeded == false)
                {
                    result.Succeeded = false;
                    foreach (var error in resetResult.Errors)
                    {
                        result.FailedReasons.Add(error.Description);
                    }
                    return result;
                }
            }

            result.Succeeded = true;
            result.SucceededMessage = $"Your password has been reset, <a class=\"underline\" href=\"{Constants.Urls.Account.Login}\">please login</a>";
            return result;
        }
    }
}