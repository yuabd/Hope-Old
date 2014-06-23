using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hope.Models
{
	public class Wish
	{
		public int WishID { get; set; }
		[MaxLength(20)]
		[Required]
		public string StudentName { get; set; }
		[MaxLength(100)]
		public string SchoolName { get; set; }
		[MaxLength(10)]
		public string Age { get; set; }
		[MaxLength(10)]
		public string Gender { get; set; }
		[MaxLength(500)]
		public string Address { get; set; }
		[MaxLength(100)]
		public string Tel { get; set; }
		[MaxLength(500)]
		public string Situation { get; set; }
		[MaxLength(2000)]
		public string Detail { get; set; }

		public bool IsApply { get; set; }

        public DateTime DateCreated { get; set; }

		public virtual Apply Apply { get; set; }
	}

    public class PaiHang
    {
        public string Name { get; set; }

        public int Count { get; set; }
    }
}