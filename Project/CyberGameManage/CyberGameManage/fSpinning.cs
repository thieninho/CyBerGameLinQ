using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CyberGameManage
{
    public partial class fSpinning : Form
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
        public fSpinning()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            int iRnd = new int();
            #region DiceSHow1
            iRnd = r.Next(0, 6);
            if (iRnd == 0)
                pbDiceShow1.Image = pbDice1.Image;
            else if (iRnd == 1)
                pbDiceShow1.Image = pbDice2.Image;
            else if (iRnd == 2)
                pbDiceShow1.Image = pbDice3.Image;
            else if (iRnd == 3)
                pbDiceShow1.Image = pbDice4.Image;
            else if (iRnd == 4)
                pbDiceShow1.Image = pbDice5.Image;
            else
                pbDiceShow1.Image = pbDice6.Image;
            #endregion

            #region DiceSHow2
            iRnd = r.Next(0, 6);
            if (iRnd == 0)
                pbDiceShow2.Image = pbDice1.Image;
            else if (iRnd == 1)
                pbDiceShow2.Image = pbDice2.Image;
            else if (iRnd == 2)
                pbDiceShow2.Image = pbDice3.Image;
            else if (iRnd == 3)
                pbDiceShow2.Image = pbDice4.Image;
            else if (iRnd == 4)
                pbDiceShow2.Image = pbDice5.Image;
            else
                pbDiceShow2.Image = pbDice6.Image;
            #endregion

            #region DiceSHow3
            iRnd = r.Next(0, 6);
            if (iRnd == 0)
                pbDiceShow3.Image = pbDice1.Image;
            else if (iRnd == 1)
                pbDiceShow3.Image = pbDice2.Image;
            else if (iRnd == 2)
                pbDiceShow3.Image = pbDice3.Image;
            else if (iRnd == 3)
                pbDiceShow3.Image = pbDice4.Image;
            else if (iRnd == 4)
                pbDiceShow3.Image = pbDice5.Image;
            else
                pbDiceShow3.Image = pbDice6.Image;
            #endregion


        }

        private void pb_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
