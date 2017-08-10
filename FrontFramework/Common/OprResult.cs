using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Common
{
    class OprResult
    {
        public OprResult()
        {
            this.result = true;
            this.info = "";
        }
        public OprResult(Boolean result)
        {
            this.result = result;
            this.info = "";
        }
        public OprResult(Boolean result, String info)
        {
            this.result = result;
            this.info = info;
        }
        public Boolean result;
        public String info;
    }
}
