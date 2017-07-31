using FrontFramework.Enums;
using FrontFramework.Lang.XMLBasedLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontFramework.Tests
{
    class LanguageTester
    {
        public static void startTester() 
        {
            Console.WriteLine("LanguageTester starts");
            XmlProcessor processor = XmlProcessor.getInstance();
            String id = "test";

            Dictionary<LanguageEnum, String> dic = new Dictionary<LanguageEnum, string>();
            //dic.Add(LanguageEnum.CHINESE, "の");
            //processor.addItem(id, dic);
            //processor.editItem(id, dic);

            Console.WriteLine(processor.searchByID("dicEditor", LanguageEnum.CHINESE).result);
            Console.WriteLine(processor.searchByID("dicEditor", LanguageEnum.CHINESE).info);
            Console.WriteLine(processor.searchByID("add", LanguageEnum.ENGLISH).result);
            Console.WriteLine(processor.searchByID("add", LanguageEnum.ENGLISH).info);

            Console.WriteLine(processor.searchByLanguage("词典编辑", LanguageEnum.CHINESE, LanguageEnum.ENGLISH).result);
            Console.WriteLine(processor.searchByLanguage("词典编辑", LanguageEnum.CHINESE, LanguageEnum.ENGLISH).info);
            Console.WriteLine(processor.searchByLanguage("Add", LanguageEnum.ENGLISH, LanguageEnum.CHINESE).result);
            Console.WriteLine(processor.searchByLanguage("Add", LanguageEnum.ENGLISH, LanguageEnum.CHINESE).info);
            Console.WriteLine(processor.searchByLanguage("aaa", LanguageEnum.ENGLISH, LanguageEnum.CHINESE).result);
            Console.WriteLine(processor.searchByLanguage("aaa", LanguageEnum.ENGLISH, LanguageEnum.CHINESE).info);
        }
    }
}
