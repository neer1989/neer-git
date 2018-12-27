using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboardWeb;
using MinorityDashboardWeb.Models;
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

        IUnityContainer unitycontainer = new UnityContainer();


        public AdminController()
        {
            unitycontainer.RegisterType<IDashboard, Dashboard>();
            objDashboard = unitycontainer.Resolve<Dashboard>();

        }


        public ActionResult Index()
        {
            //  Success(string.Format("<b>{0}</b> was successfully added to the database.", "neer..."), true);

            return View();
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
            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            TempData["DeskTransTemp"] = objDashboardM;
            return View(objDashboardM);
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
        public ActionResult DeskData(DashboardModel obj)
        {
            int result = 0; string sucessmsg = "";
            deskdata_trans objdesktrans = new deskdata_trans();
            objdesktrans.est_id = obj.est_id;
            objdesktrans.desk_id = obj.desk_id;
            objdesktrans.des_id = obj.des_id;
            objdesktrans.fin_y_id = obj.fin_y_id;
            objdesktrans.budgetary_provision_amt = obj.budgetary_provision_amt;
            objdesktrans.actual_allocation_amt = obj.actual_allocation_amt;
            objdesktrans.actual_expenditure_amt = obj.actual_expenditure_amt;
            objdesktrans.actual_remaining_amt = obj.actual_remaining_amt;
            objdesktrans.scheme_id = obj.scheme_id;
            objdesktrans.sub_id = obj.sub_id;
            objdesktrans.tran_id = obj.tran_id;

            if (obj.tran_id > 0)
            {
                result = objDashboard.UpdateDeskTrans(objdesktrans);
                sucessmsg = CommonUtility.EditMessage;
            }
            else
            {
                result = objDashboard.InsertDeskTrans(objdesktrans);
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

            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            return View(objDashboardM);
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


    }
}