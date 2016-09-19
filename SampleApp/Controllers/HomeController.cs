using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using SampleApp.Helper;
using SampleApp.Logic.Contracts;
using SampleApp.Model.Models;
using SampleApp.Models;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
    /// <summary>
    /// After login only user is able to access following section
    /// </summary>
    //[Authorize]
    public class HomeController : Controller
    {
        public readonly IAccountLogic accountLogic;
      

        public static readonly string Key = ConfigurationManager.AppSettings["API-KEY"].ToString();

        public HomeController(IAccountLogic _accountLogic)
        {
            accountLogic = _accountLogic;
          
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        /// <summary>
        /// Users dashboard screen
        /// </summary>
        /// <returns></returns>
        ///
        public ActionResult Index()
        {

            return View();
            
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
          
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Index([Bind(Exclude = "AppId,ConfirmPassword,Country,Address,City,Landmark,PhoneNumber")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //AppModel model2;

                //model2 = await CreateApp(model.FirstName, model.Subdomain);

                var AppId = "";
               
                var user = new SampleApp.Models.ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Website = model.Website, SubDomain = model.Subdomain, AppId = AppId };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                   // adminLogic.CreateCampaign(user.Id, AppId, model.FirstName, model.Subdomain, System.DateTime.Now);

                   // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                   // Session["UserId"] = model.Email;
                    UserManager.AddToRole(user.Id, "Customer");
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", HttpUtility.HtmlEncode("<p>Please confirm your account by clicking</p> <a href=\"" + callbackUrl + "\">here</a>"));
                    // return RedirectToAction("Index", "Home");
                    ViewBag.Message = "Your account has been created.";
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<AppModel> CreateApp(string FirstName, string Subdomain)
        {
            AppModel model = new AppModel();
            model.name =FirstName;
            //model.chrome_key = "AIzaSyDXtDXxDCWmr4u55IDEhoV9JYw_Zjo2O50";
            //model.chrome_web_key = "AIzaSyDXtDXxDCWmr4u55IDEhoV9JYw_Zjo2O50";
            //model.chrome_web_origin = "http://www.logicdoor.com";
            //model.chrome_web_gcm_sender_id = "710647295840";
            //model.chrome_web_default_notification_icon = "http://www.logicdoor.com/images/Logic_logo.png";
            model.chrome_web_sub_domain = Subdomain;
            model = await RestClient.PostResponse<AppModel>(model, "apps");
           
             

            return model;
        }


        public ActionResult Features()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult API()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

       
    }
}