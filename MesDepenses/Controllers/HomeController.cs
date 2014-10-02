using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(Server.MapPath("~/App_Data/CyberPlus_OP_1_20141001165451.csv")))
            {
                CsvRow row = new CsvRow();
                var csv = new List<string[]>();
                while (reader.ReadRow(row))
                {
                    foreach (string s in row)
                        csv.Add(s.Split(','));
                }
                string json = new
                            System.Web.Script.Serialization.JavaScriptSerializer().Serialize(csv);
                Debug.WriteLine(json);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
