using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using BackgroundWorkerExtensionLib;
using WinControlExtensionLib;

namespace BackgroundWorkerTestForWinForm
{
    public partial class Form1 : Form
    {

        private readonly BackgroundWorker bw;

        public Form1()
        {
            InitializeComponent();
            bw = BackgroundWorkerExtension.InitNewBackgroundWorker(bw_DoWork, bw_ProgressChanged, bw_RunWorkerCompleted);
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var ce = e.Argument as CustomerDoWorkEventArgs;
            BackgroundWorker cBw = ce.ExecBackgroundWorker;
            btnCancel.SetControlEnable(this, true);
            int count = Convert.ToInt32(ce.ExecParameter.ToString(), 10);
            for (int i = 0; i < count; i++)
            {
                if (!cBw.CancellationPending)
                {
                    cBw.ReportProgress(i, "正在处理第" + i + "条数据");
                    Thread.Sleep(500);
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var userState = e.UserState;
            lblMessage.Text = "正在处理" + e.ProgressPercentage;
            lsRes.Items.Add(userState);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            btnCancel.Enabled = false;

            if (e.Cancelled)
            {
                MessageBox.Show("取消成功");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("执行出错 " + e.Error);
            }
            else
            {
                var res = e.Result; // 从 DoWork
                MessageBox.Show("操作完成 " + res);
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsyncExt(txtExecCount.Text);
            }      
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bw.CancelAsyncExt();
        }

    }

}
