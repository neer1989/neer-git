using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class HomeModel
    {

        public List<latest_news> lstLatestNews { get; set; }
        public List<keyperson_master> lstKeyPerson { get; set; }
        public List<front_slider> lstFrontSlider { get; set; }
        public List<citizen_charter> lstCitizenCharterList { get; set; }

        public List<advertisement_master> lstAdvertisementList { get; set; }

    }
}