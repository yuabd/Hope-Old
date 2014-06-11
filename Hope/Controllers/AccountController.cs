using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hope.Models;
using System.Web.Security;
using System.Configuration;

namespace Hope.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

		public ActionResult Index(string returnUrl)
		{
			LoginViewModel model = new LoginViewModel();

			return View(model);
		}

		[HttpPost]
		public ActionResult Index(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
                var password = ConfigurationManager.AppSettings["password"];

                if (model.UserID == "admin" && model.Password == password)
				{
					FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
					   1, // Ticket version
					   model.UserID, // Username associated with ticket
					   DateTime.Now, // Date/time issued
					   DateTime.Now.AddDays(30), // Date/time to expire
					   true, // persistent user cookie
					   "1", // User-data, in this case the roles
					   FormsAuthentication.FormsCookiePath);// Path cookie valid for Encrypt the cookie using the machine key for secure transport

					string hash = FormsAuthentication.Encrypt(ticket);
					HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

					cookie.Expires = DateTime.Now.AddHours(12);

					if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

					// Add the cookie to the list for outgoing response
					HttpContext.Response.Cookies.Add(cookie);

					if (returnUrl != null)
						return Redirect(returnUrl.ToString());
					else
						return RedirectToAction("Index", "Wish", new { area = "admin" });
				}
				else
				{
					ViewBag.LoginError = "用户名或者密码错误！";
					return View(model);
				}
			}
			else
			{
				return View(model);
			}
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Abandon();

			return Redirect("/");
		}

    }
}
