using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hope.Models
{
	public class Apply
	{
		[Key]
		[ForeignKey("Wish")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int WishID { get; set; }
		[MaxLength(20)]
		[Required]
		public string ContactName { get; set; }
		[Required]
		[MaxLength(100)]
		public string ContactAddress { get; set; }
		//[Required]
		//[RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "格式错误")]
		//public string ContactEmail { get; set; }
		[MaxLength(100)]
		[Required]
		public string ContactTel { get; set; }
		[MaxLength(500)]
		public string Message { get; set; }
		public DateTime Date { get; set; }

		public virtual Wish Wish { get; set; }
	}
}