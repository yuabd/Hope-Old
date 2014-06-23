using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hope.Models;

namespace Hope.Services
{
    public class SiteService
    {
        private SiteDataContext db = new SiteDataContext();

        public void InsertWish(Wish wish)
        {
            wish.DateCreated = DateTime.Now;
            db.Wishes.Add(wish);
        }

        public Wish GetWish(int wishID)
        {
            return db.Wishes.Find(wishID);
        }

        public void UpdateWish(Wish wish)
        {
            var w = GetWish(wish.WishID);

            w.Address = wish.Address;
            w.Age = wish.Age;
            w.Detail = wish.Detail;
            w.Gender = wish.Gender;
            w.IsApply = wish.IsApply;
            w.SchoolName = wish.SchoolName;
            w.Situation = wish.Situation;
            w.StudentName = wish.StudentName;
            w.Tel = wish.Tel;
        }

        public void DeleteWish(Wish wish)
        {
            db.Wishes.Remove(wish);
        }

        public IQueryable<Wish> GetWishes()
        {
            return db.Wishes.OrderByDescending(m => m.DateCreated);
        }

        public List<Wish> SearchWishByApplyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<Wish>();
            }

            var list = (from l in db.Wishes
                        join a in db.Applies on l.WishID equals a.WishID
                        where a.ContactName.Contains(name) || l.StudentName.Contains(name)
                        select l).ToList();

            return list;
        }

        public List<PaiHang> GetPaiHangBang(string year)
        {
            var id = 0;
            try
            {
                id = Convert.ToInt32(year);
            }
            catch (Exception e)
            {
                
            }

            var list = (from l in db.Applies
                        where id == 0 ? true : l.Date.Year == id
                        //join a in db.Applies on l.WishID equals a.WishID
                        group l by l.ContactName into g
                        orderby g.Count() descending
                        select new PaiHang()
                        {
                            Count = g.Count(),
                            Name = g.Key
                        }).ToList();

            return list;
        }

        public IQueryable<Wish> GetWishes(bool isApply)
        {
            return db.Wishes.Where(m => m.IsApply == isApply).OrderByDescending(m => m.DateCreated);
        }

        public void InsertApply(Apply apply)
        {
            db.Applies.Add(apply);
        }

        public Apply GetApply(int applyID)
        {
            return db.Applies.Find(applyID);
        }

        public void UpdateApply(Apply apply)
        {
            var a = db.Applies.FirstOrDefault(m => m.WishID == apply.WishID);

            if (a != null)
            {
                a.ContactAddress = apply.ContactAddress;
                a.ContactName = apply.ContactName;
                a.ContactTel = apply.ContactTel;
                a.Message = apply.Message;

                db.SaveChanges();
            }
        }

        public void DeleteApply(int applyID)
        {
            var apply = GetApply(applyID);

            if (apply != null)
            {
                db.Applies.Remove(apply);
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}