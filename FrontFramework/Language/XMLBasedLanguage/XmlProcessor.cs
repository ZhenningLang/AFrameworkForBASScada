using FrontFramework.Common;
using FrontFramework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FrontFramework.Lang.XMLBasedLanguage
{
    /// <summary>
    /// 帮助操作 XAML 文档，主要是为了操作 Language
    /// </summary>
    class XmlProcessor
    {
        ///////////////////////////////////////////////////////////////////////////////////// 单例
        private static XmlProcessor instance = new XmlProcessor();
        private XmlProcessor() 
        {
            try
            {
                // 打开文档
                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                xmlNodeList = xmlDoc.SelectNodes("//group");
                // 获取数据 allWords
                XmlNodeList tempList;
                foreach (XmlNode node in xmlNodeList) 
                {
                    tempList = node.ChildNodes;
                    foreach (XmlNode toBeAdded in tempList)
                    {
                        allWords.Add(toBeAdded.InnerText.Trim());
                    }
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                throw er;
            }

        }
        public static XmlProcessor getInstance()
        {
            return XmlProcessor.instance;
        }

        ///////////////////////////////////////////////////////////////////////////////////// Variable definition

        private String fileName = "lang.xml"; // 想要读取的文档名称
        private XmlDocument xmlDoc = null; // 想要读取的文档引用
        private XmlNodeList xmlNodeList = null; // 文档中所有节点的列表

        private HashSet<String> allWords = new HashSet<String>(); // 辅助：当前语言 - 翻译列表

        ///////////////////////////////////////////////////////////////////////////////////// getters
        public XmlNodeList getXmlNodeList() { return this.xmlNodeList; }

        ///////////////////////////////////////////////////////////////////////////////////// functions

        /// <summary>
        /// 增添一个翻译条目
        /// </summary>
        /// <param name="id"> 新字典条目ID </param>
        /// <param name="dic"> 内容 </param>
        public OptResult addItem(String id, Dictionary<LanguageEnum, String> dic)
        {
            OptResult result = new OptResult(true);
            // 首先检测是否有重复，否则可能会出现重复数据
            foreach (var item in dic) 
            {
                if (allWords.Contains(item.Value))
                {
                    result.result = false;
                    result.info = "Word \"" + item.Value + "\" already exists.";
                    return result;
                }
            }
            try
            {
                XmlElement newEle = xmlDoc.CreateElement("group");
                newEle.SetAttribute("id", id);
                foreach (var item in dic)
                {
                    XmlElement temp = xmlDoc.CreateElement(item.Key.ToString().ToLower());
                    temp.InnerText = item.Value;
                    newEle.AppendChild(temp);
                }
                xmlDoc.DocumentElement.AppendChild(newEle);
                xmlDoc.Save(fileName);
                foreach (var item in dic)
                {
                    allWords.Add(item.Value);
                }
            }
            catch (Exception er) 
            {
                result.result = false;
                result.info = er.ToString();
            }
            return result;
        }

        /*
        public void addLanguage(LanguageEnum lang)
        {
            // 首先检测是否有重复，否则可能会出现重复数据 todo
            XmlElement newEle = xmlDoc.CreateElement("group");
            newEle.SetAttribute("id", id);
            foreach (var item in dic)
            {
                XmlElement temp = xmlDoc.CreateElement(item.Key.ToString().ToLower());
                temp.InnerText = item.Value;
                newEle.AppendChild(temp);
            }
            xmlDoc.DocumentElement.AppendChild(newEle);
            xmlDoc.Save(fileName);
        }*/

        /// <summary>
        /// 修改一个翻译条目
        /// </summary>
        /// <param name="id"> 字典条目ID </param>
        /// <param name="dic"> 想要更新的内容 </param>
        public OptResult editItem(String id, Dictionary<LanguageEnum, String> dic)
        {
            OptResult result = new OptResult(true);
            try
            {
                XmlNode node = xmlDoc.SelectSingleNode("//group[@id='" + id + "']");
                if (node != null)
                {
                    foreach (var item in dic)
                    {
                        XmlNode temp = node.SelectSingleNode(item.Key.ToString().ToLower());
                        allWords.Add(item.Value);
                        allWords.Remove(temp.InnerText.Trim());
                        temp.InnerText = item.Value;
                    }
                    xmlDoc.Save(fileName);
                }
                else
                {
                    result.result = false;
                    result.info = "'id = " + id + "' is not found.";
                }
            }
            catch (Exception er)
            {
                result.result = false;
                result.info = er.ToString();
            }
            return result;
        }

        /// <summary>
        /// 删除一个翻译条目
        /// </summary>
        /// <param name="id"> 想要删除的字典条目ID </param>
        public OptResult delItem(String id)
        {            
            OptResult result = new OptResult(true);
            try
            {
                XmlNode node = xmlDoc.SelectSingleNode("//group[@id='" + id + "']");
                if (node != null)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        allWords.Remove(childNode.InnerText.Trim());
                    }
                    xmlDoc.DocumentElement.RemoveChild(node);
                    xmlDoc.Save(fileName);
                }
                else
                {
                    result.result = false;
                    result.info = "'id = " + id + "' is not found.";
                }
            }
            catch (Exception er)
            {
                result.result = false;
                result.info = er.ToString();
            }
            return result;
        }

        public OptResult searchByID(String id, LanguageEnum to)
        {
            OptResult result = new OptResult(true);
            try
            {
                XmlNode node = xmlDoc.SelectSingleNode("//group[@id='" + id + "']");
                if (node != null)
                {
                    result.info = node.SelectSingleNode(to.ToString().ToLower()).InnerText.Trim();
                }
                else
                {
                    result.result = false;
                    result.info = "'id = " + id + "' is not found.";
                }
            }
            catch (Exception er)
            {
                result.result = false;
                result.info = er.ToString();
            }
            return result;
        }
        public OptResult searchByLanguage(String word, LanguageEnum from, LanguageEnum to)
        {
            OptResult result = new OptResult(false);
            try
            {
                XmlNodeList nodeList = xmlDoc.SelectNodes("//group");
                foreach (XmlNode node in nodeList)
                {
                    if (node.SelectSingleNode(from.ToString().ToLower()).InnerText.Trim().Equals(word))
                    {
                        result.info = node.SelectSingleNode(to.ToString().ToLower()).InnerText.Trim();
                        result.result = true;
                        return result;
                    }
                }
            }
            catch (Exception er)
            {
                result.result = false;
                result.info = er.ToString();
            }
            return result;
        }

    }
}
