using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRMenu.Models
{
	public class User
	{
		public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(100)")]
        public string UserName { get; set; } = "";
		public string PassWord { get; set; } = "";
		public string Name { get; set; } = "";
        public byte StateId { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("StateId")]
        public State? State { get; set; }
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
    }
}

