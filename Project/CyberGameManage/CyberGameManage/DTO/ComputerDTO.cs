using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.DTO
{
    public class ComputerDTO
    {
        private string status;
        private string name;
        private int iD;

        public ComputerDTO(int iD, string name, string status)
        {
            ID = iD;
            Name = name;
            Status = status;
        }

        public ComputerDTO (DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Status = row["status"].ToString();
        }
        public int ID 
        { 
            get { return iD; }
            set { iD = value; } 
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
