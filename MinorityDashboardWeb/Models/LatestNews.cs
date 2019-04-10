using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class LatestNews
    {
        public int latest_news_id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string latest_news_e { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string latest_news_m { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string news_description_e { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string news_description_m { get; set; }
        [Required(ErrorMessage = "News date is required")]
        public DateTime news_date { get; set; }
        public bool isactive { get; set; }

        public List<latest_news> lstLatestNews { get; set; }

    }
}