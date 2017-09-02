namespace CalendarBackend.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;

    public class TeamMemberHandler : AuthorizationHandler<TeamMemberRequirement>
    {
        public TeamMemberHandler(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.TeamMemberRole = configuration.GetValue<string>("TeamMemberRole");
        }

        public string TeamMemberRole { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamMemberRequirement requirement)
        {
            if (!string.IsNullOrWhiteSpace(this.TeamMemberRole) && context.User.IsInRole(this.TeamMemberRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
