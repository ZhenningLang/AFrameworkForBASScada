using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Help
{
    class OnlineHelper: IHelper
    {
        String helperUrl = "";
        public OnlineHelper() { }
        public void setHelperUrl(String helperUrl) 
        {
            this.helperUrl = helperUrl;
        }
        public void openHelper() 
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", helperUrl);
            }
            catch (Exception er)
            {
                FrontFramework.Log.LogUtil.writeFuncErrorLog("URL cannot be openned: " + helperUrl, er);
            }
        }
    }
}
