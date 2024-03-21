using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PdfPrime.AppSettings;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.IO.Util.IntHashtable;

namespace PdfPrime.Services
{
    public interface IRunService
    {
        public Task Run();
    }
    public class RunService : IRunService
    {
        private readonly ApplicationOptions _options;
        private readonly IBaseService _service;
        private readonly IFileSecureService _fileSecureService;
        private readonly ITempFileService _tempFileService;

        public RunService(IBaseService baseService, IFileSecureService fileSecureService, ITempFileService tempFileService)
        {
            _options = baseService.Options;
            _service = baseService;
            _fileSecureService = fileSecureService;
            _tempFileService = tempFileService;
        }
        public async Task RunTest()
        {
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(5);
            List<Task> tasks = new List<Task>();

            // Create a TaskFactory and pass it our custom scheduler.
            TaskFactory factory = new TaskFactory(lcts);
            CancellationTokenSource cts = new CancellationTokenSource();

            Dictionary<string, List<(PdfPage, string[])>> dicPdfs = new Dictionary<string, List<(PdfPage, string[])>>();

            if (_service.CommandLineOptions.Input != null)
            {
                string[] fileEntries = Directory.GetFiles(_service.CommandLineOptions.Input);

                Task t = factory.StartNew(() =>
                {
                    foreach (string fileEntry in fileEntries)
                    {
                        PdfService pdfService = new PdfService();
                        //var lstPdfContent = pdfService.Read(fileEntry).ToList();
                        dicPdfs.Add(fileEntry, pdfService.Read(fileEntry).ToList());
                        Log.Logger.Debug("loading {0:10} {1:5} {2:5}", Path.GetFileName(fileEntry), Path.GetExtension(fileEntry), fileEntry.Length);
                    }

                }, cts.Token);
                tasks.Add(t);
            }
            //Task.WaitAll(tasks.ToArray());
            await Task.WhenAll(tasks);

            if (_service.CommandLineOptions.Output != null)
            {
                Task t = factory.StartNew(async () =>
                {
                    foreach (var pdf in dicPdfs)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var entry in pdf.Value)
                        {
                            var vv = entry.Item2.Select(x => x.ToString());
                            sb.AppendLine(string.Join(Environment.NewLine, vv));
                            Log.Logger.Debug("{0}", vv.Select(x => x.ToString()));
                        }
                        Log.Logger.Information("loading {0:10} {1:5} {2:5}", Path.GetFileName(pdf.Key), Path.GetExtension(pdf.Key), sb.Length);
                        await _tempFileService.UpdateAsync(Path.Combine(_service.Options.ApplicationOutputPath!, $"{Path.GetFileNameWithoutExtension(pdf.Key)}.txt"), sb.ToString());
                    }

                }, cts.Token);
                tasks.Add(t);

            }
            //Task.WaitAll(tasks.ToArray());
            //pdfService.Read()

            await Task.WhenAll(tasks);
            await Task.CompletedTask;
        }
        public async Task Run()
        {

            //var fileName = Path.Combine(_service.Options.ApplicationOutputPath!, $"{Guid.NewGuid()}.txt");

            //await _tempFileService.UpdateAsync(fileName, "sample record");
            //await _tempFileService.DeleteAsync(fileName);

            //IEnumerable<PdfPage, string[]>
            //Dictionary < string, IEnumerable < (PdfPage, string[]) > = new Dictionary<string, IEnumerable<PdfPage, string>>();

            Dictionary<string, List<(PdfPage, string[])>> dicPdfs = new Dictionary<string, List<(PdfPage, string[])>>();
            if (_service.CommandLineOptions.Input != null)
            {


                string[] fileEntries = Directory.GetFiles(_service.CommandLineOptions.Input);

                foreach (string fileEntry in fileEntries)
                {
                    PdfService pdfService = new PdfService();
                    //var lstPdfContent = pdfService.Read(fileEntry).ToList();
                    dicPdfs.Add(fileEntry, pdfService.Read(fileEntry).ToList());
                    Log.Logger.Debug("loading {0:10} {1:5} {2:5}", Path.GetFileName(fileEntry), Path.GetExtension(fileEntry), fileEntry.Length);
                }

                //pdfService.Read
            }

            if (_service.CommandLineOptions.Output != null)
            {
                foreach (var pdf in dicPdfs)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var entry in pdf.Value)
                    {
                        var vv = entry.Item2.Select(x => x.ToString());
                        sb.AppendLine(string.Join(Environment.NewLine, vv));
                        Log.Logger.Debug("{0}", vv.Select(x => x.ToString()));
                    }
                    Log.Logger.Information("loading {0:10} {1:5} {2:5}", Path.GetFileName(pdf.Key), Path.GetExtension(pdf.Key), sb.Length);
                    await _tempFileService.UpdateAsync(Path.Combine(_service.Options.ApplicationOutputPath!, $"{Path.GetFileNameWithoutExtension(pdf.Key)}.txt"), sb.ToString());
                }
            }

            //pdfService.Read()


            await Task.CompletedTask;
        }
        public async Task RunSchedule()
        {
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(5);
            List<Task> tasks = new List<Task>();

            // Create a TaskFactory and pass it our custom scheduler.
            TaskFactory factory = new TaskFactory(lcts);
            CancellationTokenSource cts = new CancellationTokenSource();

            // Use our factory to run a set of tasks.
            Object lockObj = new Object();
            int outputItem = 0;


            //int iteration = tCtr;
            Task t = factory.StartNew(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    //lock (lockObj)
                    //{
                    //lcts.
                    Task.Delay(1000);
                    Console.Write("{0} in task on thread  scheid {1} ",
                                  i, lcts.Id);
                    outputItem++;
                    if (outputItem % 3 == 0)
                        Console.WriteLine();
                    //}
                }
            }, cts.Token);
            tasks.Add(t);



            //for (int tCtr = 0; tCtr <= 4; tCtr++)
            //{
            //    int iteration = tCtr;
            //    Task t = factory.StartNew(() =>
            //    {
            //        for (int i = 0; i < 1000; i++)
            //        {
            //            lock (lockObj)
            //            {
            //                Console.Write("{0} in task t-{1} on thread {2}   ",
            //                              i, iteration, Thread.CurrentThread.ManagedThreadId);
            //                outputItem++;
            //                if (outputItem % 3 == 0)
            //                    Console.WriteLine();
            //            }
            //        }
            //    }, cts.Token);
            //    tasks.Add(t);
            //}
            // Use it to run a second set of tasks.
            //for (int tCtr = 0; tCtr <= 4; tCtr++)
            //{
            //    int iteration = tCtr;
            //    Task t1 = factory.StartNew(() =>
            //    {
            //        for (int outer = 0; outer <= 10; outer++)
            //        {
            //            for (int i = 0x21; i <= 0x7E; i++)
            //            {
            //                lock (lockObj)
            //                {
            //                    Console.Write("'{0}' in task t1-{1} on thread {2}   ",
            //                                  Convert.ToChar(i), iteration, Thread.CurrentThread.ManagedThreadId);
            //                    outputItem++;
            //                    if (outputItem % 3 == 0)
            //                        Console.WriteLine();
            //                }
            //            }
            //        }
            //    }, cts.Token);
            //    tasks.Add(t1);
            //}

            // Wait for the tasks to complete before displaying a completion message.


            Task.WaitAll(tasks.ToArray());
            cts.Dispose();

            //factory.

            await Task.FromResult(0);
            Console.WriteLine("\n\nSuccessful completion.");

        }

    }
}
