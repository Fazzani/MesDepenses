using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MesDepenses.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MesDepenses.Controllers
{
    public class FilesHub : Hub
    {
        static CancellationTokenSource _cancellationTokenSource;

        //[AsyncTimeout(100000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async void Lancer()
        {
            string search = "jquery";
            var path = (System.Web.HttpContext.Current == null)
                   ? System.Web.Hosting.HostingEnvironment.MapPath("~/")
                   : System.Web.HttpContext.Current.Server.MapPath("~/");
            var dirs = Directory.EnumerateDirectories(path, "*.*", SearchOption.AllDirectories).ToList();
            _cancellationTokenSource = new CancellationTokenSource();
            var countFiles = dirs.Select(x => Directory.EnumerateFiles(x, "*", SearchOption.TopDirectoryOnly).Count()).Sum(x => x);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            await Task.Run(async () =>
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
                                Clients.All.addFile(file, myString.Contains(search));
                                //model.DataDictionary.Add(file, myString.Contains(search));
                                myFile.Close();
                            }
                            catch (Exception)
                            {
                                Clients.All.addFile(file, false);
                            }
                            finally
                            {
                                myFile.Close();
                            }

                        }
                    }
                    Clients.All.end(sp.Elapsed.ToString(), countFiles);
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    //UpdateAsyncTestModel(sp, model);
                    model.ErrorMessage = ex.Message;
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    sp.Stop();
                }
            }, _cancellationTokenSource.Token);

        }

        //private static void UpdateAsyncTestModel(Stopwatch sp, AsyncTestModel model)
        //{
        //    sp.Stop();
        //    model.ElapsedTime = sp.Elapsed.ToString();
        //    model.Count = model.DataDictionary.Count;
        //}

        public async void Arreter()
        {
            if (_cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();

            }
        }
    }
}