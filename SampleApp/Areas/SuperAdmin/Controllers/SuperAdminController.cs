using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleApp.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SuperAdminController : Controller
    {
        //
        // GET: /SuperAdmin/SuperAdmin/
        public ActionResult Index()
        {
            return View();
        }
	}
}