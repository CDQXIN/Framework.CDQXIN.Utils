using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Framework.CDQXIN.Utils
{
    public class XmlHelper
    {
        public static string Serialize<T>(T entity)
        {
            string xml;
            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(ms);
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8
                }))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                    xmlNameSpace.Add("", "");
                    serializer.Serialize(writer, entity, xmlNameSpace);
                    writer.Flush();
                    writer.Close();
                }
                using (StreamReader sr = new StreamReader(ms))
                {
                    ms.Position = 0L;
                    xml = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return xml;
        }
        public static string Serialize(Type type, object entity)
        {
            string xml;
            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(ms);
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8
                }))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                    xmlNameSpace.Add("", "");
                    serializer.Serialize(writer, entity, xmlNameSpace);
                    writer.Flush();
                    writer.Close();
                }
                using (StreamReader sr = new StreamReader(ms))
                {
                    ms.Position = 0L;
                    xml = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return xml;
        }
        public static T DeSerialze<T>(string xml, bool catchEx = false)
        {
            T t = default(T);
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                XmlReaderSettings settings = new XmlReaderSettings();
                using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                    {
                        object obj = xmlSerializer.Deserialize(xmlReader);
                        t = (T)((object)obj);
                    }
                }
            }
            catch (Exception e)
            {
                if (!catchEx)
                    throw e;
            }

            return t;
        }
        public static object DeSerialze(Type type, string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            XmlReaderSettings settings = new XmlReaderSettings();
            object obj = null;
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    obj = xmlSerializer.Deserialize(xmlReader);
                }
            }
            return obj;
        }

        /// <summary>
        /// 获取节点指定属性值
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <param name="attrName">属性名</param>
        /// <returns></returns>
        public static string GetNodeAttr(XmlNode node, string attrName)
        {
            foreach (XmlAttribute xmlAttribute in node.Attributes)
            {
                if (xmlAttribute.Name.ToLower() == attrName.ToLower())
                {
                    return (xmlAttribute.Value == null) ? string.Empty : xmlAttribute.Value.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取节点指定节点
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <param name="nodeName">xml节点名</param>
        /// <returns></returns>
        public static XmlNode GetXmlNode(XmlNode node, string nodeName)
        {
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                if (xmlNode.LocalName.ToLower() == nodeName.ToLower())
                {
                    return xmlNode;
                }
            }
            return null;
        }

        /// <summary>
        /// 读取xml文件的指定节点
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="nodeLocalName">节点名称</param>
        /// <returns></returns>
        public static XmlNode GetXmlNodeFromFile(string filepath, string nodeLocalName)
        {
            if (File.Exists(filepath))
            {
                XmlDocument xmldocument = new XmlDocument();
                FileStream fileStream = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                xmldocument.Load(fileStream);
                fileStream.Close();
                fileStream.Dispose();
                return xmldocument.SelectSingleNode(nodeLocalName);
            }
            return null;
        }

        public T DeSerializeToObject<T>(string message)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(new StringReader(message)))
            {
                if (xmlSerializer.CanDeserialize(reader))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
            return default;
        }
    }
}
