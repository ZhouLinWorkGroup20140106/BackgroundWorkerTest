using System;
using System.ComponentModel;
using System.Threading;

namespace BackgroundWorkerExtensionLib
{

    public class CustomerDoWorkEventArgs
    {
        public object ExecParameter { get; set; }
        public BackgroundWorker ExecBackgroundWorker { get; set; }
    }

    public static class BackgroundWorkerExtension
    {

        public static BackgroundWorker InitNewBackgroundWorker(DoWorkEventHandler doWorkEvent, ProgressChangedEventHandler progressChangedEvent, RunWorkerCompletedEventHandler runWorkerCompletedEvent)
        {
            var bw = new BackgroundWorker();
            bw.RegisterDoWork(doWorkEvent);
            bw.RegisterProgressChanged(progressChangedEvent);
            bw.RegisterRunWorkerCompleted(runWorkerCompletedEvent);
            return bw;
        }

        private static void RegisterDoWork(this BackgroundWorker bw, DoWorkEventHandler doWorkEvent = null)
        {
            bw.WorkerSupportsCancellation = true;

            //默认实现
            if (doWorkEvent == null)
            {
                doWorkEvent = (sender, e) =>
                {
                    var ce = e.Argument as CustomerDoWorkEventArgs;//获取传递参数
                    BackgroundWorker cBw = ce.ExecBackgroundWorker;
                    int count = Convert.ToInt32(ce.ExecParameter.ToString(), 10);
                    for (int i = 0; i < count; i++)
                    {
                        if (!cBw.CancellationPending)//永远可以取消
                        {
                            //汇报当前处理进度
                            cBw.ReportProgress(i, "正在处理第" + i + "条数据");
                            Thread.Sleep(500);
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    e.Result = "执行操作完成的结果：1";
                };

            }

            bw.DoWork += doWorkEvent;
        }

        private static void RegisterProgressChanged(this BackgroundWorker bw, ProgressChangedEventHandler progressChangedEvent = null)
        {
            bw.WorkerReportsProgress = true;

            //默认实现
            if (progressChangedEvent == null)
            {
                progressChangedEvent = (sender, e) =>
                {
                    var userState = e.UserState;
                    Console.WriteLine("DoWork中传递的执行参数为" + userState);
                    var percentage = e.ProgressPercentage;
                    Console.WriteLine("DoWork中当前的执行进度为" + percentage);
                };
            }

            bw.ProgressChanged += progressChangedEvent;
        }

        private static void RegisterRunWorkerCompleted(this BackgroundWorker bw, RunWorkerCompletedEventHandler runWorkerCompletedEvent = null)
        {

            if (runWorkerCompletedEvent == null)
            {
                runWorkerCompletedEvent = (sender, e) =>
                {
                    if (e.Cancelled)
                    {
                        Console.WriteLine("取消成功");
                    }
                    else if (e.Error != null)
                    {
                        var error = e.Error;
                        Console.WriteLine("执行出错，返回的结果是-" + error);
                    }
                    else
                    {
                        var res = e.Result;
                        Console.WriteLine("操作完成，返回的结果是-" + res); // 从 DoWork
                    }
                };
            }

            bw.RunWorkerCompleted += runWorkerCompletedEvent;
        }

        public static void CancelAsyncExt(this BackgroundWorker bw)
        {
            bw.CancelAsync();
        }

        public static void RunWorkerAsyncExt(this BackgroundWorker bw, object param)
        {
            //只有在BackgroundWorker空闲的时候才执行
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync(new CustomerDoWorkEventArgs
                {
                    ExecParameter = param ?? 10,
                    ExecBackgroundWorker = bw
                });
            }
 
        }

    }

}
