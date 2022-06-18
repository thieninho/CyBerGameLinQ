using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DAO
{
    public class ComputerDAO
    {
        private static ComputerDAO instance;
        public static ComputerDAO Instance 
        { 
            get { if (instance == null) instance = new ComputerDAO();  return ComputerDAO.instance; }
            private set { ComputerDAO.instance = value; }
        }
        public static int ComputerWidth = 132;
        public static int ComputerHeight = 125;
        private ComputerDAO() { }

        public void SwitchComputer(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchComputer @idComputer1 , @idComputer2", new object[] { id1, id2 });
        }
        public List<ComputerDTO> LoadComputerList()
        {
            var computerList = (from computer in DataProvider.Instance.db().ComputerOrders
                               select new ComputerDTO(computer.id, computer.name, computer.status)).ToList();
            return computerList;
        }
        public bool InsertComputer(string name, string status)
        {
            var computer = new ComputerOrder
            {
                name = name,
                status = status,
            };
            DataProvider.Instance.db().ComputerOrders.InsertOnSubmit(computer);
            DataProvider.Instance.db().SubmitChanges();

            return true;
        }
        public bool UpdateComputer(string name, string status)
        {
            var computer = DataProvider.Instance.db().ComputerOrders.Single(com => com.name == name);
            computer.status = status;
            DataProvider.Instance.db().SubmitChanges();

            return true;
        }

        public bool DeleteComputer(string name)
        {
            var computer = DataProvider.Instance.db().ComputerOrders.Single(com => com.name == name);
            DataProvider.Instance.db().ComputerOrders.DeleteOnSubmit(computer);
            DataProvider.Instance.db().SubmitChanges();

            return true;
        }
    }
}
