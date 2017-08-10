using FrontFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Utils
{
    interface PropertyUtilInterface
    {
        OprResult loadProperty(Object obj);
        OprResult getStrProp(String name);
        OprResult setStrProp(String name, String value);
        /// <summary>
        /// value - <propName, propVal>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Dictionary<String, Dictionary<String, String>> getListStrProp(String name);
        Dictionary<String, Dictionary<String, String>> getListStrProp(String name, String propName, String propVal);
    }
}
