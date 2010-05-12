using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace BlackBox
{
    public static class SerializationExtension
    {
        public static XNode ToXml(this object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            
            using(var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
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
            var serializer = new DataContractSerializer(type);             
            var  xmlReader = XmlReader.Create(new StringReader(xml));
            return serializer.ReadObject(xmlReader);
        }

        public static object Copy(this object obj)
        {
            return obj.ToXml().ToString().Deserialize(obj.GetType());
        }
    }
}