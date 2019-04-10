using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class SliderModel
    {
        public int slider_id { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string title_e { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string title_m { get; set; }
        public string slider_img { get; set; }
        [Required(ErrorMessage = "Order is Required")]
        public int slide_order { get; set; }
        public bool isactive { get; set; }

        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase[] File { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_date { get; set; }

        public List<front_slider> lstFrontSlider { get; set; }

    }
}