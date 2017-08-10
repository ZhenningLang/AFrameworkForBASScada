using FrontFramework.Utils.Spring;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Language
{
    class TranslatorFactory
    {
        private static Translator translator = null;
        static TranslatorFactory() 
        {
            TranslatorFactory.translator = (Translator)SpringUtil.getContext().GetObject("Translator");
        }
        public static Translator getTranslator() 
        {
            return TranslatorFactory.translator;
        }
    }
}
