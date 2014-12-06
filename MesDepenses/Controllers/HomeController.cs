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
using log4net;
using MesDepenses.Models;
using MesDepensesServices;
using MesDepensesServices.DAL;
using MesDepensesServices.Domain;
using ToolsLibrary;

namespace MesDepenses.Controllers
{
  public class HomeController : Controller
  {
    private static readonly ILog _log = LogManager.GetLogger("MesDepenses");

    public ActionResult Index()
    {
      _log.Error("test error");
      var compte = new CompteModel();
      DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<OperationModel>));
      var stream = new StreamReader(Server.MapPath("~/App_Data/CyberPlus_OP_1_20141001165451.csv"), Encoding.Default);
      MemoryStream s = new MemoryStream(Encoding.UTF8.GetBytes(stream.ReadToEnd().CsvToJson()));
      compte.ListOperation = (List<OperationModel>)ser.ReadObject(s);

      return View();
    }

    public ActionResult TestTools()
    {
      var compte = new CompteModel();
      DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<OperationModel>));
      var stream = new StreamReader(Server.MapPath("~/App_Data/CyberPlus_OP_1_20141001165451.csv"), Encoding.Default);
      MemoryStream s = new MemoryStream(Encoding.UTF8.GetBytes(stream.ReadToEnd().CsvToJson()));
      compte.ListOperation = (List<OperationModel>)ser.ReadObject(s);
      compte.ListOperation.ForEach(x => x.History = new History { CreationDate = DateTime.Now, IsNew = true, OperationName = x.Libelle });
      //compte.ListOperation.OrderBy("Libelle", EnumerableExtensions.SortDirection.asc);
      compte.ListOperation.OrderBy("History.CreationDate", EnumerableExtensions.SortDirection.asc);
      return View(compte.ListOperation);
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

    public PartialViewResult MyCategories()
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
