using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RecipeApi.Authorization;

public class IsRecipeOwnerHandler :
    AuthorizationHandler<IsRecipeOwnerRequirement, Recipe?>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsRecipeOwnerRequirement requirement,
        Recipe? resource)
    {
        if (resource is null
        || resource.CreatedById == context.User.Identity?.Name)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}