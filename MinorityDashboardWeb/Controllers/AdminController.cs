using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboardWeb;
using MinorityDashboardWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Unity;

namespace MinorityDashboard.Web.Controllers
{
    [CustomAuthorize]
    public class AdminController : BaseController
    {
        // GET: Admin
        IDashboard objDashboard; // = new Dashboard();
        IDistrictAdmin objDistrictAdmin;
        IUnityContainer unitycontainer = new UnityContainer();


        public AdminController()
        {
            unitycontainer.RegisterType<IDashboard, Dashboard>();
            unitycontainer.RegisterType<IDistrictAdmin, DistrictAdmin>();
            objDashboard = unitycontainer.Resolve<Dashboard>();
            objDistrictAdmin = unitycontainer.Resolve<DistrictAdmin>();

        }


        public ActionResult Index()
        {
            //  Success(string.Format("<b>{0}</b> was successfully added to the database.", "neer..."), true);

            //List<district_master> lst = objDashboard.GetDistrict();

            //JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


            //ViewBag.DataPoints = JsonConvert.SerializeObject(lst, _jsonSetting);

            //List<object> iData = new List<object>();
            //List<int> lststr1 = new List<int>();
            //List<string> lststr2 = new List<string>();
            //foreach (district_master dm in lst)
            //{
            //    List<object> x = new List<object>();
            //    lststr1.Add(dm.des_id);
            //    lststr2.Add(dm.des_name.Trim());

            //}
            //iData.Add(lststr1);
            //iData.Add(lststr2);

            //ViewBag.DataPointsChart1 = JsonConvert.SerializeObject(lststr1, _jsonSetting).Trim('"');
            // ViewBag.DataPointsChart2 = JsonConvert.SerializeObject(lststr2, _jsonSetting).Trim('"');


         List<gp_district_scheme_details_Result> lstdsd = objDistrictAdmin.SPDistrictSchemeDetails(0);



            return View(lstdsd);
        }

