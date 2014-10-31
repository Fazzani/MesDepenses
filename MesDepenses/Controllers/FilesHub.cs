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
            var path = (HttpContext.Current == null)
                   ? System.Web.Hosting.HostingEnvironment.MapPath("~/")
                   : HttpContext.Current.Server.MapPath("~/");
            var dirs = Directory.EnumerateDirectories(path, "*.*", SearchOption.AllDirectories).ToList();
            _cancellationTokenSource = new CancellationTokenSource();
            var countFiles = dirs.Select(x => Directory.EnumerateFiles(x, "*", SearchOption.TopDirectoryOnly).Count()).Sum(x => x);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            ParallelOptions parallelOptions = new ParallelOptions { CancellationToken = _cancellationTokenSource.Token, MaxDegreeOfParallelism = Environment.ProcessorCount };
            int treatedFiles = 0;

            await Task.Run(async () =>
            {
                AsyncTestModel model = new AsyncTestModel { DataDictionary = new Dictionary<string, bool>(), RealCount = countFiles };
                try
                {
                    Parallel.ForEach(dirs, parallelOptions, dir =>
                    {
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        var files = Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly);

                        Parallel.ForEach(files, parallelOptions, async file =>
                        {
                            StreamReader myFile = new StreamReader(file);

                            try
                            {
                                string myString = await myFile.ReadToEndAsync();
                                Clients.All.addFile(file, myString.Contains(search));
                                Interlocked.Increment(ref treatedFiles);
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

                        });
                    });
                    Clients.All.end(sp.Elapsed.ToString(), countFiles, treatedFiles);
                }
                catch (Exception ex)
                {
                    model.ErrorMessage = ex.Message;
                }
                finally
                {
                    sp.Stop();
                }
            }, _cancellationTokenSource.Token);

        }

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