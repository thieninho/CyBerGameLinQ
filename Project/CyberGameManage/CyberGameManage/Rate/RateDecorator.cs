using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberGameManage.Decorator
{
    public abstract class RateDecorator : IRate
    {
        private IRate _raTe;
        protected RateDecorator(IRate inner)
        {
            _raTe = inner;
        }

        public virtual string Feedback()
        {
            return _raTe.Feedback();
        }
    }
}
