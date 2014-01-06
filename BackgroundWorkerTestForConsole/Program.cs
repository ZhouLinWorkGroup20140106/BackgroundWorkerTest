using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgroundWorkerExtensionLib;

namespace BackgroundWorkerTestForConsole
{
    class Program
    {
        private static readonly BackgroundWorker bw = BackgroundWorkerExtension.InitNewBackgroundWorker(null,null,null);

        static void Main(string[] args)
        {
            bw.RunWorkerAsyncExt(null); 

            Console.ReadLine();

            bw.CancelAsyncExt();

            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
