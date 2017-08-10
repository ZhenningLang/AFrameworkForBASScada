using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Utils.Spring
{
    class SpringUtil
    {
        private static IApplicationContext ctx = ContextRegistry.GetContext();
        public static IApplicationContext getContext() 
        {
            return SpringUtil.ctx;
        }
    }
}
