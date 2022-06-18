using CyberGameManage.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Rate.Decorator
{
    class FourStar : RateDecorator
    {
        public FourStar(IRate inner) : base(inner)
        {
        }

        public override string Feedback()
        {
            return base.Feedback() + "Cyber Game are happy to serve you";
        }
    }
}
