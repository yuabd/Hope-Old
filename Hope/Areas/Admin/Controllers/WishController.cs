using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hope.Services;
using Hope.Models;
using System.IO;

namespace Hope.Areas.Admin.Controllers
{
    [Authorize]
    public class WishController : Controller
    {
        private SiteService siteService = new SiteService();
        //
        // GET: /Admin/Wish/

        SiteDataContext db = new SiteDataContext();

        public ActionResult Index(int? page, bool? id, string StudentName, string Tel, string name)
        {
            var wishes = new List<Wish>();
            ViewBag.ID = id;
            if (id != null)
            {
                wishes = siteService.GetWishes((bool)id).ToList();
            }
            else
            {
                wishes = siteService.GetWishes().ToList();
            }

            if (!string.IsNullOrWhiteSpace(StudentName))
            {
                wishes = (from l in wishes
                          where l.StudentName.Contains(StudentName)
                          select l).ToList();
            }

            if (!string.IsNullOrWhiteSpace(Tel))
            {
                wishes = (from l in wishes
                          where l.Tel.Contains(Tel)
                          select l).ToList();
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                wishes = (from l in wishes
                          join a in db.Applies on l.WishID equals a.WishID
                          where a.ContactName.Contains(name)
                          select l).ToList();
            }

            var pwishes = new Paginated<Wish>(wishes, page ?? 1, 20);

            return View(pwishes);
        }

        public ActionResult Add()
        {
            var wish = new Wish();

            return View(wish);
        }

        [HttpPost]
        public ActionResult Add(Wish wish)
        {
            if (ModelState.IsValid)
            {
                siteService.InsertWish(wish);
                siteService.Save();

                return RedirectToAction("Index");
            }

            return View(wish);
        }

        public ActionResult Edit(int id)
        {
            var wish = siteService.GetWish(id);

            return View(wish);
        }

        [HttpPost]
        public ActionResult Edit(Wish wish)
        {
            if (ModelState.IsValid)
            {
                siteService.UpdateWish(wish);
                siteService.Save();

                return RedirectToAction("Index");
            }

            return View(wish);
        }

        public ActionResult Delete(int id)
        {
            var wish = siteService.GetWish(id);
            siteService.DeleteApply(id);

            siteService.DeleteWish(wish);
            siteService.Save();

            return RedirectToAction("Index");
        }

        public ActionResult DeleteApply(int id)
        {
            var wish = siteService.GetWish(id);
            siteService.DeleteApply(id);
            wish.IsApply = false;
            siteService.Save();

            return RedirectToAction("Index");
        }

        public ActionResult GetApply(int id)
        {
            var apply = siteService.GetApply(id);

            return View(apply);
        }

        public ActionResult EditApply(Apply apply)
        {
            siteService.UpdateApply(apply);

            return RedirectToAction("Index");
        }

        //public JsonResult GetApply(int applyID)
        //{
        //    var apply = siteService.GetApply(applyID);

        //    var result = Json(new
        //    {
        //        ContactName = apply.ContactName,
        //        ContactAddress = apply.ContactAddress,
        //        ContactEmail = apply.ContactEmail,
        //        ContactTel = apply.ContactTel,
        //        Message = apply.Message
        //    });

        //    return result;
        //}

        public ActionResult Apply(int id)
        {
            return View();
        }

        public ActionResult UploadExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            string line;
            string[] arrayLine;

            if (file.ContentLength > 0)
            {
                if (file.ContentType == "application/vnd.ms-excel")
                {
                    string filename = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
                    string filePath = HttpContext.Server.MapPath("/content/" + filename);
                    file.SaveAs(filePath);
                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            var wish = new Wish();
                            arrayLine = line.Split(new char[] { ',' });

                            if (arrayLine.Count() <= 8)
                            {
                                continue;
                            }
                            
                            wish.StudentName = arrayLine[0];
                            wish.SchoolName = arrayLine[5];
                            wish.Gender = arrayLine[2];
                            wish.Address = arrayLine[3];
                            wish.Age = arrayLine[4];
                            wish.Tel = arrayLine[6];
                            wish.Situation = arrayLine[7];
                            wish.Detail = arrayLine[8];
                            wish.DateCreated = DateTime.Now;
                            wish.IsApply = false;

                            siteService.InsertWish(wish);

                            siteService.Save();

                            wish.IsApply = true;
                            var apply = new Apply();

                            apply.ContactName = "指挥中心";
                            apply.ContactAddress = "指挥中心";
                            apply.ContactTel = "13606918910";
                            apply.WishID = wish.WishID;
                            apply.Date = DateTime.Now;

                            siteService.InsertApply(apply);

                            siteService.Save();
                        }
                    }
                }
            }

            return View();
        }

    }
}
