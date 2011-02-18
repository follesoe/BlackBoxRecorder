using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BlackBox
{
    public static class SerializationExtension
    {
        public static XNode ToXml(this object obj)
        {
            if (obj == null) return null;

            var serializer = new XmlSerializer(obj.GetType());
            
            using(var ms = new MemoryStream())
            {
                serializer.Serialize(ms, obj);
                ms.Flush();
                ms.Position = 0;
                
                using(var sr = new StreamReader(ms))
                {
                    string xmlString = sr.ReadToEnd();
                    var xml = XDocument.Parse(xmlString);
                    return xml.FirstNode;
                }
            }
        }

        public static object Deserialize(this string xml, Type type)
        {
            var serializer = new XmlSerializer(type);             
            var  xmlReader = XmlReader.Create(new StringReader(xml));
            return serializer.Deserialize(xmlReader);
        }

        public static object Copy(this object obj)
        {
            return obj.ToXml().ToString().Deserialize(obj.GetType());
        }
    }
}