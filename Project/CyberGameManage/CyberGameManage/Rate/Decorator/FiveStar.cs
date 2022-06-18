using CyberGameManage.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Rate.Decorator
{
    class FiveStar : RateDecorator
    {
        public FiveStar(IRate inner) : base(inner)
        {
        }

        public override string Feedback()
        {
            return base.Feedback() + "You are a great customer, so are we";
        }
    }
}
