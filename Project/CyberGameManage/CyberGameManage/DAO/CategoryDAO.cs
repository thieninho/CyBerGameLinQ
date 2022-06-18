using CyberGameManage.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<CategoryDTO> GetListCategory()
        {
            var cateList = (from cate in DataProvider.Instance.db().OrderCategories
                           select new CategoryDTO(cate.id, cate.name)).ToList();
            return cateList;
        }
        public CategoryDTO GetCategoryByID(int id)
        {
            var cate = (from c in DataProvider.Instance.db().OrderCategories
                       where c.id == id
                       select new CategoryDTO(c.id, c.name)).FirstOrDefault();

            return cate;
        }
    }
}
