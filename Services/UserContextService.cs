using System.Security.Claims;

namespace PogodynkaAPI.Services
{
    public interface IUserContextService
    {
        int GetUserId { get; }
        ClaimsPrincipal User { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor contextAccesor;

        public UserContextService(IHttpContextAccessor contextAccesor)
        {
            this.contextAccesor = contextAccesor;
        }

        public ClaimsPrincipal User => contextAccesor.HttpContext.User;
        public int GetUserId => int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
