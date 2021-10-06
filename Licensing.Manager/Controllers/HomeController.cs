using Dapper;
using Licensing.Manager.Extension;
using Licensing.Manager.General;
using Licensing.Manager.Handlers.QueryHandlers.Admin;
using Licensing.Manager.Handlers.QueryHandlers.Products;
using Licensing.Manager.Helper;
using Licensing.Manager.Models;
using Licensing.Manager.ViewModels;
using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;
        private static IHttpContextAccessor _hcontext;

        public HomeController(SignInManager<ApplicationUser> signInManager, IMediator mediator, IHttpContextAccessor haccess)
        {
            _signInManager = signInManager;
            _mediator = mediator;
            _hcontext = haccess;
        }

        [CanAccessFilter]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProductFromDatabase()
        {
            List<ProductsVM> ProductData = await _mediator.Send(new ListProductFromDatabaseQuery());
            return Json(ProductData);
        }



        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public static async Task<List<MenuItemViewModel>> GetMenuItem(string userId = "")
        {
            try
            {

                if (string.IsNullOrEmpty(userId))
                {
                    userId = _hcontext.HttpContext.User.GetUserId();
                }
                var model = new SearchMenuItemViewModel { UserId = userId };
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetUserMenuAccess",
                         new { UserId = userId },
                        commandType: CommandType.StoredProcedure);

                    var menuList = new List<MenuItemViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        menuList = result.Select(x => new MenuItemViewModel
                        {
                            Id = x.Id,
                            MenuName = x.MenuName,
                            ParentManuName = x.ParentMenuName,
                            HasAccess = x.HasAccess,
                            UserId = x.UserId,
                            URL = x.URL
                        }).ToList();
                    }

                    return menuList;
                }
                
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public ActionResult Unauth()
        {
            return View();
        }

    }
}
