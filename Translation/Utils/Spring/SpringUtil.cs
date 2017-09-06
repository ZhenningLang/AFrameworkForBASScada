using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Utils.Spring
{
    class SpringUtil
    {
        private static IApplicationContext ctx = null;
        static SpringUtil()
        {
            try
            {
                ctx = new XmlApplicationContext("resources//configs//springTrans.xml");
                foreach (String str in ctx.GetObjectDefinitionNames()) 
                {
                    Console.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static IApplicationContext getContext() 
        {
            return SpringUtil.ctx;
        }
    }
}
