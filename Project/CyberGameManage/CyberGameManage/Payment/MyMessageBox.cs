using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Payment
{
    public static class MyMessageBox
    {
        public static System.Windows.Forms.DialogResult ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons button, System.Windows.Forms.MessageBoxIcon icon) 
        {
            System.Windows.Forms.DialogResult dlgResult = System.Windows.Forms.DialogResult.None;
            using (fMessageBox msgBox = new fMessageBox())
            {
                msgBox.Text = caption;
                msgBox.Message = message;
                switch(icon)
                {
                    case System.Windows.Forms.MessageBoxIcon.Information:
                        msgBox.MessageIcon = CyberGameManage.Properties.Resources.Cash;
                        break;
                    case System.Windows.Forms.MessageBoxIcon.Question:
                        msgBox.MessageIcon = CyberGameManage.Properties.Resources.MoMo;
                        break;
                }
                dlgResult = msgBox.ShowDialog();
            }
            return dlgResult;
        }


    }
}
