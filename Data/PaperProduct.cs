using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaperBoyMain0.Data
{
    public class PaperProduct
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Title")]
        [Required]
        public string title { get; set; }
        [Display(Name = "Image Path")]
        [Required]
        public string imgPath { get; set; }
        [Display(Name = "Information")]
        [Required]
        public string info { get; set; }
        [Display(Name = "Price")]
        [Range(1, 100000)]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public decimal price { get; set; }
        [Display(Name = "Rating")]
        [Required]
        public string rating { get; set; }
        [Display(Name = "Stock")]
        [Required]
        public string stock { get; set; }

        [Display(Name = "Purchase Link")]
        [Required]
        public string purchaseLink { get; set; }
    }
}
