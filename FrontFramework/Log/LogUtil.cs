using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "resources/configs/Log4Net.config", Watch = true)]
namespace FrontFramework.Log
{
    public class LogUtil
    {
        public static readonly log4net.ILog logFuncError = log4net.LogManager.GetLogger("logFuncError");
        public static readonly log4net.ILog logUserOper = log4net.LogManager.GetLogger("logUserOperation");
        private LogUtil(){}

        public static void writeFuncErrorLog(String error, Exception se)
        {
            logFuncError.Error(error, se);
        }

        public static void writeUserOperation(String userName, String oprName, Object[] inputParas, Object result)
        {
            String logText = "<td>User Name: " + userName + "</td>" +
                "<td>Operation Name: " + oprName + "</td>";
            logText += "<td>Parameters: ";
            foreach(Object obj in inputParas)
            {
                logText += " " + obj.ToString() + " ";
            }
            logText += "</td>";
            logText += "<td>Result: " + result.ToString() + "</td>";
            logUserOper.Info(logText);
        }
    }
 }