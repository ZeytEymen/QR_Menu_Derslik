using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace QRMenu.Models
{
    public class Food
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("İsim")]
        public string Name { get; set; } = "";
        [Range(0, float.MaxValue)]
        public float Price { get; set; }
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Description { get; set; }
        public byte StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public State? State { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        [NotMapped]
        public IFormFile? Picture { get; set; }
        [Column(TypeName = "image")]
        public byte[]? Image { get; set; }
        public string? FileData { get; set; }
    }
}

