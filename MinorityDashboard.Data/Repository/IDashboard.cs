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
        int InsertSchemeAllotment(scheme_amount_allocation obj);

        int UpdateSchemeAllotment(scheme_amount_allocation obj);

        List<trans_docfile> GetTransactionFile(int deskdocid);

        List<parentscheme> GetParentScheme();
        List<scheme_child1> GetSchemeChild1();
        List<scheme_child2> GetSchemeChild2();
        List<scheme_child3> GetSchemeChild3();
        List<GetSchemeDesc_Result> GetFilteredSchemeDesc(int ParentId, int Childschm1, int Childschm2, int Childschm3);
        int InsertSchemeDescMapping(scheme_desc_mapping obj);


        List<parent_scheme_amt> GetParentScheme_amt();
        List<scheme_child1_amt> GetSchemeChild1_amt();
        List<scheme_child2_amt> GetSchemeChild2_amt();
        List<scheme_child3_amt> GetSchemeChild3_amt();


        int InsertGR(grdetail obj);
        List<grdetail> GetGRList();

        int InsertLatestNews(latest_news obj);
        List<latest_news> GetLatestNewsList();
        List<latest_news> GetLatestNewsById(int id);

        int UpdateDeleteLatestNews(latest_news obj);
        int UpdateDeleteGR(grdetail obj);
        int UpdateDeleteSchemDesc(scheme_desc_mapping obj);
        int CheckSchemeDescription(int ParentId, int Childschm1, int Childschm2, int Childschm3);
        List<GetSchemeAmountAllocation_Result> GetSchemeAmountAllocation();

        int InsertGallery(gallery_master obj);
        List<gallery_master> GetGalleryList();
        int UpdateDeleteGallery(gallery_master obj);

        List<org_structure> GetOrgList();
        int InsertOrgStructure(org_structure obj);
        int UpdateDeleteOrgStructure(org_structure obj);

        List<keyperson_master> GetKeyPerson();
        int InsertKeyPerson(keyperson_master obj);
        int UpdateKeyPerson(keyperson_master obj);

        List<front_slider> GetFrontSlider();
        int InsertFrontSlider(front_slider obj);
        int UpdateFrontSlider(front_slider obj);

        List<citizen_charter> GetCitizenCharter();
        int InsertCitizenCharter(citizen_charter obj);
        int UpdateCitizenCharter(citizen_charter obj);


        List<advertisement_master> GetAdvertisement();
        int InsertAdvertisement(advertisement_master obj);
        int UpdateAdvertisement(advertisement_master obj);
    }
}
