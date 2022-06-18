using CyberGameManage.Decorator;
using CyberGameManage.Payment;
using CyberGameManage.Rate.Decorator;
using Microsoft.JScript;
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

    public partial class fMessageBox : Form
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
        public fMessageBox()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        #region subsustemfacade
        public class AccountService
        {
            public void GetAccout(string name)
            {
                MessageBox.Show("Payment for the cashier: " + name);
            }
        }

        public class EmailService
        {
            public void SendMail(string mailTo)
            {
                MessageBox.Show("Sending an email to " + mailTo);
            }
        }
        public class PaymentService
        {
            public void PaymentByMomo()
            {
                MessageBox.Show("Payment by Momo");
            }
            public void PaymentByCreditCard()
            {
                MessageBox.Show("Payment by Credit Card");
            }
            public void PaymentByEBankingAccount()
            {
                MessageBox.Show("Payment by E-banking account");
            }
            public void PaymentByCash()
            {
                MessageBox.Show("Payment by cash");
            }
        }

        public class VATService
        {
            public void FreeVAT()
            {
                MessageBox.Show("Free VAT");
            }

            public void StandardVAT()
            {
                MessageBox.Show("Standard VAT");
            }

            public void ExpressVAT()
            {
                MessageBox.Show("Express VAT");
            }
        }

        public class SmsService
        {
            public void sendSMS(string mobilePhone)
            {
                MessageBox.Show("Payment Momo by mobilephone: " + mobilePhone);
            }
        }
        #endregion

        #region decorator
        private IRate pRate(IRate raTe)
        {
            if (rd1sao.Checked)
            {
                raTe = new OneStar(raTe);
                rd1sao.Checked = false;
                return pRate(raTe);
            }
            if (rd2sao.Checked)
            {
                raTe = new TwoStar(raTe);
                rd2sao.Checked = false;
                return pRate(raTe);
            }
            if (rd3sao.Checked)
            {
                raTe = new ThreeStar(raTe);
                rd3sao.Checked = false;
                return pRate(raTe);
            }
            if (rd4sao.Checked)
            {
                raTe = new FourStar(raTe);
                rd4sao.Checked = false;
                return pRate(raTe);
            }
            if (rd5sao.Checked)
            {
                raTe = new FiveStar(raTe);
                rd5sao.Checked = false;
                return pRate(raTe);
            }
            else
            {
                lbFeddback.Text = raTe.Feedback();
                return null;
            }

        }


         #endregion

        public Image MessageIcon
        {
            get { return pictureBox.Image; }
            set { pictureBox.Image = value; }
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; } 
        }

        private void ptb_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            pRate(new IFeedBack());
        }
    }
}
