using CyberGameManage.DAO;
using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CyberGameManage
{
    public partial class fAdmin : Form
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
        BindingSource orderList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource computerList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Loaddd();
        }
        public AccountDTO loginAccount;
        List<OrderDTO> SearchOrderByName(string name)
        {
            List<OrderDTO> listOrder = OrderDAO.Instance.SearchOrderByName(name);

            return listOrder;
        }
        void Loaddd()
        {

            dtgvOrder.DataSource = orderList;
            
            dtgvAccount.DataSource = accountList;
            //dtgvComputer.DataSource = computerList;
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            //LoadAccountList();
            LoadListOrder();
            LoadAccount();
            AddOrderBinding();
            AddAccountBinding();
            //AddComputerBinding();
            LoadCategoryIntoCombobox(cbOrderCategory);
            LoadComputer();
        }
        #region methods

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        //void AddComputerBinding()
        //{
        //    txbComputer.DataBindings.Add(new Binding("Text", dtgvComputer.DataSource, "Name", true, DataSourceUpdateMode.Never));
        //    txbStatus.DataBindings.Add(new Binding("Text", dtgvComputer.DataSource, "Status", true, DataSourceUpdateMode.Never));

        //}
        #endregion

        void AddOrderBinding()
        {
            txbOrderName.DataBindings.Add(new Binding("Text", dtgvOrder.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbOrderID.DataBindings.Add(new Binding("Text", dtgvOrder.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmOrderPrice.DataBindings.Add(new Binding("Value", dtgvOrder.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadListOrder()
        {
            orderList.DataSource = OrderDAO.Instance.GetListOrder();
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadComputer()
        {
            computerList.DataSource = ComputerDAO.Instance.LoadComputerList();
        }
        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private event EventHandler insertOrder;
        public event EventHandler InsertOrder
        {
            add { insertOrder += value; }
            remove { insertOrder -= value; }
        }

        private event EventHandler deleteOrder;
        public event EventHandler DeleteOrder
        {
            add { deleteOrder += value; }
            remove { deleteOrder -= value; }
        }

        private event EventHandler updateOrder;
        public event EventHandler UpdateOrder
        {
            add { updateOrder += value; }
            remove { updateOrder -= value; }
        }
    

        private void btnShowOrder_Click(object sender, EventArgs e)
        {
            LoadListOrder();

        }

        private void txbOrderID_TextChanged(object sender, EventArgs e)
        {
            if (dtgvOrder.SelectedCells.Count > 0)
            {
                int id = (int)dtgvOrder.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                CategoryDTO cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                cbOrderCategory.SelectedItem = cateogory;

                int index = -1;
                int i = 0;
                foreach (CategoryDTO item in cbOrderCategory.Items)
                {
                    if (item.ID == cateogory.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cbOrderCategory.SelectedIndex = index;
            }
        }


        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbOrderID.Text);

            if (OrderDAO.Instance.DeleteOrder(id))
            {
                MessageBox.Show("Delete Successfully");
                LoadListOrder();
                if (deleteOrder != null)
                    deleteOrder(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            string name = txbOrderName.Text;
            int categoryID = (cbOrderCategory.SelectedItem as CategoryDTO).ID;
            float price = (float)nmOrderPrice.Value;
            int id = Convert.ToInt32(txbOrderID.Text);

            if (OrderDAO.Instance.UpdateOrder(id, name, categoryID, price))
            {
                MessageBox.Show("Update Successfully");
                LoadListOrder();
                if (updateOrder != null)
                    updateOrder(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            string name = txbOrderName.Text;
            int categoryID = (cbOrderCategory.SelectedItem as CategoryDTO).ID;
            float price = (float)nmOrderPrice.Value;

            if (OrderDAO.Instance.InsertOrder(name, categoryID, price))
            {
                MessageBox.Show("Add Successfully!");
                LoadListOrder();
                if (insertOrder != null)
                    insertOrder(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Fail!");
            }
        }
        #endregion


        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Add Successfully!");
            }
            else
            {
                MessageBox.Show("Fail!");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Update Successfully!");
            }
            else
            {
                MessageBox.Show("Fail!");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Are you sure?");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Delete Successfully!");
            }
            else
            {
                MessageBox.Show("Fail!");
            }

            LoadAccount();
        }
        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Reset Successfully");
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }
        private void btnSearchOrder_Click(object sender, EventArgs e)
        {
            orderList.DataSource = SearchOrderByName(txbSearchOrderName.Text);
        }

       
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPass(userName);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }


        private void ptb_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            this.uSP_GetListBillByDateForReportTableAdapter.Fill(this.dataSet1.USP_GetListBillByDateForReport, dtpkFromDate.Value, dtpkToDate.Value);

            this.reportViewer1.RefreshReport();
        }
    }
}
