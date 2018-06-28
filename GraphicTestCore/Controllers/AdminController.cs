using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qBI.Areas.Identity;
using qBIPro.Models;
using qBIPro.Data;
using qBI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace qBI.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _userContext;
        private readonly AppSignInManager _signInManager;
        private readonly AppUserManager _userManager;
        // GET: /<controller>/
        public AdminController(ApplicationDbContext userContext, AppUserManager userManager, AppSignInManager signInManager)
        {
            _userContext = userContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
           
            return View(_userContext.Users.ToList());
        }

        public IActionResult Create()
        {
            return Redirect("~/Identity/Account/Register");
        }

        public async Task<IActionResult> LoginAs(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                throw new System.Exception("User not found");
            }

            //get User Data from Userid
            var user = await _userManager.FindByIdAsync(id);

            //Gets list of Roles associated with current user
            var rolesForUser = await _userManager.GetRolesAsync(user);

            using (var transaction = _userContext.Database.BeginTransaction())
            {

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var resultR = await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }

                //Delete User
                var result = await _userManager.DeleteAsync(user);
                transaction.Commit();
                if (result.Succeeded)
                {

                    TempData["Message"] = "User Deleted Successfully. ";
                    TempData["MessageValue"] = "1";

                    return RedirectToAction("Index");
                }
                else
                {
                    throw new System.Exception("Not deleted");
                }
            }


        }
        public async Task<IActionResult> Edit(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ApplicationUser user)
        {
            if (user == null)
            {
                return NotFound();
            }

            
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _userContext.Database.BeginTransaction())
                {
                    _userContext.Update(user);
                    await _userContext.SaveChangesAsync();
                    transaction.Commit();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }
    }
}
