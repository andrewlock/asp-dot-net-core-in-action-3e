using Microsoft.AspNetCore.Authorization;

namespace RecipeApi.Authorization;

public class IsRecipeOwnerRequirement : IAuthorizationRequirement { }