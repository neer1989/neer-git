using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
  public interface IDistrictAdmin
    {
        List<implementation_agency_master> GetImplementationAgency();

        int InsertDistrictSchemeUpdate(district_scheme_details obj);

        List<gp_amount_districtname_Result> SPAmountDistrictName();

        List<gp_amount_schemename_Result> SPSchemeName();

        List<gp_district_scheme_details_Result> SPDistrictSchemeDetails(int districtId);

        List<gp_GetAmount_Scheme_Result> SPGetAmounts(int? descid, int? fyid, int? pscheme, int? childsch1, int? childsch2, int? childsch3);

        List<district_GetUsedSchemeAmount_Result> GetUsedSchemeAmount();

    }
}
