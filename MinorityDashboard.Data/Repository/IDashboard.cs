using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
   public interface IDashboard
    {
        void testmethod();

        int InsertDeskTrans(deskdata_trans obj);

        int InsertDesk(desk_master obj);

        int InsertSubject(subject_master obj);

        List<subject_master> GetSubject();
        List<district_master> GetDistrict();
        List<scheme_master> GetScheme();
        List<desk_master> GetDesk();
        List<financialyear_master> GetFinancial();

        List<estimation_master> GetEstimation();
        List<GetDeskTransactionData_Result> GetTransDeskData();
        List<GetSubjectData_Result> GetSubjectData();
        List<GetSchemeData_Result> GetSchemeTransaction();
        int InsertDesk_Doc(deskdocument obj, List<string> doclist);
        List<GetDeskDocuments_Result> GetDeskDocumentsData();

        int UpdateDeskTrans(deskdata_trans obj);
        int UpdateSchems(scheme_master obj);
        int DeleteDeskTrans(deskdata_trans obj);

        int InsertScheme(scheme_master obj);

        List<trans_docfile> GetTransactionFile(int deskdocid);

        List<parentscheme> GetParentScheme();
        List<scheme_child1> GetSchemeChild1();
        List<scheme_child2> GetSchemeChild2();
        List<scheme_child3> GetSchemeChild3();

    }
}
