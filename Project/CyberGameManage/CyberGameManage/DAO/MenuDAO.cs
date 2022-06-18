using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CyberGameManage.DTO;

namespace CyberGameManage.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;
        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }
        private MenuDAO() { }
        public List<DTO.MenuDTO> GetListMenuByComputer(int id)
        {
            List<DTO.MenuDTO> listMenu = new List<DTO.MenuDTO>();
            string query = "SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Orderr AS f WHERE bi.idBill = b.id AND bi.idOrder = f.id AND b.status = 0 AND b.idComputer = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                DTO.MenuDTO menu = new DTO.MenuDTO(item);
                listMenu.Add(menu);
            }
            return listMenu;
        }
    }
}
