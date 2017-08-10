using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Help
{
    class DocHelper: IHelper
    {
        String helperFilePathName = "";
        public DocHelper() { }
        public void setHelperFilePathName(String helperFilePathName) 
        {
            this.helperFilePathName = helperFilePathName;
        }
        public void openHelper()
        {
            if (openFile(helperFilePathName)) 
            {
                return;
            }
            else if (openFile(helperFilePathName + ".txt"))
            {
                return;
            }
            else if (openFile(helperFilePathName + ".doc"))
            {
                return;
            }
            else if (openFile(helperFilePathName + ".docx"))
            {
                return;
            }
            else if (openFile(helperFilePathName + ".pdf"))
            {
                return;
            }
            else if (openFile(helperFilePathName + ".htm"))
            {
                return;
            }
            else if (openFile(helperFilePathName + ".html"))
            {
                return;
            }
            else 
            {
                FrontFramework.Log.LogUtil.writeFuncErrorLog("File cannot be openned: " + helperFilePathName, new Exception());
            }
        }

        public Boolean openFile(String fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
                return true;
            }
            catch (Exception er)
            {
                return false;
            }
        }

    }
}
