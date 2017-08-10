using FrontFramework.Utils.Spring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Utils
{
    class PropUtilFactory
    {
        private static PropertyUtilInterface propUtil = null;
        static PropUtilFactory()
        {
            PropUtilFactory.propUtil = (PropertyUtilInterface)SpringUtil.getContext().GetObject("PropUtil");
        }
        public static PropertyUtilInterface getPropUtil()
        {
            return PropUtilFactory.propUtil;
        }

    }
}