using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using SampleApp.Helper;
using SampleApp.Logic.Contracts;
using SampleApp.Model.Models;
using SampleApp.Models;
using SampleApp.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SampleApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/
        public static readonly string Key = ConfigurationManager.AppSettings["API-KEY"].ToString();
        public readonly IAdminLogic adminLogic;

        public AdminController(IAdminLogic _adminLogic)
        {
            adminLogic = _adminLogic;
        }

        public AdminController()
        {
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
             var emailid = Session["UserId"].ToString();
            //getting user id using email
            var obj=adminLogic.GetuserIdbyemail(emailid);


            Session["UserId"] = obj.Id;
            return View();
        }

        public ActionResult EditProfile()
        {
            ProfileViewModel VM = new ProfileViewModel();
            try
            {
             
                var UserID = Session["UserId"].ToString();

                AspNetUsers obj = adminLogic.GetUserDetailsById(UserID);

                VM.FirstName = obj.FirstName;
                VM.LastName = obj.LastName;
                VM.Password = obj.PasswordHash;
                VM.Email = obj.Email;
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(VM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ProfileViewModel VM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
               
                var FirstName = VM.FirstName;
                var LastName = VM.LastName;
                var Email = VM.Email;
                var Password = "";
                var UserId = Session["UserId"].ToString();
                if (VM.Password != null)
                {
                    Password = VM.Password;
                   
                    //generate hashpassword
                    Password = Crypto.HashPassword(Password);
                }
                else
                {
                    //our old password
                    Password = VM.OldPassword;
                }
                bool result = adminLogic.EditProfile(UserId, FirstName, LastName, Password);

                if (result == true)
                {
                    TempData["msg"] = "Profile has been updated. ";
                }
                else
                {
                    TempData["msg"] = "Profile has not been updated. ";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("EditProfile");
        }

        public ActionResult GetCode(string AppId)
        {
            RegisterViewModel VM = new RegisterViewModel();
            try
            {

                Campaign obj = adminLogic.GetCampaignDetailsById(AppId);

                VM.Subdomain = obj.URL;
                VM.APPId = obj.AppId;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(VM);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetCode(string sendcode, string txtcopycode)
        {
            try
            {
                EmailLogic.SendEmail(sendcode, txtcopycode, "API Header Code");
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("GetCode");
        }

        public ActionResult AddCampiagns()
        {
         
           
           
            return View();
        }



        public string newapps(string Title,string URL)
        {
           
            string url = "https://onesignal.com/api/v1/apps";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add("Authorization", Key);
            request.Method = "POST";

            string data = "name=" + Title + "&chrome_web_origin=" + URL + ""; // make sure this is URL encoded
            // request.ContentType = "application/json";


            using (Stream requestStream = request.GetRequestStream())
            using (StreamWriter writer = new StreamWriter(requestStream, Encoding.ASCII))
            {
                writer.Write(data);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var AppId = "";
            // careful, non-2xx responses will throw an exception
            using (Stream responseStream = response.GetResponseStream())

            using (StreamReader reader = new StreamReader(responseStream))
            {

                //  return reader.ReadToEnd();


                string json = reader.ReadToEnd();
                AppId = JObject.Parse(json)["id"].ToString();

            }


            return AppId;


        }


       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddCampiagns(AddCampiagnViewModel VM)
        {
            if (!ModelState.IsValid)
                return View();

            var UserId = Session["UserId"].ToString();
            AspNetUsers obj = adminLogic.GetUserDetailsById(UserId);
            //AppModel model2;
            //model2 = await  CreateApp(VM.Title, VM.URL);

           // var AppId = model2.id;
            var AppId = newapps(VM.Title, VM.URL);
            if (AppId != null)
            {
                bool result = adminLogic.CreateCampaign(obj.Id, AppId, VM.Title, VM.URL, System.DateTime.Now);

                if (result == true)
                {
                    TempData["msg"] = "Your campaign has been created.";
                }
            }
            else
            {
                TempData["msg"] = "Error occured.";
            }

            return View();
        }

        public ActionResult DeleteCampaign(string AppId)
        {

            var result = adminLogic.DeleteCampaignbyId(AppId);
            return RedirectToAction("ManageCampaign");
        }
        public async Task<AppModel> CreateApp(string Title, string URL)
        {
            AppModel model = new AppModel();

            model.name = Title;
            //model.chrome_key = "AIzaSyDXtDXxDCWmr4u55IDEhoV9JYw_Zjo2O50";
            //model.chrome_web_key = "AIzaSyDXtDXxDCWmr4u55IDEhoV9JYw_Zjo2O50";
            model.chrome_web_origin = URL;
            //model.chrome_web_gcm_sender_id = "710647295840";
            //model.chrome_web_default_notification_icon = "http://www.logicdoor.com/images/Logic_logo.png";
            model.chrome_web_sub_domain = Title;

            model = await RestClient.PostResponse<AppModel>(model, "apps");

            return model;
        }

        public ActionResult AccountSetting()
        {
            AccountSettingViewModel VM = new AccountSettingViewModel();
            try
            {
                var UserId = Session["UserId"].ToString();

                AspNetUsers obj = adminLogic.GetUserDetailsById(UserId);

                VM.AppId = obj.AppId;
                VM.CompanyName = obj.SubDomain;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(VM);
        }

        [HttpPost]
        public async Task<ActionResult> AccountSetting(AccountSettingViewModel VM)
        {
            try
            {
                bool result = adminLogic.UpdateAccountSetting(VM.AppId, VM.CompanyName);

                AppModel model = new AppModel();

                model.name = VM.CompanyName;


                model = await RestClient.PutResponse<AppModel>(model, "apps/"+VM.AppId+"");

                if (result == true)
                {
                    TempData["msg"] = "Account setting has been changed. ";
                }
                else
                {
                    TempData["msg"] = "Account setting has not been changed. ";
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("AccountSetting");
        }

        public ActionResult AddNotification()
        {

            return View();
           
        }

        [HttpPost]
        public ActionResult AddNotification(string campaignid, string title, string notificationmessage, string notificationurl, string notificationsource, string notificationmedium, string ImagePath)
        {

            var id = campaignid;

            var fileName = "";
            var path1 = "";
            var path2 = "";
            

            var pic = System.Web.HttpContext.Current.Request.Files["ImagePath"];
            if (pic != null)
            {
                fileName = Path.GetFileName(pic.FileName);

                path1 = Path.Combine(Server.MapPath("~/UserImages/"),fileName);
                path2 = "/UserImages/" + fileName;
                pic.SaveAs(path1);
            }
           



            return View();

        }

        public JsonResult DropdownCampaign()
        {
            var Campaigns = new List<Campaign>();
            try
            {
               
                var UserId = Session["UserId"].ToString();
                Campaigns = adminLogic.GetAllCampaignbyuserId(UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(Campaigns, JsonRequestBehavior.AllowGet);
       
        }

        public ActionResult ApiAccess()
        {
            return View();
        }

        public ActionResult CustomizeDesktop()
        {
         return View();
        }
     public ActionResult ManageCampaign()
        {
                 
            var Campaigns = new List<Campaign>(); 
            try
            {
                var UserId = Session["UserId"].ToString();
                Campaigns = adminLogic.GetAllCampaignbyuserId(UserId);
            }
            catch(Exception ex)
            {
                throw;
            }
           return View(Campaigns);
        }

        public ActionResult ManageNotification()
        {
            return View();
        }
    }
}