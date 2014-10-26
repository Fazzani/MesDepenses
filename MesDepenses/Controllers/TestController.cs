using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MesDepenses.Models;
using MesDepensesServices;
using MesDepensesServices.DAL;
using MesDepensesServices.Domain;

namespace MesDepenses.Controllers
{
    public class TestController : AsyncController
    {
        static CancellationTokenSource _cancellationTokenSource;
        public ActionResult Index()
        {
            return PartialView();
        }

        //[AsyncTimeout(100000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async Task<JsonResult> Lancer(string search = "jquery", CancellationToken cancelToken = default(CancellationToken))
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return await Task.Run(async () =>
             {
                 Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
                 try
                 {

                     foreach (var dir in Directory.EnumerateDirectories(Server.MapPath("~/")))
                     {
                         await Task.Delay(1000);
                         _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                         var files = Directory.EnumerateFiles(dir);
                         foreach (var file in files)
                         {
                             StreamReader myFile = new StreamReader(file);
                             string myString = await myFile.ReadToEndAsync().ConfigureAwait(false);
                             dictionary.Add(file, myString.Contains(search));
                             myFile.Close();
                         }
                     }

                     return Json(dictionary, JsonRequestBehavior.AllowGet);
                 }
                 catch (OperationCanceledException ex)
                 {
                     Console.WriteLine(ex.Message);
                     return Json(dictionary, JsonRequestBehavior.AllowGet);

                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex.Message);
                     return Json(dictionary, JsonRequestBehavior.AllowGet);

                 }
             }, _cancellationTokenSource.Token);

        }

        public async Task<JsonResult> Arreter()
        {
            if (_cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                return Json(new { Cancel = "ok" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { Cancel = "can't be canceled!!" }, JsonRequestBehavior.AllowGet);
        }

    }
}
