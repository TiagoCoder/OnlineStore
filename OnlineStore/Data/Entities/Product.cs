using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float Price { get; set; }


        [Display(Name = "Last purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last sale")]
        public DateTime? LastSale { get; set; }


        [Display(Name = "Is product available?")]
        public bool IsAvailable { get; set; }


        public int Stock { get; set; }


        [Required]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a category")]
        public int CategoryId { get; set; }


        public Category Category { get; set; }


        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
    }
}
