using BlogFodder.Core.Shared.Models;

namespace BlogFodder.Core.Membership.Models
{
    /// <summary>
    /// The result of either an attempted login or registration
    /// </summary>
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public List<ResultMessage> Messages { get; set; } = new();
        public string? NavigateToUrl { get; set; }
    }
}