using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hope.Models
{
	public class User
	{
		public int UserID { get; set; }
		[MaxLength(32)]
		[Required]
		public string Password { get; set; }
	}
}