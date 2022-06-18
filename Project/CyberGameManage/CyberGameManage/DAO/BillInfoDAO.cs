using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }

        public void DeleteBillInfoByOrderID(int id)
        {
            var bill = DataProvider.Instance.db().BillInfos.Single(b => b.idOrder == id);
            DataProvider.Instance.db().BillInfos.DeleteOnSubmit(bill);
            DataProvider.Instance.db().SubmitChanges();
        }

        public List<BillInfoDTO> GetListBillInfo(int id)
        {
            var billList = (from bill in DataProvider.Instance.db().BillInfos
                           where bill.idBill == id
                           select new BillInfoDTO(bill.id, bill.idBill, bill.idOrder, bill.count)).ToList();
            return billList;
        }
        public void InsertBillInfo(int idBill, int idOrder, int count)
        {
            DataProvider.Instance.ExecuteNonQuery(" USP_InsertBillInfo @idBill , @idOrder , @count", new object[] { idBill, idOrder, count });
        }
    }
}
