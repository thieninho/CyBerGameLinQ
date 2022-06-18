using CyberGameManage.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Rate.Decorator
{
    class TwoStar : RateDecorator
    {
        public TwoStar(IRate inner) : base(inner)
        {
        }

        public override string Feedback()
        {
            return base.Feedback() + "What problem do you have? Don't be sad with CyberGame";
        }
    }
}
