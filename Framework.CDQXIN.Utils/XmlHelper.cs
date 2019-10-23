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
    }
}
