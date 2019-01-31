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

        public int InsertDeskTrans(deskdata_trans obj)
        {
            deskdata_trans returnobj = ObjCR.SaveData<deskdata_trans>(obj);
            return returnobj.tran_id;
        }

        public List<grdetail> GetGRList()
        {
            return ObjCR.GetData<grdetail>();
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

        public void testmethod()
        {
            // throw new NotImplementedException();
        }


    }
}
