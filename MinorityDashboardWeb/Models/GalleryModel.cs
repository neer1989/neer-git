using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class GalleryModel
    {
        public int gallery_id { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string title_e { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string title_m { get; set; }
        public string file_name { get; set; }
        public string file_extension { get; set; }
        [Required(ErrorMessage = "Posted is Required")]
        public DateTime posted_date { get; set; }
        public bool isactive { get; set; }
        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase[] File { get; set; }

        public List<gallery_master> lstGalleryList { get; set; }

    }
}