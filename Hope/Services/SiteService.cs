﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hope.Models;

namespace Hope.Services
{
	public class SiteService
	{
		private SiteDataContext db = new SiteDataContext();

		public void InsertWish(Wish wish ) 
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