using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SMS_Speed.Utility
{
    class Config
    {
        public static Dictionary<string, string> ReadConfigFile()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument docXML = new XmlDocument();
            docXML.Load(Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + @".exe.config");
            XmlNodeList lsNode;
            lsNode = docXML.GetElementsByTagName("setting");
            foreach (XmlNode node in lsNode)
            {
                string outterXML = node.OuterXml;
                string[] lsName = outterXML.Split('"');
                string name = lsName[1];
                string value = node.InnerText;
                dic.Add(name, value);
            }
            return dic;
        }

        public static string WriteConfigFile(Dictionary<string, string> dic,out bool result)
        {
            result = false;
            try
            {
                XmlDocument docXML = new XmlDocument();
                docXML.Load(Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + @".exe.config");
                XmlNodeList lsNode;
                lsNode = docXML.GetElementsByTagName("setting");
                foreach (XmlNode node in lsNode)
                {
                    string outterXML = node.OuterXml;
                    string[] lsName = outterXML.Split('"');
                    string name = lsName[1];
                    node.InnerXml = "<value>" + dic[name] + "</value>";
                }
                docXML.Save(Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + @".exe.config");
                result = true;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
