using Licensing.Manager.Data;
using Licensing.Manager.Extension;
using Licensing.Manager.General;
using Licensing.Manager.Handlers.QueryHandlers.Admin;
using Licensing.Manager.Model;
using Licensing.Manager.Models;
using Licensing.Manager.ViewModels;
using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Licensing.Manager.Controllers
{
    [Authorize]
    
    public class AdminController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private static IHttpContextAccessor _hcontext;
        private readonly IMediator _mediator;

        public AdminController( IMediator mediator, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor haccess)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _hcontext = haccess;
            _mediator = mediator;

        }
        private UserStore<ApplicationUser> GetUserStore() => new UserStore<ApplicationUser>(_context);
       
        [CanAccessFilter]
        public async Task <IActionResult> Index()
        {
            return View();
        }

        public async Task<JsonResult> UserList()
        {
            var ListUser = await _mediator.Send(new GetAllAsoNetUsersQuery());
            return Json(ListUser);
        }

        [HttpPost]
        public async Task<JsonResult> CreateUser(CreateUserViewModel modal)
        {
            try
            {

                if (!string.IsNullOrEmpty(modal.UserId))
                {
                    var user = await _userManager.FindByIdAsync(modal.UserId);
                    user.FullName = modal.FullName;
                    user.Phone = modal.Phone;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true, message = "User Updated!" });
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = modal.Email,
                            Email = modal.Email,
                            FullName = modal.FullName,
                            Phone = modal.Phone,
                            IsActive = true

                        };
                        var result = await _userManager.CreateAsync(user, modal.Password);
                        if (result.Succeeded)
                        {
                            var roleName = "Guest";
                            var roleExist = await _roleManager.RoleExistsAsync(roleName);
                            if (!roleExist)
                            {
                                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                            }
                            var assignResult = await _userManager.AddToRoleAsync(user, roleName);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var confirmResult = await _userManager.ConfirmEmailAsync(user, code);

                            if (confirmResult.Succeeded)
                            {
                                await _emailSender.SendEmailAsync(modal.Email, "Account Created",
                                $"Your Account Has Been Created...");
                                return Json(new { isSuccess = true, message = "User Created!" });
                            }
                            else
                            {
                                return Json(new { isSuccess = true, message = "Please confirm your account!" });
                            }
                        }
                    }
                }
                return Json(new { isSuccess = false });
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }
        [HttpPost]
        public async Task<JsonResult> ActiveUser(ActiveUser modal)
        {
            try
            {
                if (!string.IsNullOrEmpty(modal.UserId))
                {
                    var user = await _userManager.FindByIdAsync(modal.UserId);
                    user.IsActive = modal.IsActive;
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true, message = "User Updated!" });
                    }
                }


                return Json(new { isSuccess = true, message = "User Updated!" });

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [HttpGet]
        public async Task<JsonResult> DeleteUser(string userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdentity = await _userManager.FindByIdAsync(userId);
                    var result = await _userManager.DeleteAsync(userIdentity);
                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true, message = "User Deleted!" });
                    }
                    else
                    {
                        return Json(new { isSuccess = false });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }

        [CanAccessFilter]
        public async Task<IActionResult> CreateRole()
        {
           
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> RoleList()
        {
            var ListRole = await _mediator.Send(new GetAllAspNetRoleQuery());
            return Json(ListRole);
        }

        [HttpPost]
        public async Task<JsonResult> CreateRole(string roleId, string roleName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(roleId))
                    {
                        var role = await _roleManager.FindByIdAsync(roleId);
                        role.Name = roleName;
                        var result = await _roleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                            return Json(new { isSuccess = true, message = "Role Updated!" });
                        }
                    }
                    else
                    {
                        var roleExist = await _roleManager.RoleExistsAsync(roleName);
                        if (!roleExist)
                        {
                            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                            if (result.Succeeded)
                            {
                                return Json(new { isSuccess = true, message = "Role Created!" });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = true, message = "Role Already Exist!" });
                        }
                    }
                    return Json(new { isSuccess = true, message = "Error while Creating Role!" });
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }

        [HttpGet]
        public async Task<JsonResult> DeleteRole(string roleId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true, message = "Role Deleted!" });
                    }
                    else
                    {
                        return Json(new { isSuccess = false });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }

        [CanAccessFilter]
        public IActionResult RoleAccess()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            var ListRole = await _mediator.Send(new GetAllAspNetRoleQuery());
            return Json(ListRole);
        }
        
        [HttpGet]
        public async Task<string> GetMenuItem(string roleId)
        {
            var model = new RoleIdViewModel { RoleId = roleId };
            List<MenuItemModel> result = await _mediator.Send(new GetMenuItemQuery(model));

            IList<JSTree> MenuList = result.Where(r => r.ParentManuName == 0).Select(r => new JSTree
            {
                text = r.MenuName,
                parent = "#",
                children = result.Where(c => c.ParentManuName == r.Id).Select(c => new JSTree
                {
                    text = c.MenuName,
                    parent = r.Id.ToString(),
                    state = new State()
                    {
                        selected = c.HasAccess
                    },
                    li_attr = new JSTreeAttr
                    {
                        id = c.Id.ToString(),
                        selected = false
                    }

                }).ToArray(),
                state = new State
                {
                    selected = r.HasAccess
                },
                li_attr = new JSTreeAttr
                {
                    id = r.Id.ToString(),
                    selected = false
                }
            }).AsQueryable().ToList();

            var menujson = JsonConvert.SerializeObject(MenuList.ToArray(), Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });

            return JsonConvert.SerializeObject(menujson, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });

        }


        [HttpPost]
        public async Task<JsonResult> CreateRoleAccess(RoleAccessModel modal)
        {
            var model = new RoleAccessModel { RoleId = modal.RoleId, RolesItem=modal.RolesItem };
            var result = await _mediator.Send(new CreateRoleAccessQuery(model));
            return Json(result);
        }

        [CanAccessFilter]
        public IActionResult AssignRole()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var ListUser = await _mediator.Send(new GetAllAsoNetUsersQuery());
            return Json(ListUser);
        }

        [HttpPost]
        public async Task<JsonResult> AssignRole(string userId, string roleName, List<string> roleList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdentity = await _userManager.FindByIdAsync(userId);
                    foreach (var item in roleList)
                    {
                        var isrole = await _userManager.IsInRoleAsync(userIdentity, item);
                        if (isrole)
                        {
                            var remove = await _userManager.RemoveFromRoleAsync(userIdentity, item);
                        }
                    }

                    var result = await _userManager.AddToRoleAsync(userIdentity, roleName);
                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true, message = "Role Assigned!" });
                    }
                    else
                    {
                        return Json(new { isSuccess = true, message = "Error while Assigning Role!" });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }

        [CanAccessFilter]
        public async Task<IActionResult> ChangeAvtar()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return View("ChangeAvtar", user);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeAvatar(string UserId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdentity = await _userManager.FindByIdAsync(UserId);

                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var image = Request.Form.Files[i];
                        string filename = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
                        var fileExt = System.IO.Path.GetExtension(filename).Substring(1);
                        if (fileExt.ToLower() != "jpeg" && fileExt.ToLower() != "png" && fileExt.ToLower() != "jpg")
                        {
                            return Json(new { result = "error", message = "Please upload valid image file" });
                        }
                        filename = System.DateTime.Now.ToString("ddMMyyyyhhmmsss") + "." + fileExt;
                        string FullPath = this._hostingEnvironment.WebRootPath + "\\userImages\\" + filename;

                        using (FileStream output = System.IO.File.Create(FullPath))
                            image.CopyTo(output);

                        userIdentity.Image = "/userImages/" + filename;
                    }

                    var userStore = GetUserStore();
                    var result = await userStore.UpdateAsync(userIdentity);
                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true });
                    }
                    else
                    {
                        return Json(new { isSuccess = false });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }

        [CanAccessFilter]
        public async Task<IActionResult> ChangePassword()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return View("ChangePassword",user);
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdentity = await _userManager.FindByIdAsync(model.UserId);
                    var code = await _userManager.GeneratePasswordResetTokenAsync(userIdentity);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    var result = await _userManager.ResetPasswordAsync(userIdentity, code, model.Password);
                    if (result.Succeeded)
                    {
                        await _emailSender.SendEmailAsync(userIdentity.Email, "Changed Password",
                        $"Your Password Has Been Changed...");
                        return Json(new { isSuccess = true });

                    }
                    else
                    {
                        return Json(new { isSuccess = false });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }
        [CanAccessFilter]
        public async Task<IActionResult> Profile()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return View("Profile", user);
        }

        [HttpPost]
        public async Task<JsonResult> Update(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdentity = await _userManager.FindByIdAsync(model.UserId);
                    userIdentity.FullName = model.FullName;
                    userIdentity.Phone = model.Phone;
                    userIdentity.Address1 = model.Address1;
                    userIdentity.Address2 = model.Address2;
                    userIdentity.City = model.City;
                    userIdentity.Country = model.Country;
                    var userStore = GetUserStore();
                    var result = await userStore.UpdateAsync(userIdentity);
                    if (result.Succeeded)
                    {
                        return Json(new { isSuccess = true });
                    }
                    else
                    {
                        return Json(new { isSuccess = false });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { isSuccess = false });
        }
    }
}
