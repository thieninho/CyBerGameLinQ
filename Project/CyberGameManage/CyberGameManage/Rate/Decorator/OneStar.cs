using CyberGameManage.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Rate.Decorator
{
    class OneStar : RateDecorator
    {
        public OneStar(IRate inner) : base(inner)
        {
        }

        public override string Feedback()
        {
            return base.Feedback() + "So sorry for your bad experience, What do we need to change?";
        }
    }
}
