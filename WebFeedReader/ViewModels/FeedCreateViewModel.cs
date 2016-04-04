using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFeedReader.ViewModels
{
    public class FeedCreateViewModel
    {
        [Required]
        [StringLength(4000)]
        public string Url { get; set; }
    }
}