using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MesDepenses.Tools;

namespace MesDepenses.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult ReadData()
        {
            var stream = new StreamReader(Server.MapPath("~/App_Data/CyberPlus_OP_1_20141001165451.csv"), Encoding.Default);
            //TODO: async 
            return Json(stream.ReadToEnd().CsvToJson(), JsonRequestBehavior.AllowGet);
        }
    }
}
