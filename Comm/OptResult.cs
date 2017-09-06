using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class OptResult
    {
        public OptResult()
        {
            this.result = true;
            this.info = "";
        }
        public OptResult(Boolean result)
        {
            this.result = result;
            this.info = "";
        }
        public OptResult(Boolean result, String info)
        {
            this.result = result;
            this.info = info;
        }
        public Boolean result;
        public String info;
    }
}
