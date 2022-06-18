using CyberGameManage.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Rate.Decorator
{
    class ThreeStar : RateDecorator
    {
        public ThreeStar(IRate inner) : base(inner)
        {
        }

        public override string Feedback()
        {
            return base.Feedback() + "Next time remember to rate Cyber Game better";
        }
    }
}
