using CarVillaMVC.Helpers;
using CarVillaMVC.Models;
using CarVillaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarVillaMVC.Controllers
{
    public class AccountController : Controller
    {
        public SignInManager<AppUser> _signInManager { get; }
        public UserManager<AppUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser()
            { 
                Name= registerVm.Name,
                Surname=registerVm.Surname,
                UserName=registerVm.Username,
                Email=registerVm.Email
            };

            var result = await _userManager.CreateAsync(user,registerVm.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", "Not Found");
                    return View();
                }
            }

            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (!ModelState.IsValid)
            { 
                return View();
            }

            var user = await _userManager.FindByEmailAsync(loginVm.UsernameOrEmail);
            if (user == null)
            {
                user=await _userManager.FindByNameAsync(loginVm.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View();
                }
            }

            var result= await _signInManager.CheckPasswordSignInAsync(user, loginVm.Password,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "User not found");
                return View();
            }

            await _signInManager.SignInAsync(user,true);
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach(UserRole item in Enum.GetValues(typeof(UserRole)))
            {
                if(await _roleManager.FindByNameAsync(item.ToString()) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() 
                    {
                        Name= item.ToString()
                    });

                }
            }
            return RedirectToAction("Index","Home");
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
