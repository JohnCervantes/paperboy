using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaperBoyMain0.Data
{
    public class ImageModel
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string imgPath {get; set;}
    }
}
