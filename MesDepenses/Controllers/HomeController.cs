using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MesDepenses.Models;
using MesDepensesServices;
using MesDepensesServices.DAL;
using MesDepensesServices.Domain;

namespace MesDepenses.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var compte = new CompteModel();
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<OperationModel>));
            //var stream = new StreamReader(Server.MapPath("~/App_Data/CyberPlus_OP_1_20141001165451.csv"), Encoding.Default);
            //MemoryStream s = new MemoryStream(Encoding.UTF8.GetBytes(stream.ReadToEnd().CsvToJson()));
            //compte.ListOperation = (List<OperationModel>)ser.ReadObject(s);
            using (var repository = new MesdepensesContext())
            {
                repository.Categories.Add(new Categorie {Libelle = "Dépenses"});
                repository.SaveChanges();
                var tmp = repository.Categories.Count();
                Debug.WriteLine(tmp);
            }
            
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

        public PartialViewResult MyOperations()
        {
            return PartialView();
        }

        public JsonResult ReadData()
        {
            var stream = new StreamReader(Server.MapPath("~/App_Data/CyberPlus_OP_1_20141001165451.csv"), Encoding.Default);
            //TODO: async 
            var tmp = stream.ReadToEnd().CsvToJson();
            return Json(tmp, JsonRequestBehavior.AllowGet);
        }
    }
}
