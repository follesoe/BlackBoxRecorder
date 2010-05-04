using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BlackBox
{
    public static class SerializationExtension
    {
        public static XNode ToXml(this object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            using(var sw = new StringWriter())
            {
                serializer.Serialize(sw, obj);
                var xml = XDocument.Parse(sw.ToString());
                return xml.FirstNode;
            }            
        }

        public static object Deserialize(this string xml, Type type)
        {
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(new StringReader(xml));
        }

        public static object Copy(this object obj)
        {
            return obj.ToXml().ToString().Deserialize(obj.GetType());
        }
    }
}