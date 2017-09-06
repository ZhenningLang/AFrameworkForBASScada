using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.FCMP
{
    public class FCMPCommunication: IComm
    {
        private static FCMPCommunication fcmpCommInstance = new FCMPCommunication();
        private FCMPCommunication() 
        {

        }
        public static FCMPCommunication getFCMPCommInstance() 
        {
            return fcmpCommInstance;
        }
    }
}
