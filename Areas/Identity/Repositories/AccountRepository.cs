// using System.Text;
// using System.Text.Encodings.Web;
// using App.Areas.Identity.Controllers;
// using App.Areas.Identity.Models.AccountViewModels;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.AspNetCore.WebUtilities;
// using ProjectFinalEngineer.Models;

// namespace ProjectFinalEngineer.Areas.Identity.Repositories;
// public class AccountRepository : IAccountRepository
// {
//     private readonly UserManager<AppUser> _userManager;
//     private readonly SignInManager<AppUser> _signInManager;
//     private readonly IEmailSender _emailSender;
//     private readonly ILogger<AccountController> _logger;
//     public AccountRepository(
//           UserManager<AppUser> userManager,
//           SignInManager<AppUser> signInManager,
//           IEmailSender emailSender,
//           ILogger<AccountController> logger)
//     {
//         _userManager = userManager;
//         _signInManager = signInManager;
//         _emailSender = emailSender;
//         _logger = logger;
//     }
//     public async Task<string> RegisterAsync(RegisterViewModel model, string returnUrl = null)
//     {
//         var user = new AppUser { UserName = model.UserName, Email = model.Email };
//         var result = await _userManager.CreateAsync(user, model.Password);

//         if (result.Succeeded)
//         {
//             _logger.LogInformation("Đã tạo user mới.");

//             // Phát sinh token để xác nhận email
//             var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//             code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


//             await _emailSender.SendEmailAsync(model.Email,
//                 "Xác nhận địa chỉ email",
//                 @$"Bạn đã đăng ký tài khoản trên RazorWeb, 
//                            hãy <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bấm vào đây</a> 
//                            để kích hoạt tài khoản.");

//             if (_userManager.Options.SignIn.RequireConfirmedAccount)
//             {
//                 return LocalRedirect(Url.Action(nameof(RegisterConfirmation)));
//             }
//             else
//             {
//                 await _signInManager.SignInAsync(user, isPersistent: false);
//                 return LocalRedirect(returnUrl);
//             }
//         }
//     }