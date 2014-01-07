using System.Windows.Forms;

namespace WinControlExtensionLib
{
    public static class WinControlExtension
    {
        public delegate void ControlTextMethod(Control control, Form CFrom, string text);
        public delegate void ControlEnableMethod(Control control, Form CFrom, bool enable);

        public static void SetControlText(this Control control, Form CFrom, string text)
        {
            if (CFrom.InvokeRequired)
            {
                ControlTextMethod controlTextMethod = SetControlText;
                CFrom.Invoke(controlTextMethod, new object[] { control, CFrom, text });
            }
            else
            {
                control.Text = text;
            }

            Application.DoEvents();
        }

        public static void SetControlEnable(this Control control, Form CFrom, bool enable)
        {
            if (CFrom.InvokeRequired)
            {
                ControlEnableMethod controlEnableMethod = SetControlEnable;
                CFrom.Invoke(controlEnableMethod, new object[] { control, CFrom, enable });
            }
            else
            {
                control.Enabled = enable;
            }

            Application.DoEvents();
        }

    }
}
