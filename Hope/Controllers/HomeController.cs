using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hope.Services;
using Hope.Models;

namespace Hope.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
		private SiteService siteService = new SiteService();

        public ActionResult Index(int? page, int? id, int? state)
        {
            var wishes = siteService.GetWishes().ToList();
            if (id != null && id != 0)
            {
                wishes = (from l in wishes
                          where l.DateCreated.Year == id
                          select l).ToList();
            }

            if (state != null)
            {
                try
                {
                    bool a = Convert.ToBoolean(state);

                    wishes = (from l in wishes
                              where l.IsApply == a
                              select l).ToList();
                }
                catch (Exception e)
                {
                }
                
            }

			var pwishes = new Paginated<Wish>(wishes, page ?? 1, 20);

            return View(pwishes);
        }

		[HttpPost]
		public ActionResult Apply(Apply apply)
		{
			if (ModelState.IsValid)
			{
				var wish = siteService.GetWish(apply.WishID);
				wish.IsApply = true;
				apply.Date = DateTime.Now;
				siteService.InsertApply(apply);
				siteService.Save();

				return RedirectToAction("Index");
			}

			return View(apply);
		}
    }
}
