using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DAO
{
    public class OrderDAO
    {
        private static OrderDAO instance;

        public static OrderDAO Instance
        {
            get { if (instance == null) instance = new OrderDAO(); return OrderDAO.instance; }
            private set { OrderDAO.instance = value; }
        }

        private OrderDAO() { }

        public List<OrderDTO> GetOrderrByCategoryID(int id)
        {
            var listOrder = (from order in DataProvider.Instance.db().Orderrs
                            where order.idCategory == id
                            select new OrderDTO(order.id, order.name, order.idCategory, (float)order.price)).ToList();

            return listOrder;
        }
        public IQueryable<OrderDTO> GetListOrder()
        {
            var listOrder = from order in DataProvider.Instance.db().Orderrs
                             select new OrderDTO(order.id, order.name,order.idCategory, (float)order.price);
            return listOrder;
        }


        public bool InsertOrder(string name, int id, float price)
        {
            var food = new Orderr
            {
                name = name,
                idCategory = id,
                price = price,
            };
            DataProvider.Instance.db().Orderrs.InsertOnSubmit(food);
            DataProvider.Instance.db().SubmitChanges();
            return true;
        }
        public bool UpdateOrder(int idOrder, string name, int id, float price)
        {
            var food = DataProvider.Instance.db().Orderrs.Single(order => order.id == idOrder);
            food.idCategory = id;
            food.name = name;
            food.price = price;
            DataProvider.Instance.db().SubmitChanges();
            return true;
        }

        public bool DeleteOrder(int idOrder)
        {
            var food = DataProvider.Instance.db().Orderrs.Single(order => order.id == idOrder);
            DataProvider.Instance.db().Orderrs.DeleteOnSubmit(food);
            DataProvider.Instance.db().SubmitChanges();

            return true;
        }
        public List<OrderDTO> SearchOrderByName(string name)
        {
            List<OrderDTO> list = new List<OrderDTO>();

            string query = string.Format("SELECT * FROM dbo.Orderr WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                OrderDTO orderr = new OrderDTO(item);
                list.Add(orderr);
            }

            return list;
        }
    }
}
