using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Decorator
{
    public class IFeedBack : IRate
    {
        public string Feedback()
        {
            return "Thank you for choosing our Cyber ​​Game | ";
        }
    }
}
