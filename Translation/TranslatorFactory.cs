using Translation.Utils.Spring;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public class TranslatorFactory
    {
        private static ITranslator translator = null;
        static TranslatorFactory() 
        {
            try
            {
                TranslatorFactory.translator = (ITranslator)SpringUtil.getContext().GetObject("Translator");
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static ITranslator getTranslator() 
        {
            return TranslatorFactory.translator;
        }
    }
}
