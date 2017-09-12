using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FrontFramework.Utils
{
    class XmlBasedPropUtil : PropertyUtilInterface
    {
        private static XmlBasedPropUtil instance = new XmlBasedPropUtil();
        private XmlBasedPropUtil(){}
        public static XmlBasedPropUtil getInstance() 
        {
            return instance; 
        }


        private static String fileName;
        private static XmlDocument xmlDoc;
        private static Dictionary<String, String> props = new Dictionary<string, string>();
        public OptResult loadProperty(Object obj) 
        {
            OptResult result = new OptResult(true);
            try
            {
                fileName = (String)obj;
                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlNodeList nodeList = xmlDoc.SelectNodes("//property");
                foreach (XmlElement node in nodeList)
                {
                    if (node.GetAttribute("type") == null || !node.GetAttribute("type").Equals("list"))
                    {
                        props.Add(node.GetAttribute("id"), node.InnerText.Trim());
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
        public OptResult reloadProperty()
        {
            OptResult result = new OptResult(true);
            props.Clear();
            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlNodeList nodeList = xmlDoc.SelectNodes("//property");
                foreach (XmlElement node in nodeList)
                {
                    if (node.GetAttribute("type") == null || !node.GetAttribute("type").Equals("list"))
                    {
                        props.Add(node.GetAttribute("id"), node.InnerText.Trim());
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
        public OptResult getStrProp(String name)
        {
            OptResult result = new OptResult(true);
            if (props.ContainsKey(name))
            {
                result.info = props[name];
            }
            else 
            {
                result.result = false;
            }
            return result;
        }
        public OptResult setStrProp(String name, String value) 
        {
            OptResult result = new OptResult(true);
            try
            {
                XmlElement node = (XmlElement)xmlDoc.SelectSingleNode("//property[@id='" + name + "']");
                node.InnerText = value;
                xmlDoc.Save(fileName);
            }
            catch(Exception er)
            {
                result.result = false;
                result.info = er.ToString();
            }
            return result;
        }

        public Dictionary<String, Dictionary<String, String>> getListStrProp(String name) 
        {
            return getListStrProp(name, null, null);
        }
        public Dictionary<String, Dictionary<String, String>> getListStrProp(String name, String propName, String propVal)
        {
            Dictionary<String, Dictionary<String, String>> rVal = new Dictionary<String, Dictionary<String, String>>();
            XmlNodeList itemList = null;
            if (propName != null)
            {
                itemList = (XmlNodeList)xmlDoc.SelectNodes(
                    "//property[@type='list'][@" + propName + "='" + propVal + "']/item");
            }
            else
            {
                itemList = (XmlNodeList)xmlDoc.SelectNodes("//property[@type='list']/item");
            }
            foreach (XmlNode node in itemList) 
            {
                Dictionary<String, String> temp = new Dictionary<String, String>();
                foreach (XmlAttribute attr in ((XmlElement)node).Attributes) 
                {
                    temp.Add(attr.Name, attr.Value);
                }
                rVal.Add(node.InnerText.Trim(), temp);
            }
            return rVal;
        }

    }
}
