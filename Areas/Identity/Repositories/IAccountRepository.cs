using App.Areas.Identity.Models.AccountViewModels;

namespace ProjectFinalEngineer.Areas.Identity.Repositories;
public interface IAccountRepository
{
    Task<string> RegisterAsync(RegisterViewModel model, string returnUrl = null);
}