        [HttpPost]
        public JsonResult NewChart()
        {
            List<object> iData = new List<object>();
            List<gp_amount_districtname_Result> lst = objDistrictAdmin.SPAmountDistrictName();


            List<gp_amount_schemename_Result> lstSchemeB = objDistrictAdmin.SPSchemeName();


            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


            ViewBag.DataPoints = JsonConvert.SerializeObject(lst, _jsonSetting);


            List<decimal> lststr1 = new List<decimal>();
            List<string> lststr2 = new List<string>();
            List<string> lststr3 = new List<string>();

            List<decimal> lststr4 = new List<decimal>();
            List<string> lststr5 = new List<string>();
            int icolorid = 0;
            foreach (gp_amount_districtname_Result dm in lst)
            {
                List<object> x = new List<object>();
                lststr1.Add(Convert.ToInt32(dm.amt));
                lststr2.Add(dm.des_name.Trim());
                //  lststr3.Add("rgb(255, 99, 132)");
                  lststr3.Add(PickColor(icolorid));

               // lststr3.Add("#00FF00");
                icolorid++;

            }

            foreach (gp_amount_schemename_Result dm in lstSchemeB)
            {
                List<object> x = new List<object>();
                lststr4.Add(Convert.ToDecimal(dm.amt));
                lststr5.Add(dm.parent_schemename_m.Trim());


            }


            iData.Add(lststr1);
            iData.Add(lststr2);
            iData.Add(lststr3);
            iData.Add(lststr4);
            iData.Add(lststr5);
            return Json(iData, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        //  [Authorize(Roles ="2")]
        public ActionResult DeskMaster()
        {
            return View(BindDeskMaster());
        }

        [HttpPost]
        public ActionResult DeskMaster(DeskModel obj)
        {
            desk_master objdeskmaster = new desk_master();
            objdeskmaster.desk_name = obj.desk_name;
            objdeskmaster.created_by = GetUidbyClaim(); ;
            objdeskmaster.created_date = DateTime.Now;
            objdeskmaster.updated_by = GetUidbyClaim(); ;
            objdeskmaster.updated_date = DateTime.Now;

            int flg = objDashboard.InsertDesk(objdeskmaster);

            if (flg > 0)
            {
                Success(CommonUtility.SucessMessage);

            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }

            return View(BindDeskMaster());
        }

        private DeskModel BindDeskMaster()
        {
            DeskModel obj = new DeskModel();
            obj.lstDeskMaster = objDashboard.GetDesk();
            return obj;
        }

        [HttpGet]
        public ActionResult DeskData()
        {
            //DashboardModel objDashboardM = BindDropdowns();
            //objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            //TempData["DeskTransTemp"] = objDashboardM;
            //return View(objDashboardM);

            DistrictAdminModel sm = new DistrictAdminModel();
            sm.ddlParentScheme = BindParentScheme(1);
            sm.ddlChildScheme1 = BlankSelectItem();
            sm.ddlChildScheme2 = BlankSelectItem();
            sm.ddlChildScheme3 = BlankSelectItem();
            sm.lstFinancialYear = BindFinancialYear();
            sm.lstDistrict = BindDistrict();
            sm.lstDeskTransData = objDashboard.GetTransDeskData();
            sm.lstImplementationAgency = BindImplementationAgency();

            return View(sm);

        }


        [HttpGet]
        public ActionResult UploadDocuments()
        {
            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskDocData = objDashboard.GetDeskDocumentsData();
            return View(objDashboardM);
        }

        [HttpPost]
        public ActionResult GetDocuments(int deskdocid)
        {
            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskDocFile = objDashboard.GetTransactionFile(deskdocid);
            return PartialView("FileDetails", objDashboardM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocuments(DashboardModel obj)
        {
            deskdocument objdeskdoc = new deskdocument();
            objdeskdoc.est_id = obj.est_id;
            // obj.committee_amount;
            objdeskdoc.desk_id = obj.desk_id;
            objdeskdoc.des_id = obj.des_id;
            objdeskdoc.fin_y_id = obj.fin_y_id;
            objdeskdoc.doc_title = obj.DocTitle;
            objdeskdoc.scheme_id = obj.scheme_id;
            objdeskdoc.sub_id = obj.sub_id;

            List<string> lstDoc = new List<string>();

            var InputFileName = "Not Uploaded";
            if (obj.UploadDoc != null)
            {
                foreach (var HPF in obj.UploadDoc)
                {
                    string str1 = DateTime.Today.ToString("yyyyMMdd");
                    string str2 = DateTime.Now.ToString("HH:mm:ss").Replace(":", "");
                    string str = str1 + str2;
                    //InputFileName = str + "_" + Path.GetFileName(((HttpPostedFileBase[])obj.UploadDoc)[0].FileName);
                    //var ServerSavePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["DeskDocFolder"].ToString()) + InputFileName);
                    //((HttpPostedFileBase[])obj.UploadDoc)[0].SaveAs(ServerSavePath);
                    InputFileName = str + "_" + Path.GetFileName(HPF.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["DeskDocFolder"].ToString()) + InputFileName);
                    HPF.SaveAs(ServerSavePath);
                    lstDoc.Add(InputFileName);
                }
            }
            objdeskdoc.doc_filename = InputFileName;

            int result = objDashboard.InsertDesk_Doc(objdeskdoc, lstDoc);
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskDocData = objDashboard.GetDeskDocumentsData();
            return View(objDashboardM);

        }

        [HttpPost]
        public ActionResult DeskData(DistrictAdminModel obj)
        {
            int result = 0; string sucessmsg = "";
            scheme_amount_allocation objdesktrans = new scheme_amount_allocation();
            objdesktrans.des_id = obj.des_id;
            objdesktrans.fin_y_id = obj.fin_y_id;
            objdesktrans.budgetary_provision_amt = obj.budgetary_provision_amt;
            objdesktrans.actual_allocation_amt = obj.actual_allocation_amt;

            objdesktrans.imp_agency_id = obj.imp_agency_id;
            objdesktrans.parent_scheme_id = obj.parent_scheme_id;
            objdesktrans.scheme_id_child1 = obj.scheme_id_child1;
            objdesktrans.scheme_id_child2 = obj.scheme_id_child2;
            objdesktrans.scheme_id_child3 = obj.scheme_id_child3;
            objdesktrans.created_by = GetUidbyClaim();
            objdesktrans.created_date = DateTime.Now;
            objdesktrans.updated_by = GetUidbyClaim();
            objdesktrans.updated_date = DateTime.Now;

            if (obj.tran_id > 0)
            {
                // result = objDashboard.UpdateDeskTrans(objdesktrans);
                sucessmsg = CommonUtility.EditMessage;
            }
            else
            {
                // result = objDashboard.InsertDeskTrans(objdesktrans);
                result = objDashboard.InsertSchemeAllotment(objdesktrans);
                sucessmsg = CommonUtility.SucessMessage;
            }

            obj.tran_id = 0;
            if (result > 0)
            {
                Success(sucessmsg);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }

            //DashboardModel objDashboardM = BindDropdowns();
            //objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();


            DistrictAdminModel sm = new DistrictAdminModel();
            sm.ddlParentScheme = BindParentScheme(1);
            sm.ddlChildScheme1 = BlankSelectItem();
            sm.ddlChildScheme2 = BlankSelectItem();
            sm.ddlChildScheme3 = BlankSelectItem();
            sm.lstFinancialYear = BindFinancialYear();
            sm.lstDistrict = BindDistrict();
            sm.lstDeskTransData = objDashboard.GetTransDeskData();
            sm.lstImplementationAgency = BindImplementationAgency();

            return View(sm);
        }
        [HttpGet]
        public ActionResult SubjectMaster()
        {
            return View(BindSubjectMaster());
        }

        [HttpPost]
        public ActionResult SubjectMaster(SubjectModel obj)
        {
            subject_master objsub = new subject_master();
            objsub.desk_id = obj.desk_id;
            objsub.sub_name = obj.sub_name;
            objsub.created_by = GetUidbyClaim();
            objsub.created_date = DateTime.Now;
            objsub.updated_by = GetUidbyClaim();
            objsub.updated_date = DateTime.Now;
            System.Threading.Thread.Sleep(1000);
            int flg = objDashboard.InsertSubject(objsub);

            if (flg > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }

            return View(BindSubjectMaster());
        }

        private SubjectModel BindSubjectMaster()
        {
            SubjectModel obj = new SubjectModel();
            obj.lstSubject = objDashboard.GetSubjectData();
            obj.lstDesk = BindDesk();
            return obj;
        }

        private SchemeModel BindSchemeMaster()
        {
            SchemeModel obj = new SchemeModel();
            obj.lstSubject = BindSubject();
            obj.lstDesk = BindDesk();
            obj.lstSchems = objDashboard.GetSchemeTransaction();
            TempData["SchemeList"] = obj;
            return obj;
        }

        [HttpGet]
        public ActionResult SchemeMaster()
        {

            return View(BindSchemeMaster());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SchemeMaster(SchemeModel objSM)
        {
            int flg = 0;
            dynamic showMessageString = string.Empty;
            scheme_master objsch = new scheme_master();
            objsch.scheme_id = objSM.scheme_id;
            objsch.scheme_name = objSM.scheme_name;
            objsch.scheme_description = Regex.Replace(objSM.scheme_description, @"\s*(?<capture><(?<markUp>\w+)>.*<\/\k<markUp>>)\s*", "${capture}", RegexOptions.Singleline);
            objsch.desk_id = objSM.desk_id;
            objsch.sub_id = objSM.sub_id;
            objsch.created_by = GetUidbyClaim();
            objsch.created_date = DateTime.Now;
            objsch.updated_by = GetUidbyClaim();
            objsch.updated_date = DateTime.Now;
            System.Threading.Thread.Sleep(1000);

            if (objSM.scheme_id > 0)
            {
                flg = objDashboard.UpdateSchems(objsch);
            }
            else
            {
                flg = objDashboard.InsertScheme(objsch);
            }

            if (flg > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }

            return View(BindSchemeMaster());

        }

        private DashboardModel BindDropdowns()
        {

            DashboardModel objDashboardM = new DashboardModel();
            objDashboardM.lstDesk = BindDesk();
            objDashboardM.lstDistrict = BindDistrict();
            objDashboardM.lstFinancialYear = BindFinancialYear();
            objDashboardM.lstScheme = BindScheme();
            objDashboardM.lstSubject = BindSubject();
            objDashboardM.lstEstimatipn = BindEstimation();
            // objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            return objDashboardM;

        }


        public ActionResult EditDeskTrans(int id)
        {
            // Edit Delete flag for preserve the Temp Data
            return View("DeskData", GetTempDeskTrans(id));
        }
        [HttpGet]
        public ActionResult EditSchems(int id)
        {
            // Edit Delete flag for preserve the Temp Data
            return View("SchemeMaster", GetScheme(id));
        }


        public ActionResult EditSubject(int id)
        {
            // Edit Delete flag for preserve the Temp Data
            return View("SubjectMaster", GetTempSubject(id));
        }

        public ActionResult DeletDeskTrans(int id)
        {
            int result = 0;
            DashboardModel obj = new DashboardModel();
            obj = (DashboardModel)TempData.Peek("DeskTransTemp");
            List<GetDeskTransactionData_Result> newtrans = obj.lstDeskTransData.Where(s => s.tran_id == id).ToList();
            deskdata_trans objdesktrans = new deskdata_trans();
            objdesktrans.budgetary_provision_amt = Convert.ToDecimal(newtrans[0].budgetary_provision_amt);
            objdesktrans.desk_id = Convert.ToInt32(newtrans[0].desk_id);
            objdesktrans.des_id = Convert.ToInt32(newtrans[0].des_id);
            objdesktrans.est_id = Convert.ToInt32(newtrans[0].est_id);
            objdesktrans.fin_y_id = Convert.ToInt32(newtrans[0].fin_y_id);
            objdesktrans.actual_allocation_amt = Convert.ToDecimal(newtrans[0].actual_allocation_amt);
            objdesktrans.scheme_id = Convert.ToInt32(newtrans[0].scheme_id);
            objdesktrans.sub_id = Convert.ToInt32(newtrans[0].sub_id);
            objdesktrans.tran_id = Convert.ToInt32(newtrans[0].tran_id);

            result = objDashboard.DeleteDeskTrans(objdesktrans);

            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            TempData["DeskTransTemp"] = objDashboardM;
            if (result > 0)
            {

                Success(CommonUtility.DeleteMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);

            }

            return View("DeskData", objDashboardM);
        }


        private DashboardModel GetTempDeskTrans(int id)
        {
            DashboardModel obj = new DashboardModel();
            if (TempData.ContainsKey("DeskTransTemp"))
            {
                obj = (DashboardModel)TempData.Peek("DeskTransTemp");
                List<GetDeskTransactionData_Result> newtrans = obj.lstDeskTransData.Where(s => s.tran_id == id).ToList();
                obj.budgetary_provision_amt = Convert.ToDecimal(newtrans[0].budgetary_provision_amt);
                obj.desk_id = Convert.ToInt32(newtrans[0].desk_id);
                obj.des_id = Convert.ToInt32(newtrans[0].des_id);
                obj.est_id = Convert.ToInt32(newtrans[0].est_id);
                obj.fin_y_id = Convert.ToInt32(newtrans[0].fin_y_id);
                obj.actual_allocation_amt = Convert.ToDecimal(newtrans[0].actual_allocation_amt);
                obj.scheme_id = Convert.ToInt32(newtrans[0].scheme_id);
                obj.sub_id = Convert.ToInt32(newtrans[0].sub_id);
                obj.tran_id = Convert.ToInt32(newtrans[0].tran_id);
            }

            return obj;
        }

        private SchemeModel GetScheme(int id)
        {

            SchemeModel obj = new SchemeModel();
            if (TempData.ContainsKey("SchemeList"))
            {
                obj = (SchemeModel)TempData.Peek("SchemeList");
                List<GetSchemeData_Result> newtrans = obj.lstSchems.Where(s => s.scheme_id == id).ToList();
                obj.desk_id = Convert.ToInt32(newtrans[0].desk_id);
                obj.scheme_description = Convert.ToString(newtrans[0].scheme_description);
                obj.scheme_id = Convert.ToInt32(newtrans[0].scheme_id);
                obj.scheme_name = Convert.ToString(newtrans[0].scheme_name);
                obj.sub_id = Convert.ToInt32(newtrans[0].sub_id);
            }

            return obj;
        }


        private SubjectModel GetTempSubject(int id)
        {
            SubjectModel obj = new SubjectModel();
            //if (TempData.ContainsKey("SubjectsTemp"))
            //{
            //    obj = (SubjectModel)TempData.Peek("SubjectsTemp");
            SubjectModel objs = new SubjectModel();
            objs = BindSubjectMaster();
            List<GetSubjectData_Result> lst = objs.lstSubject.Where(s => s.sub_id == id).ToList();
            obj.sub_id = lst[0].sub_id;
            obj.sub_name = lst[0].sub_name;
            obj.desk_id = lst[0].desk_id;
            obj.lstDesk = objs.lstDesk;
            obj.lstSubject = BindSubjectMaster().lstSubject;
            //}
            return obj;
        }

        public ActionResult GalleryMaster()
        {
            return View();
        }
        public ActionResult OrganizationStructureMaster()
        {
            return View();
        }

        private SchemeModel BindParentChildScheme()
        {
            SchemeModel sm = new SchemeModel();
            sm.ddlParentScheme = BindParentScheme(1);
            sm.ddlChildScheme1 = BlankSelectItem();
            sm.ddlChildScheme2 = BlankSelectItem();
            sm.ddlChildScheme3 = BlankSelectItem();
            sm.lstSchemeDesc = objDashboard.GetFilteredSchemeDesc(0, 0, 0, 0);
            return sm;
        }

        [HttpGet]
        public ActionResult SchemeDescriptionMaster()
        {
            return View(BindParentChildScheme());
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SchemeDescriptionMaster(SchemeModel sm)
        {
            int result = 0;
            scheme_desc_mapping sdm = new scheme_desc_mapping();
            sdm.parent_scheme_id = sm.parent_scheme_id;
            sdm.scheme_id_child1 = sm.scheme_id_child1;
            sdm.scheme_id_child2 = sm.scheme_id_child2;
            sdm.scheme_id_child3 = sm.scheme_id_child3;
            sdm.scheme_description_e = sm.scheme_description_e;
            sdm.scheme_description_m = sm.scheme_description_m;
            sdm.created_by = GetUidbyClaim();
            sdm.created_date = DateTime.Now;
            sdm.updated_by = GetUidbyClaim();
            sdm.updated_date = DateTime.Now;
            System.Threading.Thread.Sleep(1000);

            result = objDashboard.InsertSchemeDescMapping(sdm);
            if (result > 0)
            {

                Success(CommonUtility.DeleteMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);

            }

            return View(BindParentChildScheme());
        }

        [HttpGet]
        public ActionResult SechemeAmountAllotment()
        {
            SchemeAmtAllotment obj = new SchemeAmtAllotment();
            obj.ddlFinancialYear = BindFinancialYear();
            obj.ddlParentScheme = BindParentScheme(1);
            obj.lstDistrict = DistrictwithAmt();
            TempData["SAA"] = obj;

            return View(obj);
        }

        [HttpPost]
        public ActionResult SechemeAmountAllotment(SchemeAmtAllotment SAA)
        {
            SchemeAmtAllotment obj = new SchemeAmtAllotment();
            obj.ddlFinancialYear = BindFinancialYear();
            obj.ddlParentScheme = BindParentScheme(1);



            return View(obj);
        }

        [HttpPost]
        public ActionResult SaveparentSchemeAmt(SchemeAmtAllotment SAA)
        {
            SchemeAmtAllotment obj = new SchemeAmtAllotment();
            obj.ddlFinancialYear = BindFinancialYear();
            obj.ddlParentScheme = BindParentScheme(1);
            return View("SechemeAmountAllotment", obj);
        }

        [HttpPost]
        public ActionResult DisctrictSchemeAmt(SchemeAmtAllotment lst)
        {
            SchemeAmtAllotment obj111 = (SchemeAmtAllotment)TempData["SAA"];
            SchemeAmtAllotment obj = new SchemeAmtAllotment();
            obj.ddlFinancialYear = BindFinancialYear();
            obj.ddlParentScheme = BindParentScheme(1);
            return View("SechemeAmountAllotment", obj);
        }

        [HttpPost]
        public ActionResult UploadGR(GRModel obj)
        {
            int result = 0;
            grdetail grd = new grdetail();
            grd.keywords_e = obj.keywords_e;
            grd.keywords_m = obj.keywords_m;
            grd.unique_code_e = obj.unique_code_e;
            grd.unique_code_m = obj.unique_code_m;
            grd.gr_date = obj.gr_date;
            grd.isactive = obj.isactive;
            grd.created_by = GetUidbyClaim();
            grd.created_date = DateTime.Now;
            grd.updated_by = GetUidbyClaim();
            grd.updated_date = DateTime.Now;
            grd.gr_file = SaveFileinFolder(obj.GrFile, ConfigurationManager.AppSettings["GRFolder"].ToString(), obj.gr_id)[0];

            result = objDashboard.InsertGR(grd);
            if (result > 0)
            {

                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);

            }
            GRModel grm = new GRModel();

            return View(grm);
        }

        [HttpGet]
        public ActionResult UploadGR()
        {
            GRModel grm = new GRModel();

            return View(grm);
        }

        public ActionResult LatestNews()
        {
          
            return View();
        }

        




    }
}