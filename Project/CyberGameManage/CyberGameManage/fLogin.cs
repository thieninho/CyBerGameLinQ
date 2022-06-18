using CyberGameManage.DAO;
using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CyberGameManage
{
    public partial class fLogin : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        public fLogin()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;
            string passWord = txbPassWord.Text;
            if (Login(userName, passWord))
            {
                AccountDTO loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                fComputerManager f = new fComputerManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else 
                MessageBox.Show("Login Fail!");
        }
        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }


        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit App?", "Notification!" , MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK )
                e.Cancel = true;
        }

        private void ptb_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
