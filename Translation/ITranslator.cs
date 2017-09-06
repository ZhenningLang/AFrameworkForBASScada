using Common;
using Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public interface ITranslator
    {
        void setLanguage(LanguageEnum from, LanguageEnum to);
        LanguageEnum getLanguageFrom();
        LanguageEnum getLanguageTo();
        String getTranslation(String str);
        /// <summary>
        /// 获取控件的翻译
        /// </summary>
        /// <param name="id"> 控件翻译的 id </param>
        /// <returns></returns>
        String getComponentTranslation(String id);
        String getComponentTranslation(String[] ids, String delimiter = " ");
        Dictionary<String, Dictionary<LanguageEnum, String>> getAllWords();
        OptResult addNewWordToDict(String id, Dictionary<LanguageEnum, String> dic);
        OptResult delWordFromDict(String id);
        void recoveryFromHistory();
    }
}
