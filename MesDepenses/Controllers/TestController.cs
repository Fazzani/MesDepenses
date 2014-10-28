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
            string dirPath = Server.MapPath("~/");
            var dirs = Directory.EnumerateDirectories(dirPath, "*.*", SearchOption.AllDirectories).ToList();
            _cancellationTokenSource = new CancellationTokenSource();
            var countFiles = dirs.Select(x => Directory.EnumerateFiles(x, "*", SearchOption.TopDirectoryOnly).Count()).Sum(x => x);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            return await Task.Run(async () =>
             {
                 AsyncTestModel model = new AsyncTestModel { DataDictionary = new Dictionary<string, bool>(), RealCount = countFiles };
                 try
                 {
                     foreach (var dir in dirs)
                     {
                         _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                         var files = Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly);
                         foreach (var file in files)
                         {
                             StreamReader myFile = new StreamReader(file);

                             try
                             {
                                 string myString = await myFile.ReadToEndAsync();
                                 model.DataDictionary.Add(file, myString.Contains(search));
                                 myFile.Close();
                             }
                             catch (Exception)
                             {
                                 model.DataDictionary.Add(file, false);
                             }
                             finally
                             {
                                 myFile.Close();
                             }

                         }
                     }
                     UpdateAsyncTestModel(sp, model);
                     return Json(model, JsonRequestBehavior.AllowGet);
                 }
                 catch (Exception ex)
                 {
                     UpdateAsyncTestModel(sp, model);
                     model.ErrorMessage = ex.Message;
                     return Json(model, JsonRequestBehavior.AllowGet);
                 }
                 finally
                 {
                     sp.Stop();
                 }
             }, _cancellationTokenSource.Token);

        }

        private static void UpdateAsyncTestModel(Stopwatch sp, AsyncTestModel model)
        {
            sp.Stop();
            model.ElapsedTime = sp.Elapsed.ToString();
            model.Count = model.DataDictionary.Count;
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
