using CyberGameManage.DAO;
using CyberGameManage.DTO;
using CyberGameManage.Payment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace CyberGameManage
{
    public partial class fComputerManager : Form
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
        private AccountDTO loginAccount;

        public AccountDTO LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); } // tính đóng gói OOP 
        }
        public fComputerManager(AccountDTO acc)
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.LoginAccount = acc;

            LoadComputer();
            LoadCategory();
            LoadComboboxComputer(cbSwitchComputer);
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            accountInformationToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<CategoryDTO> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadOrderListByCategoryID(int id)
        {
            List<OrderDTO> listOrderr = OrderDAO.Instance.GetOrderrByCategoryID(id);
            cbOrderr.DataSource = listOrderr;
            cbOrderr.DisplayMember = "Name";
        }

        void LoadComputer()
        {
            flpTable.Controls.Clear();
            List<ComputerDTO> computerList = ComputerDAO.Instance.LoadComputerList();
            foreach (ComputerDTO item in computerList)
            {
                Button btn = new Button() { Width = ComputerDAO.ComputerWidth, Height = ComputerDAO.ComputerHeight };
                btn.Image = Image.FromFile("Resources\\PCC123.png");
                //btn.BackgroundImageLayout = ImageLayout.Stretch;

                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Font = new Font("Palatino Linotype", 9, FontStyle.Bold);
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.SkyBlue;
                        break;
                    default:
                        btn.BackColor = Color.OrangeRed;
                        break;
                }
                flpTable.Controls.Add(btn);
            }

        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.MenuDTO> listBillInfo = DAO.MenuDAO.Instance.GetListMenuByComputer(id);
            float totalPrice = 0;
            foreach (DTO.MenuDTO item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.OrderName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }
        void LoadComboboxComputer(ComboBox cb)
        {
            cb.DataSource = ComputerDAO.Instance.LoadComputerList();
            cb.DisplayMember = "Name";
        }

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int computerID = ((sender as Button).Tag as ComputerDTO).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(computerID);
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //fLogin f = new fLogin();
            //f.ShowDialog();


        }

        private void userInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
            accountInformationToolStripMenuItem.Text = "Account (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertOrder += f_InsertOrder;
            f.DeleteOrder += f_DeleteOrder;
            f.UpdateOrder += f_UpdateOrder;
            f.ShowDialog();
        }

        void f_UpdateOrder(object sender, EventArgs e)
        {
            LoadOrderListByCategoryID((cbCategory.SelectedItem as CategoryDTO).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as ComputerDTO).ID);
        }

        void f_DeleteOrder(object sender, EventArgs e)
        {
            LoadOrderListByCategoryID((cbCategory.SelectedItem as CategoryDTO).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as ComputerDTO).ID);
            LoadComputer();
        }

        void f_InsertOrder(object sender, EventArgs e)
        {
            LoadOrderListByCategoryID((cbCategory.SelectedItem as CategoryDTO).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as ComputerDTO).ID);
        }
        #endregion

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            CategoryDTO selected = cb.SelectedItem as CategoryDTO;
            id = selected.ID;
            LoadOrderListByCategoryID(id);
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            ComputerDTO computer = lsvBill.Tag as ComputerDTO;

            if (computer == null)
            {
                MessageBox.Show("You have not selected the Computer!", "Thông báo");
                return;
            }


            int idBill = BillDAO.Instance.GetUncheckBillIDByComputerID(computer.ID);
            int orderID = (cbOrderr.SelectedItem as OrderDTO).ID;
            int count = (int)nmOrderCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(computer.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), computer.ID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, orderID, count);
            }

            ShowBill(computer.ID);
            LoadComputer();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            ComputerDTO computer = lsvBill.Tag as ComputerDTO;
            if (computer == null)
            {
                MessageBox.Show("You have not selected the Computer!", "Thông báo");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByComputerID(computer.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", ""));
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MyMessageBox.ShowMessage(string.Format("Your bill for the {0}:\n\nFinal Price = Total Price - (Total Price / 100) x Discount \n\n\t= {1} - ({1} / 100) x {2} \n\n\t= {3} VND \n\n\t Cashier: Nguyen Thanh Thien \t\t| Mobile: 0977214077", computer.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.None) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(computer.ID);
                    //MyMessageBox.ShowMessage("Payment by Cash", "Thank you", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Facade.getInstance().buyProductByCashWithFreeShipping("19110150@student.hcmute.edu.vn");
                    //Facade.getInstance().buyProductByPaypalWithStandardShipping("19110150@student.hcmute.edu.vn", "0977214077");
                    LoadComputer();
                }
            }
        }
        private void btn_cash_Click(object sender, EventArgs e)
        {
            ComputerDTO computer = lsvBill.Tag as ComputerDTO;
            if (computer == null)
            {
                MessageBox.Show("You have not selected the Computer!", "Thông báo");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByComputerID(computer.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", ""));
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Are you sure to pay the bill for the {0}:\n\nFinal Price = Total Price - (Total Price / 100) x Discount \n\n\t= {1} - ({1} / 100) x {2} \n\n\t= {3} VND", computer.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.None) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(computer.ID);

                    Facade.getInstance().buyProductByCash("Nguyen Thanh Thien");

                    //Facade.getInstance().buyProductByPaypalWithStandardShipping("19110150@student.hcmute.edu.vn", "0977214077");
                    LoadComputer();
                }
            }
        }
        private void btn_Momo_Click(object sender, EventArgs e)
        {
            ComputerDTO computer = lsvBill.Tag as ComputerDTO;
            if (computer == null)
            {
                MessageBox.Show("You have not selected the Computer!", "Thông báo");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByComputerID(computer.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", ""));
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Are you sure to pay the bill for the {0}:\n\nFinal Price = Total Price - (Total Price / 100) x Discount \n\n\t= {1} - ({1} / 100) x {2} \n\n\t= {3} VND", computer.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.None) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(computer.ID);

                    Facade.getInstance().buyProductByMomo("19110150@student.hcmute.edu.vn", "0977214077");

                    LoadComputer();

                }
            }

        }

        private void btnSwitchComputer_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as ComputerDTO).ID;

            int id2 = (cbSwitchComputer.SelectedItem as ComputerDTO).ID;
            if (MessageBox.Show(string.Format("Do you want to switch from machine {0} to machine {1}", (lsvBill.Tag as ComputerDTO).Name, (cbSwitchComputer.SelectedItem as ComputerDTO).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                ComputerDAO.Instance.SwitchComputer(id1, id2);

                LoadComputer();
            }
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckout_Click(this, new EventArgs());
        }

        private void orderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddOrder_Click(this, new EventArgs());
        }

        private void cbOrderr_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fComputerManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            t1.Stop();
            t2.Stop();
            Application.DoEvents();
            if (MessageBox.Show("Do you want to exit App?", "Notification!", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
        }

        private void ptb_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            fSpinning f = new fSpinning();
            f.ShowDialog();
            this.Show();
        }
        #region Time
        private void fComputerManager_Load(object sender, EventArgs e)
        {
            t1 = new System.Timers.Timer();
            t1.Interval = 1000;
            t1.Elapsed += OnTimeEvent1;

            t2 = new System.Timers.Timer();
            t2.Interval = 1000;
            t2.Elapsed += OnTimeEvent2;

            t3 = new System.Timers.Timer();
            t3.Interval = 1000;
            t3.Elapsed += OnTimeEvent3;

            t4 = new System.Timers.Timer();
            t4.Interval = 1000;
            t4.Elapsed += OnTimeEvent4;

            t5 = new System.Timers.Timer();
            t5.Interval = 1000;
            t5.Elapsed += OnTimeEvent5;

            t6 = new System.Timers.Timer();
            t6.Interval = 1000;
            t6.Elapsed += OnTimeEvent6;

            t7 = new System.Timers.Timer();
            t7.Interval = 1000;
            t7.Elapsed += OnTimeEvent7;

            t8 = new System.Timers.Timer();
            t8.Interval = 1000;
            t8.Elapsed += OnTimeEvent8;

            t9 = new System.Timers.Timer();
            t9.Interval = 1000;
            t9.Elapsed += OnTimeEvent9;

            t10 = new System.Timers.Timer();
            t10.Interval = 1000;
            t10.Elapsed += OnTimeEvent10;

            t11 = new System.Timers.Timer();
            t11.Interval = 1000;
            t11.Elapsed += OnTimeEvent11;

            t12 = new System.Timers.Timer();
            t12.Interval = 1000;
            t12.Elapsed += OnTimeEvent12;

            t13 = new System.Timers.Timer();
            t13.Interval = 1000;
            t13.Elapsed += OnTimeEvent13;

            t14 = new System.Timers.Timer();
            t14.Interval = 1000;
            t14.Elapsed += OnTimeEvent14;

            t15 = new System.Timers.Timer();
            t15.Interval = 1000;
            t15.Elapsed += OnTimeEvent15;

            t16 = new System.Timers.Timer();
            t16.Interval = 1000;
            t16.Elapsed += OnTimeEvent16;

            t17 = new System.Timers.Timer();
            t17.Interval = 1000;
            t17.Elapsed += OnTimeEvent17;

            t18 = new System.Timers.Timer();
            t18.Interval = 1000;
            t18.Elapsed += OnTimeEvent18;

            t19 = new System.Timers.Timer();
            t19.Interval = 1000;
            t19.Elapsed += OnTimeEvent19;

            t20 = new System.Timers.Timer();
            t20.Interval = 1000;
            t20.Elapsed += OnTimeEvent20;

            t21 = new System.Timers.Timer();
            t21.Interval = 1000;
            t21.Elapsed += OnTimeEvent21;

            t22 = new System.Timers.Timer();
            t22.Interval = 1000;
            t22.Elapsed += OnTimeEvent22;

            t23 = new System.Timers.Timer();
            t23.Interval = 1000;
            t23.Elapsed += OnTimeEvent23;

            t24 = new System.Timers.Timer();
            t24.Interval = 1000;
            t24.Elapsed += OnTimeEvent24;

            t25 = new System.Timers.Timer();
            t25.Interval = 1000;
            t25.Elapsed += OnTimeEvent25;

            t26 = new System.Timers.Timer();
            t26.Interval = 1000;
            t26.Elapsed += OnTimeEvent26;

            t27 = new System.Timers.Timer();
            t27.Interval = 1000;
            t27.Elapsed += OnTimeEvent27;

            t28 = new System.Timers.Timer();
            t28.Interval = 1000;
            t28.Elapsed += OnTimeEvent28;

            t29 = new System.Timers.Timer();
            t29.Interval = 1000;
            t29.Elapsed += OnTimeEvent29;

            t30 = new System.Timers.Timer();
            t30.Interval = 1000;
            t30.Elapsed += OnTimeEvent30;
        }
        System.Timers.Timer t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24, t25, t26, t27, t28, t29, t30;
        int h1, m1, s1, h2, m2, s2, h3, m3, s3, h4, m4, s4, h5, m5, s5, h6, m6, s6, h7, m7, s7, h8, m8, s8, h9, m9, s9, h10, m10, s10, h11, m11, s11, h12, m12, s12, h13, m13, s13, h14, m14, s14, h15, m15, s15, h16, m16, s16, h17, m17, s17, h18, m18, s18, h19, m19, s19, h20, m20, s20, h21, m21, s21, h22, m22, s22, h23, m23, s23, h24, m24, s24, h25, m25, s25, h26, m26, s26, h27, m27, s27, h28, m28, s28, h30, m30, s30, h29, m29, s29;

        #region Ontime
        private void OnTimeEvent1(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s1 += 1;
                if (s1 == 60)
                {
                    s1 = 0;
                    m1 += 1;
                }
                if (m1 == 60)
                {
                    m1 = 0;
                    h1 += 1;
                }
                txtResult1.Text = string.Format("{0}:{1}:{2}", h1.ToString().PadLeft(2, '0'), m1.ToString().PadLeft(2, '0'), s1.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent2(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s2 += 1;
                if (s2 == 60)
                {
                    s2 = 0;
                    m2 += 1;
                }
                if (m2 == 60)
                {
                    m2 = 0;
                    h2 += 1;
                }
                txtResult2.Text = string.Format("{0}:{1}:{2}", h2.ToString().PadLeft(2, '0'), m2.ToString().PadLeft(2, '0'), s2.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent3(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s3 += 1;
                if (s3 == 60)
                {
                    s3 = 0;
                    m3 += 1;
                }
                if (m3 == 60)
                {
                    m3 = 0;
                    h3 += 1;
                }
                txtResult3.Text = string.Format("{0}:{1}:{2}", h3.ToString().PadLeft(2, '0'), m3.ToString().PadLeft(2, '0'), s3.ToString().PadLeft(2, '0'));

            }));
        }


        private void OnTimeEvent4(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s4 += 1;
                if (s4 == 60)
                {
                    s4 = 0;
                    m4 += 1;
                }
                if (m4 == 60)
                {
                    m4 = 0;
                    h4 += 1;
                }
                txtResult4.Text = string.Format("{0}:{1}:{2}", h4.ToString().PadLeft(2, '0'), m4.ToString().PadLeft(2, '0'), s4.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent5(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s5 += 1;
                if (s5 == 60)
                {
                    s5 = 0;
                    m5 += 1;
                }
                if (m5 == 60)
                {
                    m5 = 0;
                    h5 += 1;
                }
                txtResult5.Text = string.Format("{0}:{1}:{2}", h5.ToString().PadLeft(2, '0'), m5.ToString().PadLeft(2, '0'), s5.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent6(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s6 += 1;
                if (s6 == 60)
                {
                    s6 = 0;
                    m6 += 1;
                }
                if (m6 == 60)
                {
                    m6 = 0;
                    h6 += 1;
                }
                txtResult6.Text = string.Format("{0}:{1}:{2}", h6.ToString().PadLeft(2, '0'), m6.ToString().PadLeft(2, '0'), s6.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent7(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s7 += 1;
                if (s7 == 60)
                {
                    s7 = 0;
                    m7 += 1;
                }
                if (m7 == 60)
                {
                    m7 = 0;
                    h7 += 1;
                }
                txtResult7.Text = string.Format("{0}:{1}:{2}", h7.ToString().PadLeft(2, '0'), m7.ToString().PadLeft(2, '0'), s7.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent8(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s8 += 1;
                if (s8 == 60)
                {
                    s8 = 0;
                    m8 += 1;
                }
                if (m8 == 60)
                {
                    m8 = 0;
                    h8 += 1;
                }
                txtResult8.Text = string.Format("{0}:{1}:{2}", h8.ToString().PadLeft(2, '0'), m8.ToString().PadLeft(2, '0'), s8.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent9(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s9 += 1;
                if (s3 == 60)
                {
                    s9 = 0;
                    m9 += 1;
                }
                if (m9 == 60)
                {
                    m9 = 0;
                    h9 += 1;
                }
                txtResult9.Text = string.Format("{0}:{1}:{2}", h9.ToString().PadLeft(2, '0'), m9.ToString().PadLeft(2, '0'), s9.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent10(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s10 += 1;
                if (s10 == 60)
                {
                    s10 = 0;
                    m10 += 1;
                }
                if (m10 == 60)
                {
                    m10 = 0;
                    h10 += 1;
                }
                txtResult10.Text = string.Format("{0}:{1}:{2}", h10.ToString().PadLeft(2, '0'), m10.ToString().PadLeft(2, '0'), s10.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent11(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s11 += 1;
                if (s11 == 60)
                {
                    s11 = 0;
                    m11 += 1;
                }
                if (m11 == 60)
                {
                    m11 = 0;
                    h11 += 1;
                }
                txtResult11.Text = string.Format("{0}:{1}:{2}", h11.ToString().PadLeft(2, '0'), m11.ToString().PadLeft(2, '0'), s11.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent12(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s12 += 1;
                if (s12 == 60)
                {
                    s12 = 0;
                    m12 += 1;
                }
                if (m12 == 60)
                {
                    m12 = 0;
                    h12 += 1;
                }
                txtResult12.Text = string.Format("{0}:{1}:{2}", h12.ToString().PadLeft(2, '0'), m12.ToString().PadLeft(2, '0'), s12.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent13(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s13 += 1;
                if (s13 == 60)
                {
                    s13 = 0;
                    m13 += 1;
                }
                if (m13 == 60)
                {
                    m13 = 0;
                    h13 += 1;
                }
                txtResult13.Text = string.Format("{0}:{1}:{2}", h13.ToString().PadLeft(2, '0'), m13.ToString().PadLeft(2, '0'), s13.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent14(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s14 += 1;
                if (s14 == 60)
                {
                    s14 = 0;
                    m14 += 1;
                }
                if (m14 == 60)
                {
                    m14 = 0;
                    h14 += 1;
                }
                txtResult14.Text = string.Format("{0}:{1}:{2}", h14.ToString().PadLeft(2, '0'), m14.ToString().PadLeft(2, '0'), s14.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent15(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s15 += 1;
                if (s15 == 60)
                {
                    s15 = 0;
                    m15 += 1;
                }
                if (m15 == 60)
                {
                    m15 = 0;
                    h15 += 1;
                }
                txtResult15.Text = string.Format("{0}:{1}:{2}", h15.ToString().PadLeft(2, '0'), m15.ToString().PadLeft(2, '0'), s15.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent16(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s16 += 1;
                if (s16 == 60)
                {
                    s16 = 0;
                    m16 += 1;
                }
                if (m16 == 60)
                {
                    m16 = 0;
                    h16 += 1;
                }
                txtResult16.Text = string.Format("{0}:{1}:{2}", h16.ToString().PadLeft(2, '0'), m16.ToString().PadLeft(2, '0'), s16.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent17(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s17 += 1;
                if (s17 == 60)
                {
                    s17 = 0;
                    m17 += 1;
                }
                if (m17 == 60)
                {
                    m17 = 0;
                    h17 += 1;
                }
                txtResult17.Text = string.Format("{0}:{1}:{2}", h17.ToString().PadLeft(2, '0'), m17.ToString().PadLeft(2, '0'), s17.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent18(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s18 += 1;
                if (s18 == 60)
                {
                    s18 = 0;
                    m18 += 1;
                }
                if (m18 == 60)
                {
                    m18 = 0;
                    h18 += 1;
                }
                txtResult18.Text = string.Format("{0}:{1}:{2}", h18.ToString().PadLeft(2, '0'), m18.ToString().PadLeft(2, '0'), s18.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent19(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s19 += 1;
                if (s19 == 60)
                {
                    s19 = 0;
                    m19 += 1;
                }
                if (m19 == 60)
                {
                    m19 = 0;
                    h19 += 1;
                }
                txtResult19.Text = string.Format("{0}:{1}:{2}", h19.ToString().PadLeft(2, '0'), m19.ToString().PadLeft(2, '0'), s19.ToString().PadLeft(2, '0'));


            }));
        }
        private void OnTimeEvent20(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s20 += 1;
                if (s20 == 60)
                {
                    s20 = 0;
                    m20 += 1;
                }
                if (m20 == 60)
                {
                    m20 = 0;
                    h20 += 1;
                }
                txtResult20.Text = string.Format("{0}:{1}:{2}", h20.ToString().PadLeft(2, '0'), m20.ToString().PadLeft(2, '0'), s20.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent21(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s21 += 1;
                if (s21 == 60)
                {
                    s21 = 0;
                    m21 += 1;
                }
                if (m21 == 60)
                {
                    m21 = 0;
                    h21 += 1;
                }
                txtResult21.Text = string.Format("{0}:{1}:{2}", h21.ToString().PadLeft(2, '0'), m21.ToString().PadLeft(2, '0'), s21.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent22(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s22 += 1;
                if (s22 == 60)
                {
                    s22 = 0;
                    m22 += 1;
                }
                if (m22 == 60)
                {
                    m22 = 0;
                    h22 += 1;
                }
                txtResult22.Text = string.Format("{0}:{1}:{2}", h22.ToString().PadLeft(2, '0'), m2.ToString().PadLeft(2, '0'), s22.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent23(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s23 += 1;
                if (s23 == 60)
                {
                    s23 = 0;
                    m23 += 1;
                }
                if (m23 == 60)
                {
                    m23 = 0;
                    h23 += 1;
                }
                txtResult23.Text = string.Format("{0}:{1}:{2}", h23.ToString().PadLeft(2, '0'), m23.ToString().PadLeft(2, '0'), s23.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent24(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s24 += 1;
                if (s24 == 60)
                {
                    s24 = 0;
                    m24 += 1;
                }
                if (m24 == 60)
                {
                    m24 = 0;
                    h24 += 1;
                }
                txtResult24.Text = string.Format("{0}:{1}:{2}", h24.ToString().PadLeft(2, '0'), m24.ToString().PadLeft(2, '0'), s24.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent25(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s25 += 1;
                if (s25 == 60)
                {
                    s25 = 0;
                    m25 += 1;
                }
                if (m25 == 60)
                {
                    m25 = 0;
                    h25 += 1;
                }
                txtResult25.Text = string.Format("{0}:{1}:{2}", h25.ToString().PadLeft(2, '0'), m25.ToString().PadLeft(2, '0'), s25.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent26(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s26 += 1;
                if (s26 == 60)
                {
                    s26 = 0;
                    m26 += 1;
                }
                if (m26 == 60)
                {
                    m26 = 0;
                    h26 += 1;
                }
                txtResult26.Text = string.Format("{0}:{1}:{2}", h26.ToString().PadLeft(2, '0'), m26.ToString().PadLeft(2, '0'), s26.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent27(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s27 += 1;
                if (s27 == 60)
                {
                    s27 = 0;
                    m27 += 1;
                }
                if (m27 == 60)
                {
                    m27 = 0;
                    h27 += 1;
                }
                txtResult27.Text = string.Format("{0}:{1}:{2}", h27.ToString().PadLeft(2, '0'), m27.ToString().PadLeft(2, '0'), s27.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent28(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s28 += 1;
                if (s8 == 60)
                {
                    s28 = 0;
                    m28 += 1;
                }
                if (m28 == 60)
                {
                    m28 = 0;
                    h28 += 1;
                }
                txtResult28.Text = string.Format("{0}:{1}:{2}", h28.ToString().PadLeft(2, '0'), m28.ToString().PadLeft(2, '0'), s28.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent29(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s29 += 1;
                if (s29 == 60)
                {
                    s29 = 0;
                    m29 += 1;
                }
                if (m29 == 60)
                {
                    m29 = 0;
                    h29 += 1;
                }
                txtResult29.Text = string.Format("{0}:{1}:{2}", h29.ToString().PadLeft(2, '0'), m29.ToString().PadLeft(2, '0'), s29.ToString().PadLeft(2, '0'));

            }));
        }
        private void OnTimeEvent30(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s30 += 1;
                if (s30 == 60)
                {
                    s30 = 0;
                    m30 += 1;
                }
                if (m30 == 60)
                {
                    m30 = 0;
                    h30 += 1;
                }
                txtResult30.Text = string.Format("{0}:{1}:{2}", h30.ToString().PadLeft(2, '0'), m30.ToString().PadLeft(2, '0'), s30.ToString().PadLeft(2, '0'));

            }));
        }

        #endregion
        private void fComputerManager_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            t1.Stop(); t2.Stop(); t3.Stop(); t4.Stop(); t5.Stop(); t6.Stop(); t7.Stop(); t8.Stop(); t9.Stop(); t10.Stop(); t11.Stop(); t12.Stop(); t13.Stop(); t14.Stop(); t15.Stop(); t16.Stop(); t17.Stop(); t18.Stop(); t19.Stop(); t20.Stop(); t21.Stop(); t22.Stop(); t23.Stop(); t24.Stop(); t25.Stop(); t26.Stop(); t27.Stop(); t28.Stop(); t29.Stop(); t30.Stop();
            Application.DoEvents();

        }

        private void btnRestart30_Click(object sender, EventArgs e)
        {
            txtResult30.Text = "00:00:00";
            h30 = 0;
            m30 = 0;
            s30 = 0;
        }

        private void btnStop21_Click(object sender, EventArgs e)
        {
            t21.Stop();
        }

        private void btnStart21_Click(object sender, EventArgs e)
        {
            t21.Start();
        }

        private void btnRestart11_Click(object sender, EventArgs e)
        {
            txtResult11.Text = "00:00:00";
            h11 = 0;
            m11 = 0;
            s11 = 0;
        }

        private void btnStop11_Click(object sender, EventArgs e)
        {
            t11.Stop();
        }

        private void btnStart11_Click(object sender, EventArgs e)
        {
            t11.Start();
        }

        private void btnRestart1_Click(object sender, EventArgs e)
        {
            txtResult1.Text = "00:00:00";
            h1 = 0;
            m1 = 0;
            s1 = 0;
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            t1.Stop();
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            t1.Start();
        }

        private void btnRestart22_Click(object sender, EventArgs e)
        {
            txtResult2.Text = "00:00:00";
            h22 = 0;
            m22 = 0;
            s22 = 0;
        }

        private void btnStop22_Click(object sender, EventArgs e)
        {
            t22.Stop();
        }

        private void btnStart22_Click(object sender, EventArgs e)
        {
            t22.Start();
        }

        private void btnRestart12_Click(object sender, EventArgs e)
        {
            txtResult12.Text = "00:00:00";
            h12 = 0;
            m12 = 0;
            s12 = 0;
        }

        private void btnStop12_Click(object sender, EventArgs e)
        {
            t12.Stop();
        }

        private void btnStart12_Click(object sender, EventArgs e)
        {
            t12.Start();
        }

        private void btnRestart2_Click(object sender, EventArgs e)
        {
            txtResult2.Text = "00:00:00";
            h2 = 0;
            m2 = 0;
            s2 = 0;

        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            t2.Stop();
        }

        private void btnStart2_Click(object sender, EventArgs e)
        {
            t2.Start();

        }

        private void btnRestart23_Click(object sender, EventArgs e)
        {
            txtResult23.Text = "00:00:00";
            h23 = 0;
            m23 = 0;
            s23 = 0;
        }

        private void btnStop23_Click(object sender, EventArgs e)
        {
            t23.Stop();
        }

        private void btnStart23_Click(object sender, EventArgs e)
        {
            t23.Start();
        }

        private void btnRestart13_Click(object sender, EventArgs e)
        {
            txtResult13.Text = "00:00:00";
            h13 = 0;
            m13 = 0;
            s13 = 0;
        }

        private void btnStop13_Click(object sender, EventArgs e)
        {
            t13.Stop();
        }

        private void btnStart13_Click(object sender, EventArgs e)
        {
            t13.Start();
        }

        private void btnRestart3_Click(object sender, EventArgs e)
        {
            txtResult3.Text = "00:00:00";
            h3 = 0;
            m3 = 0;
            s3 = 0;
        }

        private void btnStop3_Click(object sender, EventArgs e)
        {
            t3.Stop();  
        }

        private void btnStart3_Click(object sender, EventArgs e)
        {
            t3.Start();
        }

        private void btnRestart24_Click(object sender, EventArgs e)
        {
            txtResult24.Text = "00:00:00";
            h24 = 0;
            m24 = 0;
            s24 = 0;
        }

        private void btnStop24_Click(object sender, EventArgs e)
        {
            t24.Stop();
        }

        private void btnStart24_Click(object sender, EventArgs e)
        {
            t24.Start();
        }

        private void btnRestart14_Click(object sender, EventArgs e)
        {
            txtResult14.Text = "00:00:00";
            h14 = 0;
            m14 = 0;
            s14 = 0;
        }

        private void btnStop14_Click(object sender, EventArgs e)
        {
            t14.Stop();
        }

        private void btnStart14_Click(object sender, EventArgs e)
        {
            t14.Start();
        }

        private void btnRestart4_Click(object sender, EventArgs e)
        {
            txtResult4.Text = "00:00:00";
            h4 = 0;
            m4 = 0;
            s4 = 0;
        }

        private void btnStop4_Click(object sender, EventArgs e)
        {
            t4.Stop();
        }

        private void btnStart4_Click(object sender, EventArgs e)
        {
            t4.Start();
        }

        private void btnRestart25_Click(object sender, EventArgs e)
        {
            txtResult25.Text = "00:00:00";
            h25 = 0;
            m25 = 0;
            s25 = 0;
        }

        private void btnStop25_Click(object sender, EventArgs e)
        {
            t25.Stop();
        }

        private void btnStart25_Click(object sender, EventArgs e)
        {
            t25.Start();
        }

        private void btnRestart15_Click(object sender, EventArgs e)
        {
            txtResult15.Text = "00:00:00";
            h15 = 0;
            m15 = 0;
            s15 = 0;
        }

        private void btnStop15_Click(object sender, EventArgs e)
        {
            t15.Stop();
        }

        private void btnStart15_Click(object sender, EventArgs e)
        {
            t15.Start();
        }

        private void btnRestart5_Click(object sender, EventArgs e)
        {
            txtResult5.Text = "00:00:00";
            h5 = 0;
            m5 = 0;
            s5 = 0;
        }

        private void btnStop5_Click(object sender, EventArgs e)
        {
            t5.Stop();
        }

        private void btnStart5_Click(object sender, EventArgs e)
        {
            t5.Start();
        }

        private void btnRestart26_Click(object sender, EventArgs e)
        {
            txtResult26.Text = "00:00:00";
            h26 = 0;
            m26 = 0;
            s26 = 0;
        }

        private void btnStop26_Click(object sender, EventArgs e)
        {
            t26.Stop();
        }

        private void btnStart26_Click(object sender, EventArgs e)
        {
            t26.Start();
        }

        private void btnRestart16_Click(object sender, EventArgs e)
        {
            txtResult16.Text = "00:00:00";
            h16 = 0;
            m16 = 0;
            s16 = 0;
        }

        private void btnStop16_Click(object sender, EventArgs e)
        {
            t16.Stop();
        }

        private void btnStart16_Click(object sender, EventArgs e)
        {
            t16.Start();
        }

        private void btnRestart6_Click(object sender, EventArgs e)
        {
            txtResult6.Text = "00:00:00";
            h6 = 0;
            m6 = 0;
            s6 = 0;
        }

        private void btnStop6_Click(object sender, EventArgs e)
        {
            t6.Stop();
        }

        private void btnStart6_Click(object sender, EventArgs e)
        {
            t6.Start();
        }

        private void btnRestart27_Click(object sender, EventArgs e)
        {
            txtResult27.Text = "00:00:00";
            h27 = 0;
            m27 = 0;
            s27 = 0;
        }

        private void btnStop27_Click(object sender, EventArgs e)
        {
            t27.Stop();
        }

        private void btnStart27_Click(object sender, EventArgs e)
        {
            t27.Start();
        }

        private void btnRestart17_Click(object sender, EventArgs e)
        {
            txtResult17.Text = "00:00:00";
            h17 = 0;
            m17 = 0;
            s17 = 0;
        }

        private void btnStop17_Click(object sender, EventArgs e)
        {
            t17.Stop();
        }

        private void btnStart17_Click(object sender, EventArgs e)
        {
            t17.Start();
        }

        private void btnRestart7_Click(object sender, EventArgs e)
        {
            txtResult7.Text = "00:00:00";
            h7 = 0;
            m7 = 0;
            s7 = 0;
        }

        private void btnStop7_Click(object sender, EventArgs e)
        {
            t7.Stop();  
        }

        private void btnStart7_Click(object sender, EventArgs e)
        {
            t7.Start();
        }

        private void btnRestart28_Click(object sender, EventArgs e)
        {
            txtResult28.Text = "00:00:00";
            h28 = 0;
            m28 = 0;
            s28 = 0;
        }

        private void btnStop28_Click(object sender, EventArgs e)
        {
            t28.Stop();
        }

        private void btnStart28_Click(object sender, EventArgs e)
        {
            t28.Start();
        }

        private void btnRestart18_Click(object sender, EventArgs e)
        {
            txtResult18.Text = "00:00:00";
            h18 = 0;
            m18 = 0;
            s18 = 0;
        }

        private void btnStop18_Click(object sender, EventArgs e)
        {
            t18.Stop();
        }

        private void btnStart18_Click(object sender, EventArgs e)
        {
            t18.Start();
        }

        private void btnRestart8_Click(object sender, EventArgs e)
        {
            txtResult8.Text = "00:00:00";
            h8 = 0;
            m8 = 0;
            s8 = 0;
        }

        private void btnStop8_Click(object sender, EventArgs e)
        {
            t8.Stop();
        }

        private void btnStart8_Click(object sender, EventArgs e)
        {
            t8.Start();
        }

        private void btnRestart29_Click(object sender, EventArgs e)
        {
            txtResult29.Text = "00:00:00";
            h29 = 0;
            m29 = 0;
            s29 = 0;
        }

        private void btnStop29_Click(object sender, EventArgs e)
        {
            t29.Stop();
        }

        private void btnStart29_Click(object sender, EventArgs e)
        {
            t29.Start();
        }

        private void btnRestart19_Click(object sender, EventArgs e)
        {
            txtResult19.Text = "00:00:00";
            h19 = 0;
            m19 = 0;
            s19 = 0;
        }

        private void btnStop19_Click(object sender, EventArgs e)
        {
            t19.Stop();
        }

        private void btnStart19_Click(object sender, EventArgs e)
        {
            t19.Start();
        }

        private void btnRestart9_Click(object sender, EventArgs e)
        {
            txtResult9.Text = "00:00:00";
            h9 = 0;
            m9 = 0;
            s9 = 0;
        }

        private void btnStop9_Click(object sender, EventArgs e)
        {
            t9.Stop();
        }

        private void btnStart9_Click(object sender, EventArgs e)
        {
            t9.Start();
        }



        private void btnRestart21_Click(object sender, EventArgs e)
        {
            txtResult21.Text = "00:00:00";
            h21 = 0;
            m21 = 0;
            s21 = 0;
        }

        private void btnStop30_Click(object sender, EventArgs e)
        {
            t21.Stop();
        }

        private void btnStart30_Click(object sender, EventArgs e)
        {
            t21.Start();
        }

        private void btnRestart20_Click(object sender, EventArgs e)
        {
            txtResult20.Text = "00:00:00";
            h20 = 0;
            m20 = 0;
            s20 = 0;
        }

        private void btnStop20_Click(object sender, EventArgs e)
        {
            t20.Stop();
        }

        private void btnStart20_Click(object sender, EventArgs e)
        {
            t20.Start();
        }

        private void btnRestart10_Click(object sender, EventArgs e)
        {
            txtResult10.Text = "00:00:00";
            h10 = 0;
            m10 = 0;
            s10 = 0;
        }

        private void btnStop10_Click(object sender, EventArgs e)
        {
            t10.Stop();
        }

        private void btnStart10_Click(object sender, EventArgs e)
        {
            t10.Start();
        }
        #endregion
    }
}
