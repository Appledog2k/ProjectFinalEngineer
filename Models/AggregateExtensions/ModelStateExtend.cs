using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateExtensions
{
    public static class ModelStateExtend
    {
        public static void AddModelError(this ModelStateDictionary modelState, string mgs)
        {
            modelState.AddModelError(string.Empty, mgs);
        }
        public static void AddModelError(this ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.Description);
            }
        }
    }
}