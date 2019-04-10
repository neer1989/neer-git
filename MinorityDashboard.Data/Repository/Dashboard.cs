using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MinorityDashboard.DataModel;


namespace MinorityDashboard.Data.Repository
{
    public class Dashboard : IDashboard
    {
        CommonRepository ObjCR = new CommonRepository();
        public List<desk_master> GetDesk()
        {
            // var tst = ObjCR.GetData<desk_master>(s => s.desk_id == 1);
            return ObjCR.GetData<desk_master>(); ;
        }

        public List<district_master> GetDistrict()
        {
            return ObjCR.GetData<district_master>();
        }

        public List<estimation_master> GetEstimation()
        {
            return ObjCR.GetData<estimation_master>();
        }

        public List<financialyear_master> GetFinancial()
        {
            return ObjCR.GetData<financialyear_master>();
        }

        public List<scheme_master> GetScheme()
        {
            return ObjCR.GetData<scheme_master>();
        }

        public List<parentscheme> GetParentScheme()
        {
            return ObjCR.GetData<parentscheme>();
        }
        public List<scheme_child1> GetSchemeChild1()
        {
            return ObjCR.GetData<scheme_child1>();
        }
        public List<scheme_child2> GetSchemeChild2()
        {
            return ObjCR.GetData<scheme_child2>();
        }
        public List<scheme_child3> GetSchemeChild3()
        {
            return ObjCR.GetData<scheme_child3>();
        }

        public List<parent_scheme_amt> GetParentScheme_amt()
        {
            return ObjCR.GetData<parent_scheme_amt>();
        }
        public List<scheme_child1_amt> GetSchemeChild1_amt()
        {
            return ObjCR.GetData<scheme_child1_amt>();
        }
        public List<scheme_child2_amt> GetSchemeChild2_amt()
        {
            return ObjCR.GetData<scheme_child2_amt>();
        }
        public List<scheme_child3_amt> GetSchemeChild3_amt()
        {
            return ObjCR.GetData<scheme_child3_amt>();
        }


        public List<GetSchemeData_Result> GetSchemeTransaction()
        {
            //return ObjCR.GetData<GetSchemeData_Result>();
            List<GetSchemeData_Result> lst = new List<GetSchemeData_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.GetSchemeData().ToList();
            }
            return lst;
        }

        public List<subject_master> GetSubject()
        {
            return ObjCR.GetData<subject_master>();
        }

        public List<trans_docfile> GetTransactionFile(int deskdocid)
        {
            List<trans_docfile> lst = new List<trans_docfile>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.trans_docfile.Where(s => s.deskdoc_id == deskdocid).ToList();

            }

            return lst;

