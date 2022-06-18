using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DTO
{
    public class BillInfoDTO
    {
        public BillInfoDTO(int id, int billID, int orderID, int count)
        {
            this.ID = id;
            this.BillID = billID;
            this.OrderId = orderID;
            this.Count = count;
        }

        public BillInfoDTO(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idbill"];
            this.OrderId = (int)row["idorder"];
            this.Count = (int)row["count"];
        }

        private int orderId;

        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public int OrderId 
        { 
            get { return orderId; }
            set { orderId = value; } 
        }
        private int billID;
        public int BillID
        {
            get { return billID; }
            set { billID = value; }
        }
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }
}
