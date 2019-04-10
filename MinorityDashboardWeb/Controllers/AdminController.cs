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
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Unity;

namespace MinorityDashboard.Web.Controllers
{
    [CustomAuthorize(Roles = "1")]
    [CustomExceptionFilter]
    public class AdminController : BaseController
    {
        // GET: Admin
        //  IDashboard objDashboard; // = new Dashboard();
        //  IDistrictAdmin objDistrictAdmin;
        // IUnityContainer unitycontainer = new UnityContainer();


        private readonly IDashboard objDashboard;
        private readonly IDistrictAdmin objDistrictAdmin;

        public AdminController(IDashboard repository1, IDistrictAdmin repository2)
        {
            objDashboard = repository1;
            objDistrictAdmin = repository2;
            //unitycontainer.RegisterType<IDashboard, Dashboard>();
            //unitycontainer.RegisterType<IDistrictAdmin, DistrictAdmin>();
            //objDashboard = unitycontainer.Resolve<Dashboard>();
            //objDistrictAdmin = unitycontainer.Resolve<DistrictAdmin>();

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

        #region Citizen Charter

       
        [HttpGet]
        public ActionResult CitizenCharterMaster()
        {
            CitizenCharterModel CCM = new CitizenCharterModel();
            CCM.lstCitizenCharterList = objDashboard.GetCitizenCharter();
            TempData["CitizenCharterList"] = CCM;
            return View(CCM);
          
        }
        [HttpPost]
        public ActionResult CitizenCharterMaster(CitizenCharterModel CCM)
        {
            int result = 0;
            citizen_charter obj = new citizen_charter();
            obj.name_e = CCM.name_e;
            obj.name_m = CCM.name_m;
            obj.isactive = CCM.isactive; //obj.isactive;
            obj.updated_by = GetUidbyClaim();
            obj.updated_date = DateTime.Now;
            if (CCM.File.Length > 0 && CCM.File[0] != null)
            {
                obj.isactive = CCM.isactive;
                obj.file_path = SaveFileinFolder(CCM.File, ConfigurationManager.AppSettings["CitizenCharterFolder"].ToString(), CCM.cc_id)[0];
            }
            if (CCM.cc_id> 0)
            {
                obj.file_path = obj.file_path == null ? TempData["CitizenCharterFolder"].ToString() : obj.file_path;
                obj.isactive = obj.file_path != "" ? true : false;
                obj.cc_id = CCM.cc_id;
                result = objDashboard.UpdateCitizenCharter(obj);
            }
            else
            {
                obj.created_by = GetUidbyClaim();
                obj.created_date = DateTime.Now;
                result = objDashboard.InsertCitizenCharter(obj);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
         
            return RedirectToAction("CitizenCharterMaster");
        }

        public ActionResult EditCitizenCharter(int id)
        {
            CitizenCharterModel obj = new CitizenCharterModel();
            if (TempData["CitizenCharterList"] != null)
            {
                obj = (CitizenCharterModel)TempData["CitizenCharterList"];
            }
            List<citizen_charter> lst = obj.lstCitizenCharterList.Where(s => s.cc_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.file_path = lst[0].file_path;
            obj.name_e = lst[0].name_e;
            obj.name_m = lst[0].name_m;
            obj.cc_id = lst[0].cc_id;         
            TempData["CCFileName"] = lst[0].file_path;
            TempData["CitizenCharterList"] = obj;
            return View("CitizenCharterMaster", obj);
        }

        #endregion

        [HttpGet]
        public ActionResult AdvertisementMaster()
        {
            AdvertisementModel AM = new AdvertisementModel();
            AM.lstAdvertisementList = objDashboard.GetAdvertisement();
            TempData["AdvertiseFileName"] = AM;
            return View(AM);
         
        }
        [HttpPost]
        public ActionResult AdvertisementMaster(AdvertisementModel AM)
        {
            int result = 0;
            advertisement_master obj = new advertisement_master();
            obj.adv_title_e = AM.adv_title_e;
            obj.adv_title_m = AM.adv_title_m;
            obj.isactive = AM.isactive;        
            obj.updated_by = GetUidbyClaim();
            obj.updated_date = DateTime.Now;
            obj.adv_id = AM.adv_id;
            if (AM.File.Length > 0 && AM.File[0] != null)
            {
                obj.file_name = SaveFileinFolder(AM.File, ConfigurationManager.AppSettings["AdvertisementFileName"].ToString(), obj.adv_id)[0];
            }

            if (obj.adv_id > 0)
            {
                obj.file_name = obj.file_name == null ? TempData["AdvertiseFileName"].ToString() : obj.file_name;           
                result = objDashboard.UpdateAdvertisement(obj);
            }
            else
            {
                obj.created_by = GetUidbyClaim();
                obj.created_date = DateTime.Now;
                result = objDashboard.InsertAdvertisement(obj);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            return RedirectToAction("AdvertisementMaster");
        }

        public ActionResult EditAdvertisement(int id)
        {

            AdvertisementModel obj = new AdvertisementModel();
            if (TempData["AdvertiseFileName"] != null)
            {
                obj = (AdvertisementModel)TempData["AdvertiseFileName"];
            }
            List<advertisement_master> lst = obj.lstAdvertisementList.Where(s => s.adv_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.adv_title_e = lst[0].adv_title_e;
            obj.adv_title_m = lst[0].adv_title_m;
          
            obj.adv_id = lst[0].adv_id;
            obj.isactive = lst[0].isactive;
            obj.file_name = lst[0].file_name;
          
           
            TempData["AdvertiseFileName"] = obj;
            return View("AdvertisementMaster", obj);

        }


        #region Key Person



        [HttpGet]
        public ActionResult KeyPerson()
        {
            KeyPersonModel KPM = new KeyPersonModel();
            KPM.lstKeyPersonList = objDashboard.GetKeyPerson();
            TempData["KeyPersonList"] = KPM;
            return View(KPM);
        }
        [HttpPost]
        public ActionResult KeyPerson(KeyPersonModel KPM)
        {
            int result = 0;
            keyperson_master obj = new keyperson_master();
            obj.designation_e = KPM.designation_e;
            obj.designation_m = KPM.designation_m;
            obj.display_order = KPM.display_order;
            obj.isactive = KPM.isactive;           
            obj.person_image = KPM.person_image;
            obj.person_name_e = KPM.person_name_e;
            obj.person_name_m = KPM.person_name_m;
            obj.updated_by = GetUidbyClaim();
            obj.updated_date = DateTime.Now;
            obj.keyperson_id = KPM.keyperson_id;
            if (KPM.File.Length > 0 && KPM.File[0] != null)
            {

                obj.person_image = SaveFileinFolder(KPM.File, ConfigurationManager.AppSettings["KeyPersonFileName"].ToString(), obj.keyperson_id)[0];
            }

            if (obj.keyperson_id > 0)
            {
                obj.person_image = obj.person_image == null ? TempData["KeyPersonFileName"].ToString() : obj.person_image;
              //  obj.isactive = obj.person_image != "" ? true : false;             
                result = objDashboard.UpdateKeyPerson(obj);
            }
            else
            {
                obj.created_by = GetUidbyClaim();
                obj.created_date = DateTime.Now;
                result = objDashboard.InsertKeyPerson(obj);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            return RedirectToAction("KeyPerson");
        }

        public ActionResult EditKeyPerson(int id)
        {

            KeyPersonModel obj = new KeyPersonModel();
            if (TempData["KeyPersonList"] != null)
            {
                obj = (KeyPersonModel)TempData["KeyPersonList"];
            }
            List<keyperson_master> lst = obj.lstKeyPersonList.Where(s => s.keyperson_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.designation_e = lst[0].designation_e;
            obj.designation_m = lst[0].designation_m;
            obj.display_order = Convert.ToInt32(lst[0].display_order);
            obj.keyperson_id = lst[0].keyperson_id;
            obj.person_image = lst[0].person_image;
            obj.person_name_e = lst[0].person_name_e;
            obj.person_name_m = lst[0].person_name_m;          
            TempData["KeyPersonFileName"] = lst[0].person_image;
            TempData["KeyPersonList"] = obj;
            return View("KeyPerson", obj);

        }

        #endregion

        #region Front Slider

        

        [HttpGet]
        public ActionResult Slider()
        {
            SliderModel SM = new SliderModel();
            SM.lstFrontSlider = objDashboard.GetFrontSlider();
            TempData["FrontSliderList"] = SM;
            return View(SM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Slider(SliderModel SM)
        {
            int result = 0;
            front_slider obj = new front_slider();
            obj.isactive = SM.isactive;
            obj.slide_order = SM.slide_order;
            obj.title_e = SM.title_e;
            obj.title_m = SM.title_m;
            obj.slider_id = SM.slider_id;           
            obj.updated_by = GetUidbyClaim();
            obj.updated_date = DateTime.Now;
           
            if (SM.File.Length > 0 && SM.File[0] != null)
            {
                obj.slider_img = SaveFileinFolder(SM.File, ConfigurationManager.AppSettings["SliderFileName"].ToString(), obj.slider_id)[0];
            }

            if (obj.slider_id > 0)
            {
                obj.slider_img = obj.slider_img == null ? TempData["FrontSliderList"].ToString() : obj.slider_img;         
                result = objDashboard.UpdateFrontSlider(obj);
            }
            else
            {
                obj.created_by = GetUidbyClaim();
                obj.created_date = DateTime.Now;
                result = objDashboard.InsertFrontSlider(obj);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            return RedirectToAction("Slider");
        }

        public ActionResult EditSlider(int id)
        {
            SliderModel obj = new SliderModel();
            if (TempData["FrontSliderList"] != null)
            {
                obj = (SliderModel)TempData["FrontSliderList"];
            }
            List<front_slider> lst = obj.lstFrontSlider.Where(s => s.slider_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.slider_id = lst[0].slider_id;
            obj.title_e = lst[0].title_e;
            obj.title_m = lst[0].title_m;
            obj.slide_order = Convert.ToInt32(lst[0].slide_order);
            obj.slider_img = lst[0].slider_img;
                   
            TempData["FrontSliderList"] = obj;
            return View("Slider", obj);
        }

        #endregion


        #region Scheme Description

        private SchemeModel BindParentChildScheme()
        {
            SchemeModel sm = new SchemeModel();
            sm.ddlParentScheme = BindParentScheme(1);
            sm.ddlChildScheme1 = BlankSelectItem();
            sm.ddlChildScheme2 = BlankSelectItem();
            sm.ddlChildScheme3 = BlankSelectItem();
            sm.lstSchemeDesc = objDashboard.GetFilteredSchemeDesc(0, 0, 0, 0);
            TempData["SchemeDescList"] = sm;

            return sm;
        }

        [HttpGet]
        public ActionResult SchemeDescriptionMaster()
        {
            return View(BindParentChildScheme());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SchemeDescriptionMaster(SchemeModel sm)
        {
            int result = 0;

            int check_scdesc = objDashboard.CheckSchemeDescription(sm.parent_scheme_id, sm.scheme_id_child1, sm.scheme_id_child2, sm.scheme_id_child3);


            if (check_scdesc > 0)
            {
                Success("Scheme description already exists. you can only update the scheme.");
            }
            else
            {

                scheme_desc_mapping sdm = new scheme_desc_mapping();
                sdm.parent_scheme_id = sm.parent_scheme_id;
                sdm.scheme_id_child1 = sm.scheme_id_child1;
                sdm.scheme_id_child2 = sm.scheme_id_child2;
                sdm.scheme_id_child3 = sm.scheme_id_child3;
                sdm.scheme_description_e = sm.scheme_description_e;
                sdm.scheme_description_m = sm.scheme_description_m;
                sdm.isactive = sm.isactive;

                sdm.updated_by = GetUidbyClaim();
                sdm.updated_date = DateTime.Now;
                System.Threading.Thread.Sleep(1000);

                if (sm.scheme_des_id > 0)
                {
                    sdm.scheme_des_id = sm.scheme_des_id;
                    result = objDashboard.UpdateDeleteSchemDesc(sdm);
                }
                else
                {
                    sdm.created_by = GetUidbyClaim();
                    sdm.created_date = DateTime.Now;
                    result = objDashboard.InsertSchemeDescMapping(sdm);
                }
                if (result > 0)
                {

                    Success(CommonUtility.DeleteMessage);
                }
                else
                {
                    Danger(CommonUtility.ErrorMessage);

                }
            }


            return RedirectToAction("SchemeDescriptionMaster");

            //return View(BindParentChildScheme());
        }

        public ActionResult EditSchemeDesc(int id)
        {
            SchemeModel obj = new SchemeModel();
            if (TempData["SchemeDescList"] != null)
            {
                obj = (SchemeModel)TempData["SchemeDescList"];
            }
            List<GetSchemeDesc_Result> lst = obj.lstSchemeDesc.Where(s => s.scheme_des_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.parent_scheme_id = Convert.ToInt32(lst[0].parent_scheme_id);
            obj.scheme_id_child1 = Convert.ToInt32(lst[0].scheme_id_child1);
            obj.scheme_id_child2 = Convert.ToInt32(lst[0].scheme_id_child2);
            obj.scheme_id_child3 = Convert.ToInt32(lst[0].scheme_id_child3);
            obj.scheme_description_e = lst[0].scheme_description_e;
            obj.scheme_description_m = lst[0].scheme_description_m;
            obj.isactive = Convert.ToBoolean(lst[0].isactive);

            obj.scheme_des_id = lst[0].scheme_des_id;

            obj.ddlChildScheme1 = BindChildScheme1(1, obj.parent_scheme_id);

            obj.ddlChildScheme2 = obj.scheme_id_child1 == 0 ? BlankSelectItem() : BindChildScheme2(1, obj.scheme_id_child1);

            if (obj.ddlChildScheme2.Count < 1)
                obj.ddlChildScheme2.Add(new SelectListItem() { Value = "0", Text = "Select" });

            obj.ddlChildScheme3 = obj.scheme_id_child2 == 0 ? BlankSelectItem() : BindChildScheme3(1, obj.scheme_id_child2);

            if (obj.ddlChildScheme3.Count < 1)
                obj.ddlChildScheme3.Add(new SelectListItem() { Value = "0", Text = "Select" });

            TempData["SchemeDescList"] = obj;
            return View("SchemeDescriptionMaster", obj);
        }

        #endregion


        #region Scheme Amount Allocation District wise

        [HttpGet]
        [NonAction]
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
        [NonAction]
        public ActionResult SechemeAmountAllotment(SchemeAmtAllotment SAA)
        {
            SchemeAmtAllotment obj = new SchemeAmtAllotment();
            obj.ddlFinancialYear = BindFinancialYear();
            obj.ddlParentScheme = BindParentScheme(1);



            return View(obj);
        }

        #endregion


        #region Desk Data Code 

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
            //sm.lstDeskTransData = objDashboard.GetTransDeskData();
            sm.lstSchemeAmountAllocation = objDashboard.GetSchemeAmountAllocation();
            sm.lstImplementationAgency = BindImplementationAgency();
            TempData["DeskTransTemp"] = sm;

            return View(sm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeskData(DistrictAdminModel obj)
        {
            int result = 0; string sucessmsg = "";
            if (ModelState.IsValid)
            {
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

                objdesktrans.updated_by = GetUidbyClaim();
                objdesktrans.updated_date = DateTime.Now;

                if (obj.id > 0)
                {
                    objdesktrans.id = obj.id;
                    result = objDashboard.UpdateSchemeAllotment(objdesktrans);
                    sucessmsg = CommonUtility.EditMessage;
                }
                else
                {
                    objdesktrans.created_by = GetUidbyClaim();
                    objdesktrans.created_date = DateTime.Now;
                    // result = objDashboard.InsertDeskTrans(objdesktrans);
                    result = objDashboard.InsertSchemeAllotment(objdesktrans);
                    sucessmsg = CommonUtility.SucessMessage;
                }

                //obj.tran_id = 0;
                if (result > 0)
                {
                    Success(sucessmsg);
                }
                else
                {
                    Danger(CommonUtility.ErrorMessage);
                }
            }
            //DashboardModel objDashboardM = BindDropdowns();
            //objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();


            //DistrictAdminModel sm = new DistrictAdminModel();
            //sm.ddlParentScheme = BindParentScheme(1);
            //sm.ddlChildScheme1 = BlankSelectItem();
            //sm.ddlChildScheme2 = BlankSelectItem();
            //sm.ddlChildScheme3 = BlankSelectItem();
            //sm.lstFinancialYear = BindFinancialYear();
            //sm.lstDistrict = BindDistrict();
            //sm.lstSchemeAmountAllocation = objDashboard.GetSchemeAmountAllocation();
            //sm.lstImplementationAgency = BindImplementationAgency();
            //return View(sm);

            return RedirectToAction("DeskData");
        }


        public ActionResult EditDeskTrans(int id)
        {
            // Edit Delete flag for preserve the Temp Data
            return View("DeskData", GetTempDeskTrans(id));
        }

        private DistrictAdminModel GetTempDeskTrans(int id)
        {
            //DashboardModel obj = new DashboardModel();
            //if (TempData.ContainsKey("DeskTransTemp"))
            //{
            //    obj = (DashboardModel)TempData.Peek("DeskTransTemp");
            //    List<GetDeskTransactionData_Result> newtrans = obj.lstDeskTransData.Where(s => s.tran_id == id).ToList();
            //    obj.budgetary_provision_amt = Convert.ToDecimal(newtrans[0].budgetary_provision_amt);
            //    obj.desk_id = Convert.ToInt32(newtrans[0].desk_id);
            //    obj.des_id = Convert.ToInt32(newtrans[0].des_id);
            //    obj.est_id = Convert.ToInt32(newtrans[0].est_id);
            //    obj.fin_y_id = Convert.ToInt32(newtrans[0].fin_y_id);
            //    obj.actual_allocation_amt = Convert.ToDecimal(newtrans[0].actual_allocation_amt);
            //    obj.scheme_id = Convert.ToInt32(newtrans[0].scheme_id);
            //    obj.sub_id = Convert.ToInt32(newtrans[0].sub_id);
            //    obj.tran_id = Convert.ToInt32(newtrans[0].tran_id);
            //}

            //return obj;


            DistrictAdminModel obj = new DistrictAdminModel();
            if (TempData.ContainsKey("DeskTransTemp"))
            {
                obj = (DistrictAdminModel)TempData.Peek("DeskTransTemp");
                List<GetSchemeAmountAllocation_Result> newtrans = obj.lstSchemeAmountAllocation.Where(s => s.id == id).ToList();

                obj.id = Convert.ToInt32(newtrans[0].id);
                obj.des_id = Convert.ToInt32(newtrans[0].des_id);
                obj.fin_y_id = Convert.ToInt32(newtrans[0].fin_y_id);
                obj.parent_scheme_id = Convert.ToInt32(newtrans[0].parent_scheme_id);

                obj.scheme_id_child1 = Convert.ToInt32(newtrans[0].scheme_id_child1);
                obj.scheme_id_child2 = Convert.ToInt32(newtrans[0].scheme_id_child2);
                obj.scheme_id_child3 = Convert.ToInt32(newtrans[0].scheme_id_child3);

                obj.budgetary_provision_amt = Convert.ToDecimal(newtrans[0].budgetary_provision_amt);
                obj.actual_allocation_amt = Convert.ToDecimal(newtrans[0].actual_allocation_amt);
                obj.imp_agency_id = Convert.ToInt32(newtrans[0].imp_agency_id);

                obj.ddlChildScheme1 = BindChildScheme1(1, obj.parent_scheme_id);

                obj.ddlChildScheme2 = obj.scheme_id_child1 == 0 ? BlankSelectItem() : BindChildScheme2(1, obj.scheme_id_child1);

                if (obj.ddlChildScheme2.Count < 1)
                    obj.ddlChildScheme2.Add(new SelectListItem() { Value = "0", Text = "Select" });

                obj.ddlChildScheme3 = obj.scheme_id_child2 == 0 ? BlankSelectItem() : BindChildScheme3(1, obj.scheme_id_child2);

                if (obj.ddlChildScheme3.Count < 1)
                    obj.ddlChildScheme3.Add(new SelectListItem() { Value = "0", Text = "Select" });


                obj.lstImplementationAgency = BindImplementationAgency();

                TempData["DeskTransTemp"] = obj;

            }

            return obj;

        }

        #endregion

        #region latest news code
        [HttpGet]
        public ActionResult LatestNews()
        {
            LatestNews obj = new LatestNews();
            obj.news_date = DateTime.Now;
            obj.lstLatestNews = objDashboard.GetLatestNewsList();

            TempData["NewsList"] = obj;

            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult LatestNews(LatestNews LN)
        {
            int result = 0;
            latest_news obj = new latest_news();
            obj.isactive = LN.isactive;
            obj.news_date = LN.news_date;
            obj.news_description_e = LN.news_description_e;
            obj.news_description_m = LN.news_description_m;
            obj.news_e = LN.latest_news_e;
            obj.news_m = LN.latest_news_m;
            obj.updated_by = GetUidbyClaim();
            obj.updated_date = DateTime.Now;

            if (LN.latest_news_id > 0)
            {
                obj.latest_news_id = LN.latest_news_id;
                result = objDashboard.UpdateDeleteLatestNews(obj);
            }
            else
            {
                obj.created_by = GetUidbyClaim();
                obj.created_date = DateTime.Now;
                result = objDashboard.InsertLatestNews(obj);
            }

            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            //LatestNews LNObj = new LatestNews();
            //LNObj.lstLatestNews = objDashboard.GetLatestNewsList();
            //return View(LNObj);

            return RedirectToAction("LatestNews");
        }

        public ActionResult DeleteLatestNews(int id)
        {
            int result = 0;
            LatestNews obj = new LatestNews();
            if (TempData["NewsList"] != null)
            {
                obj = (LatestNews)TempData["NewsList"];
            }
            List<latest_news> latestnews = obj.lstLatestNews.Where(s => s.latest_news_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            latestnews[0].isactive = false;
            result = objDashboard.UpdateDeleteLatestNews(latestnews[0]);

            if (result > 0)
            {
                Success(CommonUtility.DeleteMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            //LatestNews LNObj = new LatestNews();
            //LNObj.lstLatestNews = objDashboard.GetLatestNewsList();

            //return View("LatestNews", LNObj);

            return RedirectToAction("LatestNews");
        }

        public ActionResult EditLatestNews(int id)
        {
            LatestNews obj = new LatestNews();
            if (TempData["NewsList"] != null)
            {
                obj = (LatestNews)TempData["NewsList"];
            }
            List<latest_news> latestnews = obj.lstLatestNews.Where(s => s.latest_news_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = latestnews[0].isactive;
            obj.latest_news_e = latestnews[0].news_e;
            obj.latest_news_m = latestnews[0].news_m;
            obj.news_description_e = latestnews[0].news_description_e;
            obj.news_description_m = latestnews[0].news_description_m;
            obj.latest_news_id = latestnews[0].latest_news_id;
            TempData["NewsList"] = obj;
            return View("LatestNews", obj);
        }
        #endregion

        #region Gallery Code

        [HttpGet]
        public ActionResult GalleryMaster()
        {
            GalleryModel grm = new GalleryModel();
            grm.posted_date = DateTime.Now;
            grm.lstGalleryList = objDashboard.GetGalleryList();
            TempData["GalleryList"] = grm;
            return View(grm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GalleryMaster(GalleryModel obj)
        {
            int result = 0;
            gallery_master grd = new gallery_master();
            grd.title_e = obj.title_e;
            grd.title_m = obj.title_m;
            grd.file_extension = "no use";
            grd.posted_date = obj.posted_date;
            grd.isactive = false; //obj.isactive;
            grd.updated_by = GetUidbyClaim();
            grd.updated_date = DateTime.Now;
            if (obj.File.Length > 0 && obj.File[0] != null)
            {
                grd.isactive = obj.isactive;
                grd.file_name = SaveFileinFolder(obj.File, ConfigurationManager.AppSettings["GalleryFolder"].ToString(), obj.gallery_id)[0];
            }


            if (obj.gallery_id > 0)
            {
                grd.file_name = grd.file_name == null ? TempData["GalleryFileName"].ToString() : grd.file_name;
                grd.isactive = grd.file_name != "" ? true : false;
                grd.gallery_id = obj.gallery_id;
                result = objDashboard.UpdateDeleteGallery(grd);
            }
            else
            {
                grd.created_by = GetUidbyClaim();
                grd.created_date = DateTime.Now;
                result = objDashboard.InsertGallery(grd);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            //GalleryModel grm = new GalleryModel();
            //grm.lstGalleryList = objDashboard.GetGalleryList();
            //grm.posted_date = DateTime.Now;
            //TempData["GalleryList"] = grm;
            //return View(grm);

            return RedirectToAction("GalleryMaster");
        }

        public ActionResult EditGallery(int id)
        {
            GalleryModel obj = new GalleryModel();
            if (TempData["GalleryList"] != null)
            {
                obj = (GalleryModel)TempData["GalleryList"];
            }
            List<gallery_master> lst = obj.lstGalleryList.Where(s => s.gallery_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.posted_date = Convert.ToDateTime(lst[0].posted_date);
            obj.title_e = lst[0].title_e;
            obj.title_m = lst[0].title_m;
            obj.gallery_id = lst[0].gallery_id;
            obj.file_name = lst[0].file_name;
            obj.file_extension = lst[0].file_extension;
            TempData["GalleryFileName"] = lst[0].file_name;
            TempData["GalleryList"] = obj;
            return View("GalleryMaster", obj);
        }
        #endregion

        #region Organization Structure Code

        [HttpGet]
        public ActionResult OrganizationStructureMaster()
        {
            OrganizationStructureModel osm = new OrganizationStructureModel();
            osm.lstOrgStructure = objDashboard.GetOrgList();
            osm.lstReportingManager = BindReporingmanagerOrg();
            TempData["OrgStructureList"] = osm;
            return View(osm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrganizationStructureMaster(OrganizationStructureModel OSM)
        {
            if (ModelState.IsValid)
            {
                int result = 0;
                org_structure obj = new org_structure();
                obj.designation = OSM.designation;
                obj.name = OSM.name;
                obj.reporting_manager = OSM.reporting_manager;
                obj.reporting_manager_name = OSM.reporting_manager_name;
                obj.isactive = OSM.isactive;
                obj.updated_by = GetUidbyClaim();
                obj.updated_date = DateTime.Now;

                if (OSM.employee_id > 0)
                {
                    obj.employee_id = OSM.employee_id;
                    result = objDashboard.UpdateDeleteOrgStructure(obj);
                }
                else
                {
                    obj.created_by = GetUidbyClaim();
                    obj.created_date = DateTime.Now;
                    result = objDashboard.InsertOrgStructure(obj);
                }
                if (result > 0)
                {
                    Success(CommonUtility.SucessMessage);
                }
                else
                {
                    Danger(CommonUtility.ErrorMessage);
                }
            }
            return RedirectToAction("OrganizationStructureMaster");
        }

        public ActionResult EditOrgStructure(int id)
        {
            OrganizationStructureModel obj = new OrganizationStructureModel();
            if (TempData["OrgStructureList"] != null)
            {
                obj = (OrganizationStructureModel)TempData["OrgStructureList"];

                List<org_structure> lst = obj.lstOrgStructure.Where(s => s.employee_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
                obj.isactive = Convert.ToBoolean(lst[0].isactive);
                obj.designation = lst[0].designation;
                obj.employee_id = lst[0].employee_id;
                obj.name = lst[0].name;
                obj.reporting_manager = Convert.ToInt32(lst[0].reporting_manager);
                obj.reporting_manager_name = lst[0].reporting_manager_name;


                TempData["OrgStructureList"] = obj;
                return View("OrganizationStructureMaster", obj);
            }
            else
            {
                return RedirectToAction("OrganizationStructureMaster");
            }
        }




        #endregion

        #region GR Upload Code
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            grd.updated_by = GetUidbyClaim();
            grd.updated_date = DateTime.Now;

            if (obj.GrFile.Length > 0)
            {
                grd.gr_file = SaveFileinFolder(obj.GrFile, ConfigurationManager.AppSettings["GRFolder"].ToString(), obj.gr_id)[0];
            }

            if (obj.gr_id > 0)
            {
                grd.gr_file = grd.gr_file == null ? TempData["GRFileName"].ToString() : grd.gr_file;
                grd.gr_id = obj.gr_id;
                result = objDashboard.UpdateDeleteGR(grd);
            }
            else
            {
                grd.created_by = GetUidbyClaim();
                grd.created_date = DateTime.Now;
                result = objDashboard.InsertGR(grd);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            //GRModel grm = new GRModel();
            //grm.lstGRList = objDashboard.GetGRList();
            //return View(grm);

            return RedirectToAction("UploadGR");
        }

        [HttpGet]
        public ActionResult UploadGR()
        {
            GRModel grm = new GRModel();
            grm.gr_date = DateTime.Now;

            grm.lstGRList = objDashboard.GetGRList();

            TempData["GRList"] = grm;

            return View(grm);
        }

        public ActionResult EditGR(int id)
        {
            GRModel obj = new GRModel();
            if (TempData["GRList"] != null)
            {
                obj = (GRModel)TempData["GRList"];
            }
            List<grdetail> lst = obj.lstGRList.Where(s => s.gr_id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = lst[0].isactive;
            obj.gr_date = lst[0].gr_date;
            obj.keywords_e = lst[0].keywords_e;
            obj.keywords_m = lst[0].keywords_m;
            obj.unique_code_e = lst[0].unique_code_e;
            obj.unique_code_m = lst[0].unique_code_m;
            obj.gr_id = lst[0].gr_id;
            TempData["GRFileName"] = lst[0].gr_file;
            TempData["GRList"] = obj;
            return View("UploadGR", obj);
        }

        #endregion



    }
}