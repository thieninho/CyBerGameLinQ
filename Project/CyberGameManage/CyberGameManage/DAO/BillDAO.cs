using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DAO
{
    public class BillDAO
    {
        private static BillDAO instance = new BillDAO();
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }
        /// <summary>
        /// Thành công: bill ID
        /// thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckBillIDByComputerID(int id)
        {
            //return (int)DataProvider.Instance.ExecuteScalar("");
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idComputer = " + id + " AND status = 0");
            if (data.Rows.Count > 0)
            {
                BillDTO bill = new BillDTO(data.Rows[0]);
                return bill.ID;
            }

            return -1;
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            var bill = DataProvider.Instance.db().Bills.Single(b => b.id == id);
            bill.DateCheckOut = DateTime.Now;
            bill.status = 1;
            bill.discount = discount;
            bill.totalPrice = totalPrice;
            DataProvider.Instance.db().SubmitChanges();
        }

        public void InsertBill(int id)
        {
            var bill = new Bill
            {
                DateCheckIn = DateTime.Now,
                DateCheckOut = DateTime.Now,
                idComputer = id,
                status = 0,
                discount = 0,
            };
            DataProvider.Instance.db().Bills.InsertOnSubmit(bill);
            DataProvider.Instance.db().SubmitChanges();
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }
        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar(" SELECT MAX(id) FROM dbo.Bill ");
            }
            catch
            {
                return 1;
            }
        }
    }
} 