            //  return ObjCR.GetData<trans_docfile>();
        }

        public List<GetDeskTransactionData_Result> GetTransDeskData()
        {
            List<GetDeskTransactionData_Result> lst = new List<GetDeskTransactionData_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.GetDeskTransactionData().ToList();

            }
            return lst;
        }

        public List<GetDeskDocuments_Result> GetDeskDocumentsData()
        {
            List<GetDeskDocuments_Result> lst = new List<GetDeskDocuments_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.GetDeskDocuments().ToList();
            }
            return lst;
        }
        public List<GetSubjectData_Result> GetSubjectData()
        {
            List<GetSubjectData_Result> lst = new List<GetSubjectData_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.GetSubjectData().ToList();
            }
            return lst;
        }

        public int InsertGR(grdetail obj)
        {
            grdetail returnobj = ObjCR.SaveData<grdetail>(obj);
            return returnobj.gr_id;
        }

        public int InsertGallery(gallery_master obj)
        {
            gallery_master returnobj = ObjCR.SaveData<gallery_master>(obj);
            return returnobj.gallery_id;
        }

        public int UpdateDeleteGR(grdetail obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var GRobj = db.grdetails.FirstOrDefault(x => x.gr_id == obj.gr_id);
                    GRobj.isactive = obj.isactive;
                    GRobj.gr_date = obj.gr_date;
                    GRobj.gr_file = obj.gr_file;
                    GRobj.keywords_e = obj.keywords_e;
                    GRobj.keywords_m = obj.keywords_m;
                    GRobj.unique_code_e = obj.unique_code_e;
                    GRobj.unique_code_m = obj.unique_code_m;
                    GRobj.updated_by = obj.updated_by;
                    GRobj.updated_date = obj.updated_date;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }

        public int UpdateDeleteGallery(gallery_master obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var Galleryobj = db.gallery_master.FirstOrDefault(x => x.gallery_id == obj.gallery_id);
                    Galleryobj.isactive = obj.isactive;
                    Galleryobj.file_extension = obj.file_extension;
                    Galleryobj.file_name = obj.file_name;
                    Galleryobj.posted_date = obj.posted_date;
                    Galleryobj.title_e = obj.title_e;
                    Galleryobj.title_m = obj.title_m;                   
                    Galleryobj.updated_by = obj.updated_by;
                    Galleryobj.updated_date = obj.updated_date;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }


        public int UpdateDeleteSchemDesc(scheme_desc_mapping obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var SDobj = db.scheme_desc_mapping.FirstOrDefault(x => x.scheme_des_id == obj.scheme_des_id);
                    SDobj.isactive = obj.isactive;
                    SDobj.parent_scheme_id = obj.parent_scheme_id;
                    SDobj.scheme_description_e = obj.scheme_description_e;
                    SDobj.scheme_description_m = obj.scheme_description_m;
                    SDobj.scheme_id_child1 = obj.scheme_id_child1;
                    SDobj.scheme_id_child2 = obj.scheme_id_child2;
                    SDobj.scheme_id_child3 = obj.scheme_id_child3;
                    SDobj.updated_by = obj.updated_by;
                    SDobj.updated_date = obj.updated_date;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }



        public int InsertDeskTrans(deskdata_trans obj)
        {
            deskdata_trans returnobj = ObjCR.SaveData<deskdata_trans>(obj);
            return returnobj.tran_id;
        }

        public List<grdetail> GetGRList()
        {
            return ObjCR.GetData<grdetail>();
        }

        public List<gallery_master> GetGalleryList()
        {
            return ObjCR.GetData<gallery_master>();
        }

        public int InsertSchemeAllotment(scheme_amount_allocation obj)
        {
            scheme_amount_allocation returnobj = ObjCR.SaveData<scheme_amount_allocation>(obj);
            return returnobj.id;
        }

        public int InsertSchemeDescMapping(scheme_desc_mapping obj)
        {
            scheme_desc_mapping returnobj = ObjCR.SaveData<scheme_desc_mapping>(obj);
            return returnobj.scheme_des_id;
        }

        public int InsertDesk(desk_master obj)
        {
            desk_master returnobj = ObjCR.SaveData<desk_master>(obj);
            return returnobj.desk_id;
        }



        public int InsertParentScheme_amt(parent_scheme_amt obj)
        {
            parent_scheme_amt returnobj = ObjCR.SaveData<parent_scheme_amt>(obj);
            return returnobj.allotment_amt_parent_id;
        }

        public int InsertScheme_Child1_amt(scheme_child1_amt obj)
        {
            scheme_child1_amt returnobj = ObjCR.SaveData<scheme_child1_amt>(obj);
            return returnobj.allotment_amt_child1_id;
        }


        public int InsertDesk_Doc(deskdocument obj, List<string> doclist)
        {
            deskdocument returnobj = ObjCR.SaveData<deskdocument>(obj);

            if (returnobj.deskdoc_id > 0)
            {
                foreach (string x in doclist)
                {
                    trans_docfile obj_tdf = new trans_docfile();
                    obj_tdf.deskdoc_id = returnobj.deskdoc_id;
                    obj_tdf.doc_filename = x;
                    obj_tdf.doc_title = returnobj.doc_title;
                    obj_tdf.is_active = true;
                    ObjCR.SaveData<trans_docfile>(obj_tdf);
                }
            }

            return returnobj.deskdoc_id;
        }

        public int InsertSubject(subject_master obj)
        {
            subject_master returnobj = ObjCR.SaveData<subject_master>(obj);
            return returnobj.sub_id;
        }

        public int InsertScheme(scheme_master obj)
        {
            scheme_master returnobj = ObjCR.SaveData<scheme_master>(obj);
            return returnobj.scheme_id;
        }

        public int UpdateDeskTrans(deskdata_trans obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var desktransobj = db.deskdata_trans.FirstOrDefault(x => x.tran_id == obj.tran_id);
                    desktransobj.budgetary_provision_amt = obj.budgetary_provision_amt;
                    desktransobj.desk_id = obj.desk_id;
                    desktransobj.des_id = obj.des_id;
                    desktransobj.est_id = obj.est_id;
                    desktransobj.fin_y_id = obj.fin_y_id;
                    desktransobj.actual_allocation_amt = obj.actual_allocation_amt;
                    desktransobj.actual_expenditure_amt = obj.actual_expenditure_amt;
                    desktransobj.actual_remaining_amt = obj.actual_remaining_amt;
                    desktransobj.scheme_id = obj.scheme_id;
                    desktransobj.sub_id = obj.sub_id;
                    desktransobj.tran_id = obj.tran_id;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }
        public int UpdateSchemeAllotment(scheme_amount_allocation obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var desktransobj = db.scheme_amount_allocation.FirstOrDefault(x => x.id == obj.id);                    
                    desktransobj.fin_y_id = obj.fin_y_id;
                    desktransobj.des_id = obj.des_id;
                    desktransobj.imp_agency_id = obj.imp_agency_id;               
                    desktransobj.actual_allocation_amt = obj.actual_allocation_amt;
                    desktransobj.budgetary_provision_amt = obj.budgetary_provision_amt;
                    desktransobj.parent_scheme_id = obj.parent_scheme_id;
                    desktransobj.scheme_id_child1 = obj.scheme_id_child1;
                    desktransobj.scheme_id_child2 = obj.scheme_id_child2;
                    desktransobj.scheme_id_child3 = obj.scheme_id_child3;
                    desktransobj.updated_by = obj.updated_by;
                    desktransobj.updated_date = obj.updated_date;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }


        public int UpdateSchems(scheme_master obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var desktransobj = db.scheme_master.FirstOrDefault(x => x.scheme_id == obj.scheme_id);
                    desktransobj.desk_id = obj.desk_id;
                    desktransobj.scheme_description = obj.scheme_description;
                    desktransobj.scheme_name = obj.scheme_name;
                    desktransobj.sub_id = obj.sub_id;
                    desktransobj.updated_by = obj.updated_by;
                    desktransobj.updated_date = obj.updated_date;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }
        public int UpdateDeleteLatestNews(latest_news obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var LNobj = db.latest_news.FirstOrDefault(x => x.latest_news_id == obj.latest_news_id);
                    LNobj.isactive = obj.isactive;
                    LNobj.news_date = obj.news_date;
                    LNobj.news_description_e = obj.news_description_e;
                    LNobj.news_description_m = obj.news_description_m;
                    LNobj.news_e = obj.news_e;
                    LNobj.news_m = obj.news_m;
                    LNobj.updated_by = obj.updated_by;
                    LNobj.updated_date = obj.updated_date;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }

        public int DeleteDeskTrans(deskdata_trans obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var desktransobj = db.deskdata_trans.FirstOrDefault(x => x.tran_id == obj.tran_id);
                    deskdata_trans_bak obj_b = new deskdata_trans_bak();
                    obj_b.committee_amount = obj.budgetary_provision_amt;
                    obj_b.desk_id = obj.desk_id;
                    obj_b.des_id = obj.des_id;
                    obj_b.est_id = obj.est_id;
                    obj_b.fin_y_id = obj.fin_y_id;
                    obj_b.scheme_amount = obj.actual_allocation_amt;
                    obj_b.scheme_id = obj.scheme_id;
                    obj_b.sub_id = obj.sub_id;
                    obj_b.tran_id = obj.tran_id;
                    db.deskdata_trans_bak.Add(obj_b);
                    db.deskdata_trans.Remove(desktransobj);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }


        public List<GetSchemeDesc_Result> GetFilteredSchemeDesc(int ParentId, int Childschm1, int Childschm2, int Childschm3)
        {
            List<GetSchemeDesc_Result> lstsdm = new List<GetSchemeDesc_Result>();
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    lstsdm = db.GetSchemeDesc(ParentId, Childschm1, Childschm2, Childschm3).ToList();
                }
            }
            catch (Exception ex)
            {
                return lstsdm;
            }
            return lstsdm;
        }


        public int InsertLatestNews(latest_news obj)
        {
            latest_news returnobj = ObjCR.SaveData<latest_news>(obj);
            return returnobj.latest_news_id;
        }

        public List<latest_news> GetLatestNewsList()
        {
            return ObjCR.GetData<latest_news>().Where(s => s.isactive == true).ToList();
        }

        public List<latest_news> GetLatestNewsById(int id)
        {
            return ObjCR.GetData<latest_news>().Where(s => s.latest_news_id == id).ToList();
        }


        public int CheckSchemeDescription(int ParentId, int Childschm1, int Childschm2, int Childschm3)
        {
            try
            {
                return ObjCR.GetData<scheme_desc_mapping>().Where(s => s.parent_scheme_id == ParentId && s.scheme_id_child1 == Childschm1 && s.scheme_id_child2 == Childschm2 && s.scheme_id_child3 == Childschm3).ToList().Count();
            }
            catch
            {
                return 0;
            }
        }

        public List<GetSchemeAmountAllocation_Result> GetSchemeAmountAllocation()
        {
            List<GetSchemeAmountAllocation_Result> lst = new List<GetSchemeAmountAllocation_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.GetSchemeAmountAllocation().ToList();
            }
            return lst;
        }



        public List<org_structure> GetOrgList()
        {
            return ObjCR.GetData<org_structure>();
        }

        public int InsertOrgStructure(org_structure obj)
        {
            org_structure returnobj = ObjCR.SaveData<org_structure>(obj);
            return returnobj.employee_id;
        }

        public int UpdateDeleteOrgStructure(org_structure obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var upobj = db.org_structure.FirstOrDefault(x => x.employee_id == obj.employee_id);
                    upobj.name = obj.name;
                    upobj.designation = obj.designation;
                    upobj.reporting_manager = obj.reporting_manager;
                    upobj.reporting_manager_name = obj.reporting_manager_name;
                    upobj.employee_id = obj.employee_id;
                    upobj.updated_by = obj.updated_by;
                    upobj.updated_date = obj.updated_date;
                    upobj.isactive = obj.isactive;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }




        public List<keyperson_master> GetKeyPerson()
        {
            return ObjCR.GetData<keyperson_master>();
        }

        public int InsertKeyPerson(keyperson_master obj)
        {
            keyperson_master returnobj = ObjCR.SaveData<keyperson_master>(obj);
            return returnobj.keyperson_id;
        }

        public int UpdateKeyPerson(keyperson_master obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var upobj = db.keyperson_master.FirstOrDefault(x => x.keyperson_id == obj.keyperson_id);
                    upobj.designation_e = obj.designation_e;
                    upobj.designation_m = obj.designation_m;
                    upobj.display_order = obj.display_order;
                    upobj.person_image = obj.person_image;
                    upobj.person_name_e = obj.person_name_e;
                    upobj.person_name_m = obj.person_name_m;
                    upobj.updated_by = obj.updated_by;
                    upobj.updated_date = obj.updated_date;
                    upobj.isactive = obj.isactive;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }


        public List<front_slider> GetFrontSlider()
        {
            return ObjCR.GetData<front_slider>();
        }

        public int InsertFrontSlider(front_slider obj)
        {
            front_slider returnobj = ObjCR.SaveData<front_slider>(obj);
            return returnobj.slider_id;
        }

        public int UpdateFrontSlider(front_slider obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var upobj = db.front_slider.FirstOrDefault(x => x.slider_id == obj.slider_id);
                    upobj.slider_img = obj.slider_img;
                    upobj.slide_order = obj.slide_order;
                    upobj.title_e = obj.title_e;
                    upobj.title_m = obj.title_m;
                    upobj.updated_by = obj.updated_by;
                    upobj.updated_date = obj.updated_date;
                    upobj.isactive = obj.isactive;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }



        public List<citizen_charter> GetCitizenCharter()
        {
            return ObjCR.GetData<citizen_charter>();
        }

        public int InsertCitizenCharter(citizen_charter obj)
        {
            citizen_charter returnobj = ObjCR.SaveData<citizen_charter>(obj);
            return returnobj.cc_id;
        }

        public int UpdateCitizenCharter(citizen_charter obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var upobj = db.citizen_charter.FirstOrDefault(x => x.cc_id == obj.cc_id);
                    upobj.file_path = obj.file_path;
                    upobj.name_e = obj.name_e;
                    upobj.name_m = obj.name_m;               
                    upobj.updated_by = obj.updated_by;
                    upobj.updated_date = obj.updated_date;
                    upobj.isactive = obj.isactive;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }


        public List<advertisement_master> GetAdvertisement()
        {
            return ObjCR.GetData<advertisement_master>();
        }

        public int InsertAdvertisement(advertisement_master obj)
        {
            advertisement_master returnobj = ObjCR.SaveData<advertisement_master>(obj);
            return returnobj.adv_id;
        }

        public int UpdateAdvertisement(advertisement_master obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var upobj = db.advertisement_master.FirstOrDefault(x => x.adv_id == obj.adv_id);
                    upobj.adv_title_e = obj.adv_title_e;
                    upobj.adv_title_m = obj.adv_title_m;
                    upobj.file_name = obj.file_name;
                    upobj.updated_by = obj.updated_by;
                    upobj.updated_date = obj.updated_date;
                    upobj.isactive = obj.isactive;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }


        public void testmethod()
        {
            // throw new NotImplementedException();
        }


    }
}
