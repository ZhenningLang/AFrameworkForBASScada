using Common;
using Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Translation.XMLBasedLanguage
{
    public class TranslatorBasedOnXML: ITranslator
    {
        private static TranslatorBasedOnXML translator = new TranslatorBasedOnXML();
        public static TranslatorBasedOnXML getTranslator()
        {
            return translator;
        }

        private LanguageEnum from { set; get; }
        private LanguageEnum to { set; get; }
        private XmlProcessor processor = XmlProcessor.getInstance();

        /// <summary>
        /// 设定从哪种语言翻译到哪种
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void setLanguage(LanguageEnum from, LanguageEnum to)
        {
            this.from = from;
            this.to = to;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("lang.xml");
                xmlDoc.SelectSingleNode("//from").InnerText = from.ToString().ToUpper().Trim();
                xmlDoc.SelectSingleNode("//to").InnerText = to.ToString().ToUpper().Trim();
                xmlDoc.Save("lang.xml");
            }
            catch (Exception er)
            { 
                Console.WriteLine(er.ToString()); 
            }
        }
        public LanguageEnum getLanguageFrom() 
        {
            return this.from;
        }
        public LanguageEnum getLanguageTo()
        {
            return this.to;
        }
        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public String getTranslation(String str)
        {
            return processor.searchByLanguage(str, from, to).info;
        }

        public String getComponentTranslation(String id)
        {
            return processor.searchByID(id, to).info;
        }
        public String getComponentTranslation(String[] ids, String delimiter = " ")
        {
            String rVal = "";
            if (to != LanguageEnum.CHINESE)
            {
                foreach (String id in ids)
                {
                    rVal += getComponentTranslation(id).ToLower();
                    rVal += delimiter;
                }
                rVal = rVal.Substring(0, 1).ToUpper() + rVal.Substring(1, rVal.Length - delimiter.Length);
            }
            else
            {
                foreach (String id in ids)
                {
                    rVal += getComponentTranslation(id);
                }
            }
            return rVal;
        }
        public Dictionary<String, Dictionary<LanguageEnum, String>> getAllWords() 
        {
            Dictionary<String, Dictionary<LanguageEnum, String>> rVal = new Dictionary<String, Dictionary<LanguageEnum, String>>();
            XmlNodeList nodeList = processor.getXmlNodeList();
            foreach(XmlElement node in nodeList)
            {
                Dictionary<LanguageEnum, String> temp = new Dictionary<LanguageEnum, string>();
                temp.Add(LanguageEnum.ENGLISH, node.SelectSingleNode("english").InnerText.Trim());
                temp.Add(LanguageEnum.CHINESE, node.SelectSingleNode("chinese").InnerText.Trim());
                rVal.Add(node.GetAttribute("id"), temp);
            }
            return rVal;

        }
        public OptResult addNewWordToDict(String id, Dictionary<LanguageEnum, String> dic) 
        {
            return processor.addItem(id, dic);
        }
        public OptResult delWordFromDict(String id)
        {
            return processor.delItem(id);
        }
        public void recoveryFromHistory()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("lang.xml");
                var historyFrom = xmlDoc.SelectSingleNode("//from").InnerText.ToUpper().Trim();
                var historyTo = xmlDoc.SelectSingleNode("//to").InnerText.ToUpper().Trim();
                this.from = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), historyFrom);
                this.to = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), historyTo);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }
        }
        /// <summary>
        /// 构造函数，读取xml文档
        /// </summary>
        private TranslatorBasedOnXML() 
        {
        }
       
    }
}
