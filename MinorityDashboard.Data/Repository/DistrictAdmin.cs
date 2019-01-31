using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
    public class DistrictAdmin : IDistrictAdmin
    {
        CommonRepository ObjCR = new CommonRepository();
        public List<implementation_agency_master> GetImplementationAgency()
        {
               return ObjCR.GetData<implementation_agency_master>();
          
        }

        public int InsertDistrictSchemeUpdate(district_scheme_details obj)
        {
            district_scheme_details returnobj = ObjCR.SaveData<district_scheme_details>(obj);
            return returnobj.id;
        }

        public List<gp_amount_districtname_Result> SPAmountDistrictName()
        {
            List<gp_amount_districtname_Result> lstsdm = new List<gp_amount_districtname_Result>();
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    lstsdm = db.gp_amount_districtname().ToList();
                }
            }
            catch (Exception ex)
            {
                return lstsdm;
            }
            return lstsdm;

        }

        public List<gp_district_scheme_details_Result> SPDistrictSchemeDetails(int districtId)
        {
            List<gp_district_scheme_details_Result> lstsdm = new List<gp_district_scheme_details_Result>();
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    lstsdm = db.gp_district_scheme_details(districtId).ToList();
                }
            }
            catch (Exception ex)
            {
                return lstsdm;
            }
            return lstsdm;

        }

        public List<gp_amount_schemename_Result> SPSchemeName()
        {
            List<gp_amount_schemename_Result> lstsdm = new List<gp_amount_schemename_Result>();
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    lstsdm = db.gp_amount_schemename().ToList();
                }
            }
            catch (Exception ex)
            {
                return lstsdm;
            }
            return lstsdm;

        }

        public List<gp_GetAmount_Scheme_Result> SPGetAmounts(int? descid, int? fyid, int? pscheme, int? childsch1, int? childsch2, int? childsch3)
        {
            List<gp_GetAmount_Scheme_Result> lstsdm = new List<gp_GetAmount_Scheme_Result>();
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    lstsdm = db.gp_GetAmount_Scheme(pscheme, childsch1, childsch2, childsch3, descid, fyid).ToList();
                }
            }
            catch (Exception ex)
            {
                return lstsdm;
            }
            return lstsdm;

        }


    }
}